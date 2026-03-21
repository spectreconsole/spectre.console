namespace Spectre.Console.Ansi;

/// <summary>
/// A ANSI/VT input parser based on the VT500-series.
/// </summary>
public sealed class AnsiParser
{
    private readonly Action<AnsiToken> _callback;
    private readonly List<char> _collect = [];
    private readonly List<int> _parameters = [0];
    private readonly List<bool> _parameterSeparators = [true];
    private readonly StringBuilder _parametersRaw = new();
    private readonly OscParser _oscParser;
    private bool _hasParameter;
    private AnsiParserState _currentState;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiMarkup"/> class.
    /// </summary>
    /// <param name="callback">The callback to be used for parsed tokens.</param>
    public AnsiParser(Action<AnsiToken> callback)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        _currentState = AnsiParserState.Ground;
        _oscParser = new OscParser();
    }

    /// <summary>
    /// Processes the specified code.
    /// </summary>
    /// <param name="code">The code to process.</param>
    public void Next(char code)
    {
        var (nextState, action) = AnsiTransitionTable.Shared.GetTransition(_currentState, code);

        // Perform entry event
        if (_currentState != nextState)
        {
            switch (_currentState)
            {
                case AnsiParserState.OscString:
                    var command = _oscParser.End(code);
                    if (command != null)
                    {
                        _callback(new AnsiToken.Osc(command));
                    }
                    break;
                case AnsiParserState.DcsPassthrough:
                    _callback(new AnsiToken.DcsUnhook());
                    break;
                case AnsiParserState.SosPmApcString:
                    _callback(new AnsiToken.ApcEnd());
                    break;
            }
        }

        // Perform the action
        switch (action)
        {
            case AnsiTransitionAction.None:
            case AnsiTransitionAction.Ignore:
                // Do nothing
                break;
            case AnsiTransitionAction.Print:
                _callback(new AnsiToken.Print(code));
                break;
            case AnsiTransitionAction.Execute:
                _callback(new AnsiToken.Execute(code));
                break;
            case AnsiTransitionAction.Collect:
                _collect.Add(code);
                break;
            case AnsiTransitionAction.Param:
                _parametersRaw.Append(code);

                if (code is ';' or ':')
                {
                    _parameters.Add(0);
                    _parameterSeparators.Add(code is ';');
                }
                else
                {
                    Debug.Assert(char.IsDigit(code), "Expected digit");

                    var accumulator = (_parameters[^1] * 10) + code - 48;
                    _parameters[^1] = accumulator > (int.MaxValue / 10) - 10 ? 0 : accumulator;
                    _hasParameter = true;
                }

                break;
            case AnsiTransitionAction.EscDispatch:
                _callback(new AnsiToken.Esc(
                    Collect: [.. _collect],
                    Final: code));
                break;
            case AnsiTransitionAction.CsiDispatch:
                _callback(new AnsiToken.Csi(
                    Collect: [.. _collect],
                    Params: _hasParameter ? [.. _parameters] : [],
                    Final: code,
                    ParamsRaw: _parametersRaw.ToString()));
                break;
            case AnsiTransitionAction.OscPut:
                _oscParser.Next(code);
                break;
            case AnsiTransitionAction.DscPut:
                _callback(new AnsiToken.DcsPut(code));
                break;
            case AnsiTransitionAction.ApcPut:
                _callback(new AnsiToken.ApcPut(code));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Perform exit event
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
                    _callback(new AnsiToken.Csi(
                        Collect: [.. _collect],
                        Params: _hasParameter ? [.. _parameters] : [],
                        Final: code,
                        ParamsRaw: _parametersRaw.ToString()));
                    break;
                case AnsiParserState.SosPmApcString:
                    _callback(new AnsiToken.ApcStart());
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
        _collect.Clear();
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
    ApcPut,
}

internal readonly record struct AnsiTransition(
    AnsiParserState State,
    AnsiTransitionAction Action)
{
    public AnsiParserState State { get; } = State;
    public AnsiTransitionAction Action { get; } = Action;
}

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
            Add(0x19, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.ApcPut);
            Add(0x00..0x17, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.ApcPut);
            Add(0x1C..0x1F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.ApcPut);
            Add(0x20..0x7F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString,
                AnsiTransitionAction.ApcPut);
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

        return new AnsiTransition(state, AnsiTransitionAction.None);
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