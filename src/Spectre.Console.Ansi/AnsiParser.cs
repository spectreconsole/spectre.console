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

    private readonly Action<AnsiToken> _callback;
    private readonly List<char> _intermediates = [];
    private readonly List<int> _parameters = [0];
    private readonly StringBuilder _parametersRaw = new();
    private readonly OscParser _oscParser;
    private bool _hasParameter;
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
                        var command = _oscParser.End(code);
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
                    _parameters.Add(0);
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
                    Params: _hasParameter ? [.. _parameters] : [],
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
                        Params: _hasParameter ? [.. _parameters] : [],
                        Final: code,
                        ParamsRaw: _parametersRaw.ToString()));
                    break;
            }
        }

        _currentState = nextState;
    }

    private void Clear()
    {
        _hasParameter = false;
        _parametersRaw.Clear();
        _parameters.Clear();
        _parameters.Add(0);
        _intermediates.Clear();
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

    private static bool IsAbort(char code)
    {
        // CAN (0x18) and SUB (0x1A) abort any in-progress
        // sequence per the VT500 state machine
        return code is '\u0018' or '\u001A';
    }
}

internal enum AnsiParserState
{
    Ground = 0,
    Escape,
    EscapeIntermediate,
    CsiEntry,
    CsiIntermediate,
    CsiParam,
    CsiIgnore,
    DcsEntry,
    DcsParam,
    DcsIntermediate,
    DcsPassthrough,
    DcsIgnore,
    OscString,
    SosPmApcString,
}

internal enum AnsiTransitionAction
{
    None = 0,
    Ignore,
    Print,
    Execute,
    Collect,
    Param,
    EscDispatch,
    CsiDispatch,
    OscPut,
    DscPut,
}

internal readonly record struct AnsiTransition(
    AnsiParserState State,
    AnsiTransitionAction Action);

internal sealed class AnsiTransitionTable
{
    private readonly Dictionary<int, Dictionary<AnsiParserState, AnsiTransition>> _transitions;

    public static AnsiTransitionTable Shared { get; } = new();

    private AnsiTransitionTable()
    {
        _transitions = [];

        // Anywhere
        foreach (var state in EnumUtils.GetValues<AnsiParserState>())
        {
            // -> Ground
            Add(0x18, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x1A, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x80..0x8f, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x91..0x97, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x99, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x9A, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x9C, state, AnsiParserState.Ground, AnsiTransitionAction.None);

            // -> SosPmApcString
            Add(0x98, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Add(0x9E, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Add(0x9F, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);

            // -> Escape
            Add(0x1B, state, AnsiParserState.Escape, AnsiTransitionAction.None);

            // -> DcsEntry
            Add(0x90, state, AnsiParserState.DcsEntry, AnsiTransitionAction.None);

            // -> OscString
            Add(0x9D, state, AnsiParserState.OscString, AnsiTransitionAction.None);

            // -> CsiEntry
            Add(0x9B, state, AnsiParserState.CsiEntry, AnsiTransitionAction.None);
        }

        // Ground
        {
            Add(0x19, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Add(0x20..0x7F, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Print);
        }

        // EscapeIntermediate
        {
            Add(0x19, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate,
                AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate,
                AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate,
                AnsiTransitionAction.Execute);
            Add(0x20..0x2F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate,
                AnsiTransitionAction.Collect);
            Add(0x7F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate,
                AnsiTransitionAction.Ignore);

            // -> Ground
            Add(0x30..0x7E, AnsiParserState.EscapeIntermediate, AnsiParserState.Ground,
                AnsiTransitionAction.EscDispatch);
        }

        // Escape
        {
            Add(0x19, AnsiParserState.Escape, AnsiParserState.Escape, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.Escape, AnsiParserState.Escape, AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.Escape, AnsiParserState.Escape, AnsiTransitionAction.Execute);
            Add(0x7F, AnsiParserState.Escape, AnsiParserState.Escape, AnsiTransitionAction.Ignore);

            // -> EscapeIntermediate
            Add(0x20..0x2F, AnsiParserState.Escape, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Collect);

            // -> Ground
            Add(0x30..0x4F, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);
            Add(0x51..0x57, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);
            Add(0x59, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);
            Add(0x5A, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);
            Add(0x5C, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);
            Add(0x60..0x7E, AnsiParserState.Escape, AnsiParserState.Ground, AnsiTransitionAction.EscDispatch);

            // -> SosPmApcString
            Add(0x58, AnsiParserState.Escape, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Add(0x5E, AnsiParserState.Escape, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Add(0x5F, AnsiParserState.Escape, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);

            // -> DcsEntry
            Add(0x50, AnsiParserState.Escape, AnsiParserState.DcsEntry, AnsiTransitionAction.None);

            // -> OscString
            Add(0x5D, AnsiParserState.Escape, AnsiParserState.OscString, AnsiTransitionAction.None);

            // -> CsiEntry
            Add(0x5B, AnsiParserState.Escape, AnsiParserState.CsiEntry, AnsiTransitionAction.None);
        }

        // SosPmApcString
        {
            Add(0x19, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.Ignore);
            Add(0x20..0x7F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.Ignore);
        }

        // DcsEntry
        {
            Add(0x19, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Add(0x7F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);

            // -> DcsIntermediate
            Add(0x20..0x2F, AnsiParserState.DcsEntry, AnsiParserState.DcsIntermediate, AnsiTransitionAction.Collect);

            // -> DcsIgnore
            Add(0x3A, AnsiParserState.DcsEntry, AnsiParserState.DcsIgnore, AnsiTransitionAction.None);

            // -> DcsParam
            Add(0x30..0x39, AnsiParserState.DcsEntry, AnsiParserState.DcsParam, AnsiTransitionAction.Param);
            Add(0x3B, AnsiParserState.DcsEntry, AnsiParserState.DcsParam, AnsiTransitionAction.Param);
            Add(0x3C..0x3F, AnsiParserState.DcsEntry, AnsiParserState.DcsParam, AnsiTransitionAction.Collect);

            // -> DcsPassthrough
            Add(0x40..0x7E, AnsiParserState.DcsEntry, AnsiParserState.DcsPassthrough, AnsiTransitionAction.None);
        }

        // DcsIntermediate
        {
            Add(0x19, AnsiParserState.DcsIntermediate, AnsiParserState.DcsIntermediate, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.DcsIntermediate, AnsiParserState.DcsIntermediate,
                AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.DcsIntermediate, AnsiParserState.DcsIntermediate,
                AnsiTransitionAction.Ignore);
            Add(0x7F, AnsiParserState.DcsIntermediate, AnsiParserState.DcsIntermediate, AnsiTransitionAction.Ignore);

            // -> DscIgnore
            Add(0x30..0x3F, AnsiParserState.DcsIntermediate, AnsiParserState.DcsIgnore, AnsiTransitionAction.None);

            // -> DcsPassthrough
            Add(0x40..0x7E, AnsiParserState.DcsIntermediate, AnsiParserState.DcsPassthrough, AnsiTransitionAction.None);
        }

        // DcsIgnore
        {
            Add(0x19, AnsiParserState.DcsIgnore, AnsiParserState.DcsIgnore, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.DcsIgnore, AnsiParserState.DcsIgnore, AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.DcsIgnore, AnsiParserState.DcsIgnore, AnsiTransitionAction.Ignore);
            Add(0x20..0x7F, AnsiParserState.DcsIgnore, AnsiParserState.DcsIgnore, AnsiTransitionAction.Ignore);
        }

        // DcsParam
        {
            Add(0x19, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Ignore);
            Add(0x30..0x39, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Param);
            Add(0x3B, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Param);
            Add(0x7F, AnsiParserState.DcsParam, AnsiParserState.DcsParam, AnsiTransitionAction.Ignore);

            // -> DcsParam
            Add(0x3A, AnsiParserState.DcsParam, AnsiParserState.DcsIgnore, AnsiTransitionAction.None);
            Add(0x3C..0x3F, AnsiParserState.DcsParam, AnsiParserState.DcsIgnore, AnsiTransitionAction.None);

            // -> DcsIntermediate
            Add(0x20..0x2F, AnsiParserState.DcsParam, AnsiParserState.DcsIntermediate, AnsiTransitionAction.Collect);

            // -> DcsPassthrough
            Add(0x40..0x7E, AnsiParserState.DcsParam, AnsiParserState.DcsPassthrough, AnsiTransitionAction.None);
        }

        // DcsPassthrough
        {
            Add(0x19, AnsiParserState.DcsPassthrough, AnsiParserState.DcsPassthrough, AnsiTransitionAction.DscPut);
            Add(0x00..0x17, AnsiParserState.DcsPassthrough, AnsiParserState.DcsPassthrough,
                AnsiTransitionAction.DscPut);
            Add(0x1C..0x1F, AnsiParserState.DcsPassthrough, AnsiParserState.DcsPassthrough,
                AnsiTransitionAction.DscPut);
            Add(0x20..0x7E, AnsiParserState.DcsPassthrough, AnsiParserState.DcsPassthrough,
                AnsiTransitionAction.DscPut);
            Add(0x7F, AnsiParserState.DcsPassthrough, AnsiParserState.DcsPassthrough, AnsiTransitionAction.Ignore);
        }

        // CsiParam
        {
            Add(0x19, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Execute);
            Add(0x30..0x39, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Param);
            Add(0x3B, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Param);
            Add(0x7F, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Ignore);

            // -> CsiIgnore (0x3A diffs from spec, but needed for ':')
            Add(0x3C..0x3F, AnsiParserState.CsiParam, AnsiParserState.CsiIgnore, AnsiTransitionAction.None);
            Add(0x3A, AnsiParserState.CsiParam, AnsiParserState.CsiParam, AnsiTransitionAction.Param);

            // -> CsiIntermediate
            Add(0x20..0x2F, AnsiParserState.CsiParam, AnsiParserState.CsiIntermediate, AnsiTransitionAction.Collect);

            // -> Ground
            Add(0x40..0x7E, AnsiParserState.CsiParam, AnsiParserState.Ground, AnsiTransitionAction.CsiDispatch);
        }

        // CsiIgnore
        {
            Add(0x19, AnsiParserState.CsiIgnore, AnsiParserState.CsiIgnore, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.CsiIgnore, AnsiParserState.CsiIgnore, AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.CsiIgnore, AnsiParserState.CsiIgnore, AnsiTransitionAction.Execute);
            Add(0x20..0x3F, AnsiParserState.CsiIgnore, AnsiParserState.CsiIgnore, AnsiTransitionAction.Ignore);
            Add(0x7F, AnsiParserState.CsiIgnore, AnsiParserState.CsiIgnore, AnsiTransitionAction.Ignore);

            // -> Ground
            Add(0x40..0x7E, AnsiParserState.CsiIgnore, AnsiParserState.Ground, AnsiTransitionAction.None);
        }

        // CsiIntermediate
        {
            Add(0x19, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIntermediate, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIntermediate,
                AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIntermediate,
                AnsiTransitionAction.Execute);
            Add(0x20..0x2F, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIntermediate,
                AnsiTransitionAction.Collect);
            Add(0x7F, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIntermediate, AnsiTransitionAction.Ignore);

            // -> CsiIgnore
            Add(0x30..0x3F, AnsiParserState.CsiIntermediate, AnsiParserState.CsiIgnore, AnsiTransitionAction.None);

            // -> Ground
            Add(0x40..0x7E, AnsiParserState.CsiIntermediate, AnsiParserState.Ground, AnsiTransitionAction.CsiDispatch);
        }

        // CsiEntry
        {
            Add(0x19, AnsiParserState.CsiEntry, AnsiParserState.CsiEntry, AnsiTransitionAction.Execute);
            Add(0x00..0x17, AnsiParserState.CsiEntry, AnsiParserState.CsiEntry, AnsiTransitionAction.Execute);
            Add(0x1C..0x1F, AnsiParserState.CsiEntry, AnsiParserState.CsiEntry, AnsiTransitionAction.Execute);
            Add(0x7F, AnsiParserState.CsiEntry, AnsiParserState.CsiEntry, AnsiTransitionAction.Ignore);

            // -> CsiParam
            Add(0x30..0x39, AnsiParserState.CsiEntry, AnsiParserState.CsiParam, AnsiTransitionAction.Param);
            Add(0x3B, AnsiParserState.CsiEntry, AnsiParserState.CsiParam, AnsiTransitionAction.Param);
            Add(0x3C..0x3F, AnsiParserState.CsiEntry, AnsiParserState.CsiParam, AnsiTransitionAction.Collect);

            // -> CsiIgnore
            Add(0x3A, AnsiParserState.CsiEntry, AnsiParserState.CsiIgnore, AnsiTransitionAction.None);

            // -> CsiIntermediate
            Add(0x20..0x2F, AnsiParserState.CsiEntry, AnsiParserState.CsiIntermediate, AnsiTransitionAction.Collect);

            // -> Ground
            Add(0x40..0x7E, AnsiParserState.CsiEntry, AnsiParserState.Ground, AnsiTransitionAction.CsiDispatch);
        }

        // OscString
        {
            Add(0x19, AnsiParserState.OscString, AnsiParserState.OscString, AnsiTransitionAction.Ignore);
            Add(0x00..0x17, AnsiParserState.OscString, AnsiParserState.OscString, AnsiTransitionAction.Ignore);
            Add(0x1C..0x1F, AnsiParserState.OscString, AnsiParserState.OscString, AnsiTransitionAction.Ignore);
            Add(0x20..0x7F, AnsiParserState.OscString, AnsiParserState.OscString, AnsiTransitionAction.OscPut);

            // -> Ground (BEL terminates OSC; xterm extension, not in the Williams diagram)
            Add(0x07, AnsiParserState.OscString, AnsiParserState.Ground, AnsiTransitionAction.None);
        }
    }

    public AnsiTransition GetTransition(
        AnsiParserState state, int code)
    {
        if (_transitions.TryGetValue(code, out var lookup) &&
            lookup.TryGetValue(state, out var transition))
        {
            return transition;
        }

        // The transition table only covers the C0, C1 and GL ranges (0x00-0x9F). Because the
        // input is already decoded to UTF-16, any codepoint at or above 0xA0 is a graphic
        // character (GR and beyond); classify it by how the current state treats printable input
        if (code >= 0xA0)
        {
            return new AnsiTransition(state, GetPrintableAction(state));
        }

        return new AnsiTransition(state, AnsiTransitionAction.None);
    }

    // How a graphic (printable) character is handled in each state. States that collect or
    // ignore control sequences never expect graphic input, so they ignore it in place
    private static AnsiTransitionAction GetPrintableAction(AnsiParserState state)
    {
        return state switch
        {
            AnsiParserState.Ground => AnsiTransitionAction.Print,
            AnsiParserState.OscString => AnsiTransitionAction.OscPut,
            AnsiParserState.DcsPassthrough => AnsiTransitionAction.DscPut,
            _ => AnsiTransitionAction.Ignore,
        };
    }

    private void Add(
        byte code, AnsiParserState fromState,
        AnsiParserState toState, AnsiTransitionAction action)
    {
        if (!_transitions.TryGetValue(code, out var lookup))
        {
            lookup = [];
            _transitions.Add(code, lookup);
        }

        lookup[fromState] = new AnsiTransition(toState, action);
    }

    private void Add(
        Range codes, AnsiParserState fromState,
        AnsiParserState toState, AnsiTransitionAction action)
    {
        for (var code = codes.Start.Value; code < codes.End.Value + 1; code++)
        {
            Add((byte)code, fromState, toState, action);
        }
    }
}