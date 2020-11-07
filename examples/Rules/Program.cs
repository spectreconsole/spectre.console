using Spectre.Console;

namespace EmojiExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // No title
            WrapInPanel(
                new Rule()
                    .RuleStyle(Style.Parse("yellow"))
                    .AsciiBorder()
                    .LeftAligned());

            // Left aligned title
            WrapInPanel(
                new Rule("[blue]Left aligned[/]")
                    .RuleStyle(Style.Parse("red"))
                    .DoubleBorder()
                    .LeftAligned());

            // Centered title
            WrapInPanel(
                new Rule("[green]Centered[/]")
                    .RuleStyle(Style.Parse("green"))
                    .HeavyBorder()
                    .Centered());

            // Right aligned title
            WrapInPanel(
                new Rule("[red]Right aligned[/]")
                    .RuleStyle(Style.Parse("blue"))
                    .RightAligned());
        }

        private static void WrapInPanel(Rule rule)
        {
            AnsiConsole.Render(rule);
            AnsiConsole.WriteLine();
        }
    }
}
