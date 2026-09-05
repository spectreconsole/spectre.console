namespace Spectre.Console;

internal delegate Task<string> TextPromptInputHandler(
    IAnsiConsole console, Style? style, bool secret,
    char? mask, IEnumerable<string>? items = null,
    CancellationToken cancellationToken = default,
    string? initialInput = null, PromptHistory? history = null);