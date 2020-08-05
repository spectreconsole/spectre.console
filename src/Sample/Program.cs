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

            // We can also use System.ConsoleColor with AnsiConsole
            // to set the foreground and background color.
            foreach (ConsoleColor value in Enum.GetValues(typeof(ConsoleColor)))
            {
                var foreground = value;
                var background = (ConsoleColor)(15 - (int)value);

                AnsiConsole.Foreground = foreground;
                AnsiConsole.Background = background;
                AnsiConsole.WriteLine("{0} on {1}", foreground, background);
                AnsiConsole.ResetColors();
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

            // A normal, square table
            var table = new Table();
            table.AddColumns("[red underline]Foo[/]", "Bar");
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Lol[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddRow("Lorem ipsum dolor sit amet, consectetur adipiscing elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddRow("Hej 👋", "[green]Världen[/]");
            AnsiConsole.Render(table);

            // A rounded table
            table = new Table { Border = BorderKind.Rounded };
            table.AddColumns("[red underline]Foo[/]", "Bar");
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Lol[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddRow("Lorem ipsum dolor sit amet, consectetur [blue]adipiscing[/] elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddRow("Hej 👋", "[green]Världen[/]");
            AnsiConsole.Render(table);

            // A rounded table without headers
            table = new Table { Border = BorderKind.Rounded, ShowHeaders = false };
            table.AddColumns("[red underline]Foo[/]", "Bar");
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Lol[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddRow("Lorem ipsum dolor sit amet, consectetur [blue]adipiscing[/] elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddRow("Hej 👋", "[green]Världen[/]");
            AnsiConsole.Render(table);

            // Emulate the usage information for "dotnet run"
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("Usage: [grey]dotnet [blue]run[/] [[options] [[[[--] <additional arguments>...]][/]");
            AnsiConsole.WriteLine();
            var grid = new Grid();
            grid.AddColumn(new GridColumn { NoWrap = true });
            grid.AddColumn(new GridColumn { NoWrap = true, Width = 2 });
            grid.AddColumn();
            grid.AddRow("Options:", "", "");
            grid.AddRow("  [blue]-h[/], [blue]--help[/]", "", "Show command line help.");
            grid.AddRow("  [blue]-c[/], [blue]--configuration[/] <CONFIGURATION>", "", "The configuration to run for.\nThe default for most projects is [green]Debug[/].");
            grid.AddRow("  [blue]-v[/], [blue]--verbosity[/] <LEVEL>", "", "Set the MSBuild verbosity level. Allowed values are \nq[grey][[uiet][/], m[grey][[inimal][/], n[grey][[ormal][/], d[grey][[etailed][/], and diag[grey][[nostic][/].");
            AnsiConsole.Render(grid);

            // A simple table
            AnsiConsole.WriteLine();
            table = new Table { Border = BorderKind.Rounded };
            table.AddColumn("Foo");
            table.AddColumn("Bar");
            table.AddColumn("Baz");
            table.AddRow("Qux\nQuuuuuux", "[blue]Corgi[/]", "Waldo");
            table.AddRow("Grault", "Garply", "Fred");
            AnsiConsole.Render(table);

            // Render a table in some panels.
            AnsiConsole.Render(new Panel(new Panel(table, border: BorderKind.Ascii)));

            // Draw another table
            table = new Table { Expand = false };
            table.AddColumn(new TableColumn("Date"));
            table.AddColumn(new TableColumn("Title"));
            table.AddColumn(new TableColumn("Production\nBudget"));
            table.AddColumn(new TableColumn("Box Office"));
            table.AddRow("Dec 20, 2019", "Star Wars: The Rise of Skywalker", "$275,000,000", "[red]$375,126,118[/]");
            table.AddRow("May 25, 2018", "[yellow]Solo[/]: A Star Wars Story", "$275,000,000", "$393,151,347");
            table.AddRow("Dec 15, 2017", "Star Wars Ep. VIII: The Last Jedi", "$262,000,000", "[bold green]$1,332,539,889[/]");
            AnsiConsole.Render(table);
        }
    }
}