using Spectre.Console;

namespace TableExample
{
    public static class Program
    {
        public static void Main()
        {
            var tree = new Tree();

            tree.AddNode(new FigletText("Dec 2020"));
            tree.AddNode(new Markup("[link]Click to go to summary[/]"));

            // Add the calendar nodes
            tree.AddNode(new Markup("[blue]Calendar[/]"),
                node => node.AddNode(
                    new Calendar(2020, 12)
                        .AddCalendarEvent(2020, 12, 12)
                        .HideHeader()));

            // Add video games node
            tree.AddNode(new Markup("[red]Played video games[/]"),
                node => node.AddNode(
                    new Table()
                        .RoundedBorder()
                        .AddColumn("Title")
                        .AddColumn("Console")
                        .AddRow("The Witcher 3", "XBox One X")
                        .AddRow("Cyberpunk 2077", "PC")
                        .AddRow("Animal Crossing", "Nintendo Switch")));


            // Add the fruit nodes
            tree.AddNode(new Markup("[green]Fruits[/]"), fruits =>
                fruits.AddNode(new Markup("Eaten"),
                    node => node.AddNode(
                        new BarChart().Width(40)
                            .AddItem("Apple", 12, Color.Red)
                            .AddItem("Kiwi", 3, Color.Green)
                            .AddItem("Banana", 21, Color.Yellow))));

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Monthly summary[/]");
            AnsiConsole.Render(tree);
        }
    }
}
