using Spectre.Console;

namespace Links;

public static class Program
{
    public static void Main()
    {
        if (AnsiConsole.Profile.Capabilities.Links)
        {
            AnsiConsole.MarkupLine("[link=https://patriksvensson.se]Click to visit my blog[/]!");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]It looks like your terminal doesn't support links[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow](╯°□°)╯[/]︵ [blue]┻━┻[/]");
        }
    }
}
