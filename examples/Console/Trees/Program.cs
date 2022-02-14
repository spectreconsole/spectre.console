using Spectre.Console;

namespace Trees;

public static class Program
{
    public static void Main()
    {
        AnsiConsole.WriteLine();

        // Render the tree
        var tree = BuildTree();
        AnsiConsole.Write(tree);
    }

    private static Tree BuildTree()
    {
        // Create the tree
        var tree = new Tree("Root")
            .Style(Style.Parse("red"))
            .Guide(TreeGuide.Line);

        // Add some nodes
        var foo = tree.AddNode("[yellow]Foo[/]");
        var table = foo.AddNode(new Table()
            .RoundedBorder()
            .AddColumn("First")
            .AddColumn("Second")
            .AddRow("1", "2")
            .AddRow("3", "4")
            .AddRow("5", "6"));

        table.AddNode("[blue]Baz[/]");
        foo.AddNode("Qux");

        var bar = tree.AddNode("[yellow]Bar[/]");
        bar.AddNode(new Calendar(2020, 12)
            .AddCalendarEvent(2020, 12, 12)
            .HideHeader());

        // Return the tree
        return tree;
    }
}
