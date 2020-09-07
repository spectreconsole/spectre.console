using System;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Borders
{
    public static class Program
    {
        public static void Main()
        {
            var items = new[]
            {
                Create("Ascii", Border.Ascii),
                Create("Ascii2", Border.Ascii2),
                Create("AsciiDoubleHead", Border.AsciiDoubleHead),
                Create("Horizontal", Border.Horizontal),
                Create("Simple", Border.Simple),
                Create("SimpleHeavy", Border.SimpleHeavy),
                Create("Minimal", Border.Minimal),
                Create("MinimalHeavyHead", Border.MinimalHeavyHead),
                Create("MinimalDoubleHead", Border.MinimalDoubleHead),
                Create("Square", Border.Square),
                Create("Rounded", Border.Rounded),
                Create("Heavy", Border.Heavy),
                Create("HeavyEdge", Border.HeavyEdge),
                Create("HeavyHead", Border.HeavyHead),
                Create("Double", Border.Double),
                Create("DoubleEdge", Border.DoubleEdge),
            };

            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Columns(items).Collapse());
        }

        private static IRenderable Create(string name, Border border)
        {
            var table = new Table().SetBorder(border);
            table.AddColumns("[yellow]Header 1[/]", "[yellow]Header 2[/]");
            table.AddRow("Cell", "Cell");
            table.AddRow("Cell", "Cell");

            return new Panel(table)
                .SetHeader($" {name} ", Style.Parse("blue"), Justify.Center)
                .SetBorderStyle(Style.Parse("grey"))
                .NoBorder();
        }
    }
}
