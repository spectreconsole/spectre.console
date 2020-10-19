using Spectre.Console;

namespace EmojiExample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // No title
            Render(new Rule().SetStyle("yellow"));

            // Left aligned title
            Render(new Rule("[white]Left aligned[/]").LeftAligned().SetStyle("red"));

            // Centered title
            Render(new Rule("[silver]Centered[/]").Centered().SetStyle("green"));

            // Right aligned title
            Render(new Rule("[grey]Right aligned[/]").RightAligned().SetStyle("blue"));
        }

        private static void Render(Rule rule)
        {
            AnsiConsole.Render(new Panel(rule).Expand().SetBorderStyle(Style.Parse("grey")));
            AnsiConsole.WriteLine();
        }
    }
}
