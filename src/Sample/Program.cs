using System;
using Spectre.Console;

namespace Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Use the static API to write some things to the console.
            AnsiConsole.Foreground = Color.Chartreuse2;
            AnsiConsole.Decoration = Decoration.Underline | Decoration.Bold;
            AnsiConsole.WriteLine("Hello World!");
            AnsiConsole.Reset();
            AnsiConsole.MarkupLine("Capabilities: [yellow underline]{0}[/]", AnsiConsole.Capabilities);
            AnsiConsole.MarkupLine("Width=[yellow]{0}[/], Height=[yellow]{1}[/]", AnsiConsole.Width, AnsiConsole.Height);
            AnsiConsole.MarkupLine("[white on red]Good[/] [red]bye[/]!");
            AnsiConsole.WriteLine();

            // We can also use System.ConsoleColor with AnsiConsole.
            foreach (ConsoleColor value in Enum.GetValues(typeof(ConsoleColor)))
            {
                AnsiConsole.Foreground = value;
                AnsiConsole.WriteLine("ConsoleColor.{0}", value);
            }

            // We can get the default console via the static API.
            var console = AnsiConsole.Console;

            // Or you can build it yourself the old fashion way.
            console = AnsiConsole.Create(
                new AnsiConsoleSettings()
                {
                    Ansi = AnsiSupport.Yes,
                    ColorSystem = ColorSystemSupport.Standard,
                    Out = Console.Out,
                });

            // In this case, we will find the closest colors
            // and downgrade them to the specified color system.
            console.WriteLine();
            console.Foreground = Color.Chartreuse2;
            console.Decoration = Decoration.Underline | Decoration.Bold;
            console.WriteLine("Hello World!");
            console.ResetColors();
            console.ResetDecoration();
            console.MarkupLine("Capabilities: [yellow underline]{0}[/]", console.Capabilities);
            console.MarkupLine("Width=[yellow]{0}[/], Height=[yellow]{1}[/]", console.Width, console.Height);
            console.MarkupLine("[white on red]Good[/] [red]bye[/]!");
            console.WriteLine();

            // Nest some panels and text
            AnsiConsole.Foreground = Color.Maroon;
            AnsiConsole.Render(
                new Panel(
                    new Panel(
                        new Panel(
                            new Panel(
                                Text.New(
                                    "[underline]I[/] heard [underline on blue]you[/] like 📦\n\n\n\n" +
                                    "So I put a 📦 in a 📦\nin a 📦 in a 📦\n\n" +
                                    "😅", foreground: Color.White),
                                content: Justify.Center,
                                border: BorderKind.Rounded))),
                    border: BorderKind.Ascii));

            // Reset colors
            AnsiConsole.ResetColors();

            // Left adjusted panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Left adjusted\nLeft",
                    foreground: Color.White),
                fit: true));

            // Centered panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Centered\nCenter",
                    foreground: Color.White),
                fit: true, content: Justify.Center));

            // Right adjusted panel with text
            AnsiConsole.Render(new Panel(
                Text.New("Right adjusted\nRight",
                    foreground: Color.White),
                fit: true, content: Justify.Right));

            var table = new Table();
            table.AddColumns("[red underline]Foo[/]", "Bar");
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Lol[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddRow("Lorem ipsum dolor sit amet, consectetur adipiscing elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddRow("Hej 👋", "[green]Världen[/]");
            AnsiConsole.Render(table);

            table = new Table(BorderKind.Rounded);
            table.AddColumns("[red underline]Foo[/]", "Bar");
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Lol[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddRow("Lorem ipsum dolor sit amet, consectetur [blue]adipiscing[/] elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddRow("Hej 👋", "[green]Världen[/]");
            AnsiConsole.Render(table);

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("  Usage: [grey]dotnet [blue]run[/] [[options] [[[[--] <additional arguments>...]][/]");
            AnsiConsole.WriteLine();
            var grid = new Grid();
            grid.AddColumns(3);
            grid.AddRow("  Options", "", "");
            grid.AddRow("    [blue]-h[/], [blue]--help[/]", "   ", "Show command line help.");
            grid.AddRow("    [blue]-c[/], [blue]--configuration[/] <CONFIGURATION>", "   ", "The configuration to run for.\nThe default for most projects is [green]Debug[/].");
            grid.AddRow("    [blue]-v[/], [blue]--verbosity[/] <LEVEL>", "   ", "Set the MSBuild verbosity level.\nAllowed values are q[grey][[uiet][/], m[grey][[inimal][/], n[grey][[ormal][/], d[grey][[etailed][/], and diag[grey][[nostic][/].");
            AnsiConsole.Render(grid);
        }
    }
}