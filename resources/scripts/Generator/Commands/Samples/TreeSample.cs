using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class TreeSample : BaseSample
    {
        public override void Run(IAnsiConsole console)
        {
            // Create the tree
            var tree = new Tree("Root")
                .Style(Style.Parse("red"))
                .Guide(TreeGuide.Line);

            // Add some nodes
            var foo = tree.AddNode("[yellow]Nest objects like tables[/]");
            var table = foo.AddNode(new Table()
                .RoundedBorder()
                .AddColumn("First")
                .AddColumn("Second")
                .AddRow("1", "2")
                .AddRow("3", "4")
                .AddRow("5", "6"));

            table.AddNode("[blue]with[/]");
            table.AddNode("[blue]multiple[/]");
            table.AddNode("[blue]children too[/]");

            var bar = tree.AddNode("Any IRenderable can be nested, such as [yellow]calendars[/]");
            bar.AddNode(new Calendar(2020, 12)
                .Border(TableBorder.Rounded)
                .BorderStyle(new Style(Color.Green3_1))
                .AddCalendarEvent(2020, 12, 12)
                .HideHeader());

            console.Write(tree);

        }
    }
}