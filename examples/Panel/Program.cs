using Spectre.Console;

namespace PanelExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var content = new Markup(
                "[underline]I[/] heard [underline on blue]you[/] like panels\n\n\n\n" +
                "So I put a panel in a panel").Centered();

            AnsiConsole.Render(
                new Panel(
                    new Panel(content)
                    {
                        Border = BorderKind.Rounded
                    }));

            // Left adjusted panel with text
            AnsiConsole.Render(new Panel(
                new Text("Left adjusted\nLeft").LeftAligned())
            {
                Expand = true,
            });

            // Centered ASCII panel with text
            AnsiConsole.Render(new Panel(
                new Text("Centered\nCenter").Centered())
            {
                Expand = true,
                Border = BorderKind.Ascii,
            });

            // Right adjusted, rounded panel with text
            AnsiConsole.Render(new Panel(
                new Text("Right adjusted\nRight").RightAligned())
            {
                Expand = true,
                Border = BorderKind.Rounded,
            });
        }
    }
}
