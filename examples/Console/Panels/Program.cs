using Spectre.Console;

namespace Panels;

public static class Program
{
    public static void Main()
    {
        var content = new Markup(
            "[underline]I[/] heard [underline on blue]you[/] like panels\n\n\n\n" +
            "So I put a panel in a panel").Centered();

        AnsiConsole.Write(
            new Panel(
                new Panel(content)
                    .Border(BoxBorder.Rounded)));

        // Left adjusted panel with text
        AnsiConsole.Write(
            new Panel(new Text("Left adjusted\nLeft").LeftJustified())
                .Expand()
                .SquareBorder()
                .Header("[red]Left[/]"));

        // Centered ASCII panel with text
        AnsiConsole.Write(
            new Panel(new Text("Centered\nCenter").Centered())
                .Expand()
                .AsciiBorder()
                .Header("[green]Center[/]")
                .HeaderAlignment(Justify.Center));

        // Right adjusted, rounded panel with text
        AnsiConsole.Write(
            new Panel(new Text("Right adjusted\nRight").RightJustified())
                .Expand()
                .RoundedBorder()
                .Header("[blue]Right[/]")
                .HeaderAlignment(Justify.Right));
    }
}
