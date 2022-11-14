using Spectre.Console;
using Spectre.Console.Rendering;

namespace Borders;

public static class Program
{
    public static void Main()
    {
        // Render panel borders
        HorizontalRule("PANEL BORDERS");
        PanelBorders();

        // Render table borders
        HorizontalRule("TABLE BORDERS");
        TableBorders();
    }

    private static void PanelBorders()
    {
        static IRenderable CreatePanel(string name, BoxBorder border)
        {
            return new Panel($"This is a panel with\nthe [yellow]{name}[/] border.")
                .Header($" [blue]{name}[/] ", Justify.Center)
                .Border(border)
                .BorderStyle(Style.Parse("grey"));
        }

        var items = new[]
        {
                CreatePanel("Ascii", BoxBorder.Ascii),
                CreatePanel("Square", BoxBorder.Square),
                CreatePanel("Rounded", BoxBorder.Rounded),
                CreatePanel("Heavy", BoxBorder.Heavy),
                CreatePanel("Double", BoxBorder.Double),
                CreatePanel("None", BoxBorder.None),
            };

        AnsiConsole.Write(
            new Padder(
                new Columns(items).PadRight(2),
                new Padding(2, 0, 0, 0)));
    }

    private static void TableBorders()
    {
        static IRenderable CreateTable(string name, TableBorder border)
        {
            var table = new Table().Border(border);
            table.AddColumn("[yellow]Header 1[/]", c => c.Footer("[grey]Footer 1[/]"));
            table.AddColumn("[yellow]Header 2[/]", col => col.Footer("[grey]Footer 2[/]").RightAligned());
            table.AddRow("Cell", "Cell");
            table.AddRow("Cell", "Cell");

            return new Panel(table)
                .Header($" [blue]{name}[/] ", Justify.Center)
                .NoBorder();
        }

        var items = new[]
        {
                CreateTable("Ascii", TableBorder.Ascii),
                CreateTable("Ascii2", TableBorder.Ascii2),
                CreateTable("AsciiDoubleHead", TableBorder.AsciiDoubleHead),
                CreateTable("Horizontal", TableBorder.Horizontal),
                CreateTable("Simple", TableBorder.Simple),
                CreateTable("SimpleHeavy", TableBorder.SimpleHeavy),
                CreateTable("Minimal", TableBorder.Minimal),
                CreateTable("MinimalHeavyHead", TableBorder.MinimalHeavyHead),
                CreateTable("MinimalDoubleHead", TableBorder.MinimalDoubleHead),
                CreateTable("Square", TableBorder.Square),
                CreateTable("Rounded", TableBorder.Rounded),
                CreateTable("Heavy", TableBorder.Heavy),
                CreateTable("HeavyEdge", TableBorder.HeavyEdge),
                CreateTable("HeavyHead", TableBorder.HeavyHead),
                CreateTable("Double", TableBorder.Double),
                CreateTable("DoubleEdge", TableBorder.DoubleEdge),
                CreateTable("Markdown", TableBorder.Markdown),
            };

        AnsiConsole.Write(new Columns(items).Collapse());
    }

    private static void HorizontalRule(string title)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Rule($"[white bold]{title}[/]").RuleStyle("grey").LeftJustified());
        AnsiConsole.WriteLine();
    }
}
