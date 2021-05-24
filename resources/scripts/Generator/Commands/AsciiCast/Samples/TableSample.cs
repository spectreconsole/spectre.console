using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class TableSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (100, 30);

        public override void Run(IAnsiConsole console)
        {
            var simple = new Table()
                .Border(TableBorder.Square)
                .BorderColor(Color.Red)
                .AddColumn(new TableColumn("[u]CDE[/]").Footer("EDC").Centered())
                .AddColumn(new TableColumn("[u]FED[/]").Footer("DEF"))
                .AddColumn(new TableColumn("[u]IHG[/]").Footer("GHI"))
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

            var table = new Table()
                .Centered()
                .Border(TableBorder.DoubleEdge)
                .Title("TABLE [yellow]TITLE[/]")
                .Caption("TABLE [yellow]CAPTION[/]")
                .AddColumn(new TableColumn(new Panel("[u]ABC[/]").BorderColor(Color.Red)).Footer("[u]FOOTER 1[/]"))
                .AddColumn(new TableColumn(new Panel("[u]DEF[/]").BorderColor(Color.Green)).Footer("[u]FOOTER 2[/]"))
                .AddColumn(new TableColumn(new Panel("[u]GHI[/]").BorderColor(Color.Blue)).Footer("[u]FOOTER 3[/]"))
                .AddRow(new Text("Hello").Centered(), new Markup("[red]World![/]"), Text.Empty)
                .AddRow(second, new Text("Whaaat"), new Text("Lol"))
                .AddRow(new Markup("[blue]Hej[/]").Centered(), new Markup("[yellow]Världen![/]"), Text.Empty);

            console.Write(table);
        }
    }
}