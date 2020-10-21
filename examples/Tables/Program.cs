using Spectre.Console;

namespace TableExample
{
    public static class Program
    {
        public static void Main()
        {
            // Create the table.
            var table = CreateTable();

            // Render the table.
            AnsiConsole.Render(table);
        }

        private static Table CreateTable()
        {
            var simple = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Red)
                .AddColumn(new TableColumn("[u]CDE[/]").Centered())
                .AddColumn(new TableColumn("[u]FED[/]"))
                .AddColumn(new TableColumn("[u]IHG[/]"))
                .AddRow("Hello", "[red]World![/]", "")
                .AddRow("[blue]Bonjour[/]", "[white]le[/]", "[red]monde![/]")
                .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            var second = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green)
                .AddColumn(new TableColumn("[u]Foo[/]"))
                .AddColumn(new TableColumn("[u]Bar[/]"))
                .AddColumn(new TableColumn("[u]Baz[/]"))
                .AddRow("Hello", "[red]World![/]", "")
                .AddRow(simple, new Text("Whaaat"), new Text("Lolz"))
                .AddRow("[blue]Hej[/]", "[yellow]Världen![/]", "");

            return new Table()
                .Centered()
                .Border(TableBorder.DoubleEdge)
                .Heading("TABLE [yellow]HEADING[/]")
                .Footnote("TABLE [yellow]FOOTNOTE[/]")
                .AddColumn(new TableColumn(new Panel("[u]ABC[/]").BorderColor(Color.Red)))
                .AddColumn(new TableColumn(new Panel("[u]DEF[/]").BorderColor(Color.Green)))
                .AddColumn(new TableColumn(new Panel("[u]GHI[/]").BorderColor(Color.Blue)))
                .AddRow(new Text("Hello").Centered(), new Markup("[red]World![/]"), Text.Empty)
                .AddRow(second, new Text("Whaaat"), new Text("Lol"))
                .AddRow(new Markup("[blue]Hej[/]").Centered(), new Markup("[yellow]Världen![/]"), Text.Empty);
        }
    }
}
