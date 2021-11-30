// Check if we can use alternate screen buffers
using Spectre.Console;

if (!AnsiConsole.Profile.Capabilities.AlternateBuffer)
{
    AnsiConsole.MarkupLine(
        "[red]Alternate screen buffers are not supported " +
        "by your terminal[/] [yellow]:([/]");

    return;
}

// Write to the terminal
AnsiConsole.Write(new Rule("[yellow]Normal universe[/]"));
AnsiConsole.Write(new Panel("Hello World!"));
AnsiConsole.MarkupLine("[grey]Press a key to continue[/]");
AnsiConsole.Console.Input.ReadKey(true);

AnsiConsole.AlternateScreen(() =>
{
    // Now we're in another terminal screen buffer
    AnsiConsole.Write(new Rule("[red]Mirror universe[/]"));
    AnsiConsole.Write(new Panel("[red]Welcome to the upside down![/]"));
    AnsiConsole.MarkupLine("[grey]Press a key to return[/]");
    AnsiConsole.Console.Input.ReadKey(true);
});