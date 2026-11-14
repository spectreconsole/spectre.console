namespace Spectre.Console.Ansi;

/// <summary>
/// An ANSI/VT input parser based on the VT500-series.
/// </summary>
/// <remarks>
/// Instances are stateful and not thread-safe: a single parser must not be used from
/// multiple threads, and the callback must not re-enter <see cref="Next(char)"/> or
/// <see cref="Next(string)"/> on the same instance.
/// </remarks>
public sealed class AnsiParser
{
    private const int ReplacementCodepoint = 0xFFFD;
    private const int MaxParameterValue = 65535;
    private const char EscapeCharacter = '\u001B';

    private readonly Action<AnsiToken> _callback;
    private readonly List<char> _intermediates = [];
    private readonly List<int> _parameters = [0];
    private readonly List<bool> _isSubparameter = [false];
    private readonly StringBuilder _parametersRaw = new();
    private readonly OscParser _oscParser;
    private bool _hasParameter;
    private bool _escapeEndsString;
    private char _highSurrogate;
    private AnsiParserState _currentState;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiParser"/> class.
    /// </summary>
    /// <param name="callback">The callback to be used for parsed tokens.</param>
    public AnsiParser(Action<AnsiToken> callback)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _currentState = AnsiParserState.Ground;
        _oscParser = new OscParser();
    }

    /// <summary>
    /// Processes the specified text.
    /// </summary>
    /// <param name="text">The text to process.</param>
    public void Next(string text)
    {
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

        // An ESC followed by another ESC, or by a surrogate, starts no sequence (a final is a
        // single UTF-16 code unit, so it can't hold an astral character). Abandon the ESC and
        // handle this character from the ground state
        if (_currentState == AnsiParserState.Escape &&
            (code == EscapeCharacter || char.IsSurrogate(code)))
        {
            AbandonEscape();
        }

        var (nextState, action) = AnsiTransitionTable.Shared.GetTransition(_currentState, code);

        // Perform the exit action of the current state
        if (_currentState != nextState)
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
                _intermediates.Add(code);
                break;
            case AnsiTransitionAction.Param:
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
                _callback(new AnsiToken.Esc(
                    Intermediates: [.. _intermediates],
                    Final: code));
                break;
            case AnsiTransitionAction.CsiDispatch:
                _callback(new AnsiToken.Csi(
                    Intermediates: [.. _intermediates],
                    Params: GetParameters(),
                    Final: code,
                    ParamsRaw: _parametersRaw.ToString()));
                break;
            case AnsiTransitionAction.OscPut:
                _oscParser.Next(code);
                break;
            case AnsiTransitionAction.DscPut:
                _callback(new AnsiToken.DcsPut(Code: code));
                break;
        }

        // Perform the entry action of the next state
        if (_currentState != nextState)
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
    /// to read, so that a single press of the Escape key is reported. A partially parsed
    /// sequence, such as ESC [ or the ESC that ends an OSC string, is kept so the rest of it can
    /// still arrive.
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
        _oscParser.Reset();
        Clear();
    }

    private void Clear()
    {
        _hasParameter = false;
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

    // Leaves an ESC that starts no sequence. A lone ESC, such as a press of the Escape key, is
    // reported. An ESC that ended a string was the start of its terminator, so it's dropped
    private void AbandonEscape()
    {
        _currentState = AnsiParserState.Ground;

        if (!_escapeEndsString)
        {
            _callback(new AnsiToken.Execute(Function: EscapeCharacter));
        }
    }

    private static bool EndsString(AnsiParserState state)
    {
        // An ESC inside a string normally starts the string terminator (ESC \)
        return state is AnsiParserState.OscString
            or AnsiParserState.SosPmApcString
            or AnsiParserState.DcsEntry
            or AnsiParserState.DcsParam
            or AnsiParserState.DcsIntermediate
            or AnsiParserState.DcsPassthrough
            or AnsiParserState.DcsIgnore;
    }

    private static bool IsAbort(char code)
    {
        // CAN (0x18) and SUB (0x1A) abort any in-progress
        // sequence per the VT500 state machine
        return code is '\u0018' or '\u001A';
    }
}