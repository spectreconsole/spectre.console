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
                    .LeftAligned());

            // Left aligned title
            WrapInPanel(
                new Rule("[white]Left aligned[/]")
                    .RuleStyle(Style.Parse("red"))
                    .LeftAligned());

            // Centered title
            WrapInPanel(
                new Rule("[silver]Centered[/]")
                    .RuleStyle(Style.Parse("green"))
                    .Centered());

            // Right aligned title
            WrapInPanel(
                new Rule("[grey]Right aligned[/]")
                    .RuleStyle(Style.Parse("blue"))
                    .RightAligned());
        }

        private static void WrapInPanel(Rule rule)
        {
            AnsiConsole.Render(new Panel(rule).Expand().BorderStyle(Style.Parse("grey")));
            AnsiConsole.WriteLine();
        }
    }
}
