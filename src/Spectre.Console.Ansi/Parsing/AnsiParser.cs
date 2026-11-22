namespace Spectre.Console.Ansi;

/// <summary>
/// An ANSI/VT parser based on the VT500 series.
/// </summary>
/// <remarks>
/// <para>
/// Instances are stateful and not thread-safe: a single parser must not be used from
/// multiple threads, and the callback must not re-enter <see cref="Next(char)"/> or
/// <see cref="Next(string)"/> on the same instance.
/// </para>
/// <para>
/// A sequence that goes over a limit is dropped instead of being dispatched cut off. The
/// limits are 32 parameters (subparameters included) and 256 parameter characters for CSI and
/// DCS, 2 intermediates (a private marker such as ? counts as one), and 2048 characters for an
/// OSC string.
/// </para>
/// </remarks>
[Experimental(Constants.ExperimentalParserId)]
public sealed class AnsiParser
{
    private const int ReplacementCodepoint = 0xFFFD;
    private const int MaxParameterValue = 65535;
    private const int MaxParameterCount = 32;
    private const int MaxParametersRawLength = 256;
    private const int MaxIntermediateCount = 2;
    private const char EscapeCharacter = '\u001B';
    private const string PasteEnd = "\u001B[201~";

    private readonly Action<AnsiToken> _callback;
    private readonly AnsiParserMode _mode;
    private readonly List<char> _intermediates = [];
    private readonly List<int> _parameters = [0];
    private readonly List<bool> _isSubparameter = [false];
    private readonly StringBuilder _parametersRaw = new();
    private readonly StringBuilder _paste = new();
    private readonly OscParser _oscParser;
    private bool _hasParameter;
    private bool _ignoring;
    private bool _escapeEndsString;
    private bool _pasting;
    private char _pendingIntroducer;
    private char _highSurrogate;
    private AnsiParserState _currentState;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiParser"/> class.
    /// </summary>
    /// <param name="callback">The callback to be used for parsed tokens.</param>
    /// <param name="mode">The kind of stream to parse.</param>
    public AnsiParser(Action<AnsiToken> callback, AnsiParserMode mode = AnsiParserMode.Output)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _mode = mode;
        _currentState = AnsiParserState.Ground;
        _oscParser = new OscParser();
    }

    /// <summary>
    /// Processes the specified text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    public void Next(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        foreach (var character in text)
        {
            Next(character);
        }
    }

    /// <summary>
    /// Processes the specified code.
    /// </summary>
    /// <param name="code">The code to process.</param>
    public void Next(char code)
    {
        // A stashed high surrogate must be immediately followed by a low surrogate to form a
        // scalar value. If the next character is anything else, the high surrogate was unpaired
        if (_highSurrogate != '\0' && !char.IsLowSurrogate(code))
        {
            _highSurrogate = '\0';
            _callback(new AnsiToken.Print(ReplacementCodepoint));
        }

        // An introducer with nothing after it, such as ESC [, can be a key until more arrives
        _pendingIntroducer = '\0';

        if (_mode == AnsiParserMode.Input && NextInput(code))
        {
            return;
        }

        if (_currentState == AnsiParserState.Escape)
        {
            // A backslash after an ESC that ended a string completes its terminator (ESC \).
            // The string has already been dispatched, so there is nothing more to emit
            if (_escapeEndsString && code == '\\')
            {
                _currentState = AnsiParserState.Ground;
                return;
            }

            // An ESC followed by another ESC, or by a surrogate, starts no sequence (a final is a
            // single UTF-16 code unit, so it can't hold an astral character). Abandon the ESC and
            // handle this character from the ground state
            if (code == EscapeCharacter || char.IsSurrogate(code))
            {
                AbandonEscape();
            }
        }

        var (nextState, action) = AnsiTransitionTable.Shared.GetTransition(_currentState, code);

        // A DCS that went over a limit is ignored up to its terminator
        if (_ignoring && nextState == AnsiParserState.DcsPassthrough)
        {
            nextState = AnsiParserState.DcsIgnore;
        }

        // A C1 control that starts a sequence leaves the current state even when it
        // starts the same kind of sequence again, so the exit and entry actions run
        var changesState = _currentState != nextState || StartsSequence(code);

        // Perform the exit action of the current state
        if (changesState)
        {
            switch (_currentState)
            {
                case AnsiParserState.OscString:
                    // CAN and SUB abort the string; only a normal terminator (ST/BEL)
                    // dispatches the accumulated command.
                    if (!IsAbort(code))
                    {
                        var command = _oscParser.End();
                        if (command != null)
                        {
                            _callback(new AnsiToken.Osc(Command: command));
                        }
                    }

                    break;
                case AnsiParserState.DcsPassthrough:
                    _callback(new AnsiToken.DcsUnhook());
                    break;
            }
        }

        // Perform the transition action
        switch (action)
        {
            case AnsiTransitionAction.None:
            case AnsiTransitionAction.Ignore:
                // Do nothing
                break;
            case AnsiTransitionAction.Print:
                EmitPrint(code);
                break;
            case AnsiTransitionAction.Execute:
                _callback(new AnsiToken.Execute(Function: code));
                break;
            case AnsiTransitionAction.Collect:
                if (_intermediates.Count == MaxIntermediateCount)
                {
                    _ignoring = true;
                }
                else
                {
                    _intermediates.Add(code);
                }

                break;
            case AnsiTransitionAction.Param:
                // Past a limit, stop collecting; the sequence is dropped at its final byte
                if (_ignoring ||
                    _parametersRaw.Length == MaxParametersRawLength ||
                    ((code is ';' or ':') && _parameters.Count == MaxParameterCount))
                {
                    _ignoring = true;
                    break;
                }

                _parametersRaw.Append(code);

                // A separator marks a parameter position, so set this for separators too.
                // An all-empty section like "ESC [ ; H" then reports its default positions
                // instead of collapsing to no params
                _hasParameter = true;

                if (code is ';' or ':')
                {
                    // A colon starts a subparameter of the current parameter, as in "4:3"
                    _parameters.Add(0);
                    _isSubparameter.Add(code == ':');
                }
                else
                {
                    Debug.Assert(char.IsDigit(code), "Expected digit");

                    var accumulator = (_parameters[^1] * 10L) + (code - 48);
                    _parameters[^1] = accumulator > MaxParameterValue ? MaxParameterValue : (int)accumulator;
                }

                break;
            case AnsiTransitionAction.EscDispatch:
                if (!_ignoring)
                {
                    _callback(new AnsiToken.Esc(
                        Intermediates: [.. _intermediates],
                        Final: code));
                }

                break;
            case AnsiTransitionAction.CsiDispatch:
                if (_mode == AnsiParserMode.Input && !_ignoring && IsPasteStart(code))
                {
                    // The text up to CSI 201 ~ is pasted, not parsed
                    _pasting = true;
                }
                else if (!_ignoring)
                {
                    _callback(new AnsiToken.Csi(
                        Intermediates: [.. _intermediates],
                        Params: GetParameters(),
                        Final: code,
                        ParamsRaw: _parametersRaw.ToString()));
                }

                break;
            case AnsiTransitionAction.OscPut:
                _oscParser.Next(code);
                break;
            case AnsiTransitionAction.DcsPut:
                _callback(new AnsiToken.DcsPut(Code: code));
                break;
        }

        // Perform the entry action of the next state
        if (changesState)
        {
            switch (nextState)
            {
                case AnsiParserState.Escape:
                    _escapeEndsString = EndsString(_currentState);
                    Clear();
                    break;
                case AnsiParserState.DcsEntry:
                case AnsiParserState.CsiEntry:
                    Clear();
                    break;
                case AnsiParserState.OscString:
                    _oscParser.Reset();
                    break;
                case AnsiParserState.DcsPassthrough:
                    _callback(new AnsiToken.DcsHook(
                        Intermediates: [.. _intermediates],
                        Params: GetParameters(),
                        Final: code,
                        ParamsRaw: _parametersRaw.ToString()));
                    break;
            }

            // In terminal input, ESC and an introducer is also a key pressed with Alt, such as
            // Alt+[. Remember the introducer, so Flush can report the key if nothing follows
            if (_mode == AnsiParserMode.Input &&
                _currentState == AnsiParserState.Escape && !_escapeEndsString &&
                nextState is AnsiParserState.CsiEntry or AnsiParserState.DcsEntry
                    or AnsiParserState.OscString or AnsiParserState.SosPmApcString)
            {
                _pendingIntroducer = code;
            }
        }

        _currentState = nextState;
    }

    /// <summary>
    /// Emits what the parser holds back while it waits for more input: a trailing unpaired high
    /// surrogate, as the Unicode replacement character, and a lone ESC, as an
    /// <see cref="AnsiToken.Execute"/> token for ESC (0x1B).
    /// </summary>
    /// <remarks>
    /// Call this at the end of the input, or when an interactive input source has nothing more
    /// to read, so that a single press of the Escape key is reported. In
    /// <see cref="AnsiParserMode.Input"/>, ESC and an introducer with nothing after it, such as
    /// ESC [, comes out as the key it is: an <see cref="AnsiToken.Esc"/> token for Alt+[. Any
    /// other partially parsed sequence, a bracketed paste, and the ESC that ends an OSC string
    /// are kept so the rest can still arrive.
    /// </remarks>
    public void Flush()
    {
        if (_highSurrogate != '\0')
        {
            _highSurrogate = '\0';
            _callback(new AnsiToken.Print(ReplacementCodepoint));
        }

        // An ESC that ended a string still waits for the backslash of its terminator
        if (_currentState == AnsiParserState.Escape && !_escapeEndsString)
        {
            AbandonEscape();
        }

        if (_pendingIntroducer == '\0')
        {
            return;
        }

        _currentState = AnsiParserState.Ground;
        _callback(new AnsiToken.Esc(Intermediates: [], Final: _pendingIntroducer));
        _pendingIntroducer = '\0';
    }

    /// <summary>
    /// Resets the parser to its initial ground state, discarding any partially parsed sequence
    /// and buffered state. Use this to recover from malformed input or to reuse the instance for
    /// an unrelated stream. No tokens are emitted.
    /// </summary>
    public void Reset()
    {
        _currentState = AnsiParserState.Ground;
        _highSurrogate = '\0';
        _pendingIntroducer = '\0';
        _pasting = false;
        _paste.Clear();
        _oscParser.Reset();

        Clear();
    }

    private bool NextInput(char code)
    {
        // Terminal input differs from output where a key pressed with Alt arrives as ESC and the
        // key, and for SS3 keys, Backspace and bracketed paste. Returns whether the code is handled

        while (true)
        {
            if (_pasting)
            {
                CollectPaste(code);
                return true;
            }

            switch (_currentState)
            {
                case AnsiParserState.Escape when IsAltKey(code):
                    _currentState = AnsiParserState.Ground;

                    // An ESC that ended a string isn't a key, so handle the code without it
                    if (_escapeEndsString)
                    {
                        continue;
                    }

                    _callback(new AnsiToken.Esc(Intermediates: [], Final: code));
                    return true;
                case AnsiParserState.Escape when code == 'O' && !_escapeEndsString:
                    // SS3, which arrow keys, F1 to F4 and the keypad can send
                    _currentState = AnsiParserState.Ss3;
                    _pendingIntroducer = code;
                    return true;
                case AnsiParserState.Ss3:
                    _currentState = AnsiParserState.Ground;
                    if (code is >= '@' and <= '~')
                    {
                        _callback(new AnsiToken.Ss3(Final: code));
                        return true;
                    }

                    // Without a final, it was Alt+O; handle the code from the ground state
                    _callback(new AnsiToken.Esc(Intermediates: [], Final: 'O'));
                    continue;
                case AnsiParserState.Ground when code == '\u007F':
                    // DEL is what the Backspace key sends
                    _callback(new AnsiToken.Execute(Function: code));
                    return true;
                default:
                    return false;
            }
        }
    }

    private void CollectPaste(char code)
    {
        _paste.Append(code);

        // The paste ends at CSI 201 ~, which isn't part of the pasted text
        if (code == '~' && EndsWith(_paste, PasteEnd))
        {
            _paste.Length -= PasteEnd.Length;
            _pasting = false;
            _callback(new AnsiToken.Paste(Text: _paste.ToString()));
            _paste.Clear();
        }
    }

    private bool IsPasteStart(char final)
    {
        // CSI 200 ~ starts a bracketed paste
        return final == '~'
               && _intermediates.Count == 0
               && _parameters.Count == 1
               && _parameters[0] == 200;
    }

    private void Clear()
    {
        _hasParameter = false;
        _ignoring = false;
        _parametersRaw.Clear();
        _parameters.Clear();
        _parameters.Add(0);
        _isSubparameter.Clear();
        _isSubparameter.Add(false);
        _intermediates.Clear();
    }

    private IReadOnlyList<AnsiParameter> GetParameters()
    {
        if (!_hasParameter)
        {
            return [];
        }

        var parameters = new List<AnsiParameter>();
        var index = 0;
        while (index < _parameters.Count)
        {
            var value = _parameters[index++];

            // The values after it that follow a colon are its subparameters
            var start = index;
            while (index < _parameters.Count && _isSubparameter[index])
            {
                index++;
            }

            IReadOnlyList<int> subparameters = index == start
                ? []
                : _parameters.GetRange(start, index - start).ToArray();

            parameters.Add(new AnsiParameter(value, subparameters));
        }

        return [.. parameters];
    }

    private void EmitPrint(char code)
    {
        if (char.IsHighSurrogate(code))
        {
            // Wait for the trailing low surrogate before emitting a scalar value
            _highSurrogate = code;
            return;
        }

        if (char.IsLowSurrogate(code))
        {
            if (_highSurrogate != '\0')
            {
                _callback(new AnsiToken.Print(char.ConvertToUtf32(_highSurrogate, code)));
                _highSurrogate = '\0';
            }
            else
            {
                // Low surrogate without a preceding high surrogate
                _callback(new AnsiToken.Print(ReplacementCodepoint));
            }

            return;
        }

        _callback(new AnsiToken.Print(code));
    }

    private void AbandonEscape()
    {
        // Leaves an ESC that starts no sequence. A lone ESC, such as a press of the Escape key, is
        // reported. An ESC that ended a string was the start of its terminator, so it's dropped
        _currentState = AnsiParserState.Ground;

        if (!_escapeEndsString)
        {
            _callback(new AnsiToken.Execute(Function: EscapeCharacter));
        }
    }

    private static bool EndsString(AnsiParserState state)
    {
        // An ESC inside a string normally starts the string terminator (ESC \)
        return state
            is AnsiParserState.OscString
            or AnsiParserState.SosPmApcString
            or AnsiParserState.DcsEntry
            or AnsiParserState.DcsParam
            or AnsiParserState.DcsIntermediate
            or AnsiParserState.DcsPassthrough
            or AnsiParserState.DcsIgnore;
    }

    private static bool IsAbort(char code)
    {
        // CAN and SUB abort any in-progress sequence
        return code
            is '\u0018' // CAN (0x18)
            or '\u001A'; // SUB (0x1A)
    }

    private static bool StartsSequence(char code)
    {
        return code
            is '\u0090' // DCS
            or '\u0098' // SOS
            or '\u009B' // CSI
            or '\u009D' // OSC
            or '\u009E' // PM
            or '\u009F'; // APC
    }

    private static bool IsAltKey(char code)
    {
        // In terminal input, these start no sequence after ESC: the controls (but ESC), the
        // characters 0x20 to 0x2F, DEL, and the SOS and PM introducers, which no reply uses
        return code
            is (< ' ' and not EscapeCharacter)
            or (>= ' ' and <= '/')
            or '\u007F'
            or 'X'
            or '^';
    }

    private static bool EndsWith(StringBuilder builder, string value)
    {
        if (builder.Length < value.Length)
        {
            return false;
        }

        var start = builder.Length - value.Length;
        for (var index = 0; index < value.Length; index++)
        {
            if (builder[start + index] != value[index])
            {
                return false;
            }
        }

        return true;
    }
}