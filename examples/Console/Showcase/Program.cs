using Spectre.Console;
using Spectre.Console.Examples;
using Spectre.Console.Rendering;

namespace Showcase;

public static partial class Program
{
    public static void Main()
    {
        var table = new Table().HideHeaders().NoBorder();
        table.Title("[u][yellow]Spectre.Console[/] [b]Features[/][/]");
        table.AddColumn("Feature", c => c.NoWrap().RightAligned().Width(10).PadRight(3));
        table.AddColumn("Demonstration", c => c.PadRight(0));
        table.AddEmptyRow();

        // Colors
        table.AddRow(
            new Markup("[red]Colors[/]"),
            GetColorTable());

        // Styles
        table.AddEmptyRow();
        table.AddRow(
            new Markup("[red]OS[/]"),
            new Grid().Expand().AddColumns(3)
            .AddRow(
                "[bold green]Windows[/]",
                "[bold blue]macOS[/]",
                "[bold yellow]Linux[/]"));

        // Styles
        table.AddEmptyRow();
        table.AddRow(
            "[red]Styles[/]",
            "All ansi styles: [bold]bold[/], [dim]dim[/], [italic]italic[/], [underline]underline[/], "
                + "[strikethrough]strikethrough[/], [reverse]reverse[/], and even [blink]blink[/].");

        // Text
        table.AddEmptyRow();
        table.AddRow(
            new Markup("[red]Text[/]"),
            new Markup("Word wrap text. Justify [green]left[/], [yellow]center[/] or [blue]right[/]."));

        table.AddEmptyRow();
        table.AddRow(
            Text.Empty,
            GetTextGrid());

        // Markup
        table.AddEmptyRow();
        table.AddRow(
            "[red]Markup[/]",
            "[bold purple]Spectre.Console[/] supports a simple [i]bbcode[/] like "
                + "[b]markup[/] for [yellow]color[/], [underline]style[/], and emoji! "
                + ":thumbs_up: :red_apple: :ant: :bear: :baguette_bread: :bus:");

        // Trees and tables
        table.AddEmptyRow();
        table.AddRow(
            new Markup("[red]Tables and Trees[/]"),
            GetTreeTable());

        // Charts
        table.AddRow(
            new Markup("[red]Charts[/]"),
            new Grid().Collapse().AddColumns(2).AddRow(
                new Panel(GetBreakdownChart()).BorderColor(Color.Grey),
                new Panel(GetBarChart()).BorderColor(Color.Grey)));


        // Exceptions
        table.AddEmptyRow();
        table.AddRow(
            new Markup("[red]Exceptions[/]"),
            ExceptionGenerator.GenerateException().GetRenderable());

        // Much more
        table.AddEmptyRow();
        table.AddRow(
            "[red]+ Much more![/]",
            "Tables, Grids, Trees, Progress bars, Status, Bar charts, Calendars, Figlet, Images, Text prompts, "
                + "List boxes, Separators, Pretty exceptions, Canvas, CLI parsing");
        table.AddEmptyRow();

        // Render the table
        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
    }

    private static IRenderable GetColorTable()
    {
        var colorTable = new Table().Collapse().HideHeaders().NoBorder();
        colorTable.AddColumn("Desc", c => c.PadRight(3)).AddColumn("Colors", c => c.PadRight(0));
        colorTable.AddRow(
            new Markup(
                "✓ [bold grey]NO_COLOR support[/]\n" +
                "✓ [bold green]3-bit color[/]\n" +
                "✓ [bold blue]4-bit color[/]\n" +
                "✓ [bold purple]8-bit color[/]\n" +
                "✓ [bold yellow]Truecolor (16.7 million)[/]\n" +
                "✓ [bold aqua]Automatic color conversion[/]"),
            new ColorBox(height: 6));

        return colorTable;
    }

    private static IRenderable GetTextGrid()
    {
        var loremTable = new Grid();
        var lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque in metus sed sapien ultricies pretium a at justo. Maecenas luctus velit et auctor maximus.";
        loremTable.AddColumn(new GridColumn().LeftAligned());
        loremTable.AddColumn(new GridColumn().Centered());
        loremTable.AddColumn(new GridColumn().RightAligned());
        loremTable.AddRow($"[green]{lorem}[/]", $"[yellow]{lorem}[/]", $"[blue]{lorem}[/]");
        return loremTable;
    }

    private static IRenderable GetTreeTable()
    {
        var tree = new Tree("📁 src");
        tree.AddNode("📁 foo").AddNode("📄 bar.cs");
        tree.AddNode("📁 baz").AddNode("📁 qux").AddNode("📄 corgi.txt");
        tree.AddNode("📄 waldo.xml");

        var table = new Table().SimpleBorder().BorderColor(Color.Grey);
        table.AddColumn(new TableColumn("Overview"));
        table.AddColumn(new TableColumn("").Footer("[grey]3 Files, 225 KiB[/]"));
        table.AddRow(new Markup("[yellow]Files[/]"), tree);

        return new Table().RoundedBorder().Collapse().BorderColor(Color.Yellow)
            .AddColumn("Foo").AddColumn("Bar")
            .AddRow(new Text("Baz"), table)
            .AddRow("Qux", "Corgi");
    }

    private static IRenderable GetBarChart()
    {
        return new BarChart()
            .AddItem("Apple", 32, Color.Green)
            .AddItem("Oranges", 13, Color.Orange1)
            .AddItem("Bananas", 22, Color.Yellow);
    }

    private static IRenderable GetBreakdownChart()
    {
        return new BreakdownChart()
            .ShowPercentage()
            .FullSize()
            .AddItem("C#", 82, Color.Green)
            .AddItem("PowerShell", 13, Color.Red)
            .AddItem("Bash", 5, Color.Blue);
    }
}
