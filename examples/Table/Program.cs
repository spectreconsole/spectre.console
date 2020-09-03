using Spectre.Console;
using Spectre.Console.Rendering;

namespace TableExample
{
    public static class Program
    {
        public static void Main()
        {
            // A simple table
            RenderSimpleTable();

            // A big table
            RenderBigTable();

            // A nested table
            RenderNestedTable();
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
            table.AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]");
            table.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            AnsiConsole.Render(table);
        }

        private static void RenderBigTable()
        {
            // Create the table.
            var table = new Table().SetBorderKind(BorderKind.Rounded);
            table.AddColumn("[red underline]Foo[/]");
            table.AddColumn(new TableColumn("[blue]Bar[/]") { Alignment = Justify.Right, NoWrap = true });

            // Add some rows
            table.AddRow("[blue][underline]Hell[/]o[/]", "World");
            table.AddRow("[yellow]Patrik [green]\"Hello World\"[/] Svensson[/]", "Was [underline]here[/]!");
            table.AddEmptyRow();
            table.AddRow(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit,sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
                "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat " +
                "non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", "<- Strange language");
            table.AddEmptyRow();
            table.AddRow("Hej", "[green]Världen[/]");

            AnsiConsole.Render(table);
        }

        private static void RenderNestedTable()
        {
            // Create simple table.
            var simple = new Table().SetBorderKind(BorderKind.Rounded).SetBorderColor(Color.Red);
            simple.AddColumn(new TableColumn("[u]Foo[/]").Centered());
            simple.AddColumn(new TableColumn("[u]Bar[/]"));
            simple.AddColumn(new TableColumn("[u]Baz[/]"));
            simple.AddRow("Hello", "[red]World![/]", "");
            simple.AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]");
            simple.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            // Create other table.
            var second = new Table().SetBorderKind(BorderKind.Square).SetBorderColor(Color.Green);
            second.AddColumn(new TableColumn("[u]Foo[/]"));
            second.AddColumn(new TableColumn("[u]Bar[/]"));
            second.AddColumn(new TableColumn("[u]Baz[/]"));
            second.AddRow("Hello", "[red]World![/]", "");
            second.AddRow(simple, new Text("Whaaat"), new Text("Lolz"));
            second.AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            var table = new Table().SetBorderKind(BorderKind.Rounded);
            table.AddColumn(new TableColumn(new Panel("[u]Foo[/]").SetBorderColor(Color.Red)));
            table.AddColumn(new TableColumn(new Panel("[u]Bar[/]").SetBorderColor(Color.Green)));
            table.AddColumn(new TableColumn(new Panel("[u]Baz[/]").SetBorderColor(Color.Blue)));

            // Add some rows
            table.AddRow(new Text("Hello").Centered(), new Markup("[red]World![/]"), Text.Empty);
            table.AddRow(second, new Text("Whaaat"), new Text("Lol"));
            table.AddRow(new Markup("[blue]Hej[/]").Centered(), new Markup("[yellow]Världen![/]"), Text.Empty);

            AnsiConsole.Render(table);
        }
    }
}
