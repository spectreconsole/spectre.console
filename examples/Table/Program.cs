using System;
using Spectre.Console;

namespace TableExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // A simple table§
            RenderSimpleTable();

            // A big table
            RenderBigTable();
        }

        private static void RenderSimpleTable()
        {
            // Create the table.
            var table = new Table();
            table.AddColumn(new TableColumn("[u]Foo[/]"));
            table.AddColumn(new TableColumn("[u]Bar[/]"));
            table.AddColumn(new TableColumn("[u]Baz[/]"));

            // Add some rows
            table.AddRow("Hello", "[red]World![/]", "");
            table.AddRow("[blue]Bounjour[/]", "[white]le[/]", "[red]monde![/]");
            table.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            AnsiConsole.Render(table);
        }

        private static void RenderBigTable()
        {
            // Create the table.
            var table = new Table { Border = BorderKind.Rounded };
            table.AddColumn("[red underline]Foo[/]");
            table.AddColumn(new TableColumn("[blue]Bar[/]") { Alignment = Justify.Right, NoWrap = true });

            // Add some rows
            table.AddRow("[blue][underline]Hell[/]o[/]", "World 🌍");
            table.AddRow("[yellow]Patrik [green]\"Hello World[/]\" Svensson[/]", "Was [underline]here[/]!");
            table.AddEmptyRow();
            table.AddRow(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
                "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat " +
                "non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "◀ Strange language");
            table.AddEmptyRow();
            table.AddRow("Hej 👋", "[green]Världen[/]");

            AnsiConsole.Render(table);
        }
    }
}
