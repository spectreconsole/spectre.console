using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Spectre.Console.Ansi;

public sealed class AnsiParser
{
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
    Put,
    OscPut,
    ApcPut,
}

internal record struct AnsiTransition(AnsiParserState state, AnsiTransitionAction action)
{
    public AnsiParserState State { get; } = state;
    public AnsiTransitionAction Action { get; } = action;
}

internal sealed class AnsiTransitionTable
{
    private readonly Dictionary<int, Dictionary<AnsiParserState, AnsiTransition>> _transitions;

    private AnsiTransitionTable()
    {
        _transitions = [];

        // Anywhere
        foreach (var state in EnumUtils.GetValues<AnsiParserState>())
        {
            // Anywhere -> Ground
            Single(0x18, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Single(0x1A, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Range(0x80..0x8f, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Range(0x91..0x97, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Single(0x99, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Single(0x9A, state, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Single(0x9C, state, AnsiParserState.Ground, AnsiTransitionAction.None);

            // Anywhere -> SosPmApcString
            Single(0x98, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Single(0x9E, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);
            Single(0x9F, state, AnsiParserState.SosPmApcString, AnsiTransitionAction.None);

            // Anywhere -> Escape
            Single(0x1B, state, AnsiParserState.Escape, AnsiTransitionAction.None);

            // Anywhere -> DcsEntry
            Single(0x90, state, AnsiParserState.DcsEntry, AnsiTransitionAction.None);

            // Anywhere -> OscString
            Single(0x9D, state, AnsiParserState.OscString, AnsiTransitionAction.None);

            // Anywhere -> CsiEntry
            Single(0x9B, state, AnsiParserState.CsiEntry, AnsiTransitionAction.None);
        }

        // Ground
        {
            // Events
            Single(0x19, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Range(0x00..0x17, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Range(0x1C..0x1F, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Execute);
            Range(0x20..0x7F, AnsiParserState.Ground, AnsiParserState.Ground, AnsiTransitionAction.Print);
        }

        // Escape Intermediate
        {
            // Events
            Single(0x19, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Execute);
            Range(0x00..0x17, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Execute);
            Range(0x1C..0x1F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Execute);
            Range(0x20..0x2F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Collect);
            Single(0x7F, AnsiParserState.EscapeIntermediate, AnsiParserState.EscapeIntermediate, AnsiTransitionAction.Ignore);
        }

        // SosPmApcString
        {
            // Events
            Single(0x19, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.Ignore);
            Range(0x00..0x17, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.Ignore);
            Range(0x1C..0x1F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.Ignore);
            Range(0x20..0x7F, AnsiParserState.SosPmApcString, AnsiParserState.SosPmApcString, AnsiTransitionAction.Ignore);
        }

        // Escape
        {
            // Events
            Single(0x19, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Range(0x00..0x17, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Range(0x1C..0x1F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Single(0x7F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
        }

        // DcsEntry
        {
            // Events
            Single(0x19, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Range(0x00..0x17, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Range(0x1C..0x1F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
            Single(0x7F, AnsiParserState.DcsEntry, AnsiParserState.DcsEntry, AnsiTransitionAction.Ignore);
        }
    }

    private void Single(
        byte code, AnsiParserState fromState,
        AnsiParserState toState, AnsiTransitionAction action)
    {
        if (!_transitions.TryGetValue(code, out var value))
        {
            value = [];
            _transitions.Add(code, value);
        }

        if (!value.TryAdd(fromState, new AnsiTransition(toState, action)))
        {
            throw new InvalidOperationException("Duplicate transition detected");
        }
    }

    private void Range(
        Range codes, AnsiParserState fromState,
        AnsiParserState toState, AnsiTransitionAction action)
    {
        for (var code = codes.Start.Value; code < codes.End.Value; code++)
        {
            Single((byte)code, fromState, toState, action);
        }
    }
}
