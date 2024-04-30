using Spectre.Console;

namespace Colors;

public static class Program
{
    public static void Main()
    {
        AnsiConsole.ResetDecoration();
        AnsiConsole.WriteLine();

        if (AnsiConsole.Profile.Capabilities.Ansi)
        {
            AnsiConsole.Write(new Rule("[bold green]ANSI Decorations[/]"));
        }
        else
        {
            AnsiConsole.Write(new Rule("[bold red]Legacy Decorations (unsupported)[/]"));
        }

        var decorations = System.Enum.GetValues(typeof(Decoration));
        foreach (var decoration in decorations)
        {
            var name = System.Enum.GetName(typeof(Decoration),decoration);
            AnsiConsole.Write(name + ": ");
            AnsiConsole.Write(new Markup(name+"\n", new Style(decoration: (Decoration)decoration)));
        }
    }
}
