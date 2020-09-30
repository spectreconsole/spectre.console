using Spectre.Console;

namespace PanelExample
{
    public static class Program
    {
        public static void Main()
        {
            var content = new Markup(
                "[underline]I[/] heard [underline on blue]you[/] like panels\n\n\n\n" +
                "So I put a panel in a panel").Centered();

            AnsiConsole.Render(
                new Panel(
                    new Panel(content)
                        .SetBorder(BoxBorder.Rounded)));

            // Left adjusted panel with text
            AnsiConsole.Render(
                new Panel(new Text("Left adjusted\nLeft").LeftAligned())
                    .Expand()
                    .SquareBorder()
                    .SetHeader("Left", Style.WithForeground(Color.Red)));

            // Centered ASCII panel with text
            AnsiConsole.Render(
                new Panel(new Text("Centered\nCenter").Centered())
                    .Expand()
                    .AsciiBorder()
                    .SetHeader("Center", Style.WithForeground(Color.Green), Justify.Center));

            // Right adjusted, rounded panel with text
            AnsiConsole.Render(
                new Panel(new Text("Right adjusted\nRight").RightAligned())
                    .Expand()
                    .RoundedBorder()
                    .SetHeader("Right", Style.WithForeground(Color.Blue), Justify.Right));
        }
    }
}
