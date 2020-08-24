using Spectre.Console;

namespace PanelExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var content = Text.Markup(
                "[underline]I[/] heard [underline on blue]you[/] like 📦\n\n\n\n" +
                "So I put a 📦 in a 📦\n\n" +
                "😅");

            AnsiConsole.Render(
                new Panel(
                    new Panel(content)
                    {
                        Alignment = Justify.Center,
                        Border = BorderKind.Rounded
                    }));

            // Left adjusted panel with text
            AnsiConsole.Render(new Panel(
                new Text("Left adjusted\nLeft"))
            {
                Expand = true,
                Alignment = Justify.Left,
            });

            // Centered ASCII panel with text
            AnsiConsole.Render(new Panel(
                new Text("Centered\nCenter"))
            {
                Expand = true,
                Alignment = Justify.Center,
                Border = BorderKind.Ascii,
            });

            // Right adjusted, rounded panel with text
            AnsiConsole.Render(new Panel(
                new Text("Right adjusted\nRight"))
            {
                Expand = true,
                Alignment = Justify.Right,
                Border = BorderKind.Rounded,
            });
        }
    }
}
