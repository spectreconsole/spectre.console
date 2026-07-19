namespace Spectre.Console.Ansi;

internal readonly record struct AnsiTransition(
    AnsiParserState State,
    AnsiTransitionAction Action);