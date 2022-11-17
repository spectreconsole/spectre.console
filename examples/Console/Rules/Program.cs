using Spectre.Console;

namespace Rules;

public static class Program
{
    public static void Main(string[] args)
    {
        // No title
        Render(
            new Rule()
                .RuleStyle(Style.Parse("yellow"))
                .AsciiBorder()
                .LeftJustified());

        // Left aligned title
        Render(
            new Rule("[blue]Left aligned[/]")
                .RuleStyle(Style.Parse("red"))
                .DoubleBorder()
                .LeftJustified());

        // Centered title
        Render(
            new Rule("[green]Centered[/]")
                .RuleStyle(Style.Parse("green"))
                .HeavyBorder()
                .Centered());

        // Right aligned title
        Render(
            new Rule("[red]Right aligned[/]")
                .RuleStyle(Style.Parse("blue"))
                .RightJustified());
    }

    private static void Render(Rule rule)
    {
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
    }
}
