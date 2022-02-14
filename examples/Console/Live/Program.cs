using System;
using System.Threading;
using Spectre.Console;

namespace Live;

public static class Program
{
    public static void Main()
    {
        var table = new Table().Centered();

        // Animate
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(VerticalOverflow.Ellipsis)
            .Cropping(VerticalOverflowCropping.Top)
            .Start(ctx =>
            {
                void Update(int delay, Action action)
                {
                    action();
                    ctx.Refresh();
                    Thread.Sleep(delay);
                }

                    // Columns
                    Update(230, () => table.AddColumn("Release date"));
                Update(230, () => table.AddColumn("Title"));
                Update(230, () => table.AddColumn("Budget"));
                Update(230, () => table.AddColumn("Opening Weekend"));
                Update(230, () => table.AddColumn("Box office"));

                    // Rows
                    Update(70, () => table.AddRow("May 25, 1977", "[yellow]Star Wars[/] [grey]Ep.[/] [u]IV[/]", "$11,000,000", "$1,554,475", "$775,398,007"));
                Update(70, () => table.AddRow("May 21, 1980", "[yellow]Star Wars[/] [grey]Ep.[/] [u]V[/]", "$18,000,000", "$4,910,483", "$547,969,004"));
                Update(70, () => table.AddRow("May 25, 1983", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VI[/]", "$32,500,000", "$23,019,618", "$475,106,177"));
                Update(70, () => table.AddRow("May 19, 1999", "[yellow]Star Wars[/] [grey]Ep.[/] [u]I[/]", "$115,000,000", "$64,810,870", "$1,027,044,677"));
                Update(70, () => table.AddRow("May 16, 2002", "[yellow]Star Wars[/] [grey]Ep.[/] [u]II[/]", "$115,000,000", "$80,027,814", "$649,436,358"));
                Update(70, () => table.AddRow("May 19, 2005", "[yellow]Star Wars[/] [grey]Ep.[/] [u]III[/]", "$113,000,000", "$108,435,841", "$850,035,635"));
                Update(70, () => table.AddRow("Dec 18, 2015", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VII[/]", "$245,000,000", "$247,966,675", "$2,068,223,624"));
                Update(70, () => table.AddRow("Dec 15, 2017", "[yellow]Star Wars[/] [grey]Ep.[/] [u]VIII[/]", "$317,000,000", "$220,009,584", "$1,333,539,889"));
                Update(70, () => table.AddRow("Dec 20, 2019", "[yellow]Star Wars[/] [grey]Ep.[/] [u]IX[/]", "$245,000,000", "$177,383,864", "$1,074,114,248"));

                    // Column footer
                    Update(230, () => table.Columns[2].Footer("$1,633,000,000"));
                Update(230, () => table.Columns[3].Footer("$928,119,224"));
                Update(400, () => table.Columns[4].Footer("$10,318,030,576"));

                    // Column alignment
                    Update(230, () => table.Columns[2].RightAligned());
                Update(230, () => table.Columns[3].RightAligned());
                Update(400, () => table.Columns[4].RightAligned());

                    // Column titles
                    Update(70, () => table.Columns[0].Header("[bold]Release date[/]"));
                Update(70, () => table.Columns[1].Header("[bold]Title[/]"));
                Update(70, () => table.Columns[2].Header("[red bold]Budget[/]"));
                Update(70, () => table.Columns[3].Header("[green bold]Opening Weekend[/]"));
                Update(400, () => table.Columns[4].Header("[blue bold]Box office[/]"));

                    // Footers
                    Update(70, () => table.Columns[2].Footer("[red bold]$1,633,000,000[/]"));
                Update(70, () => table.Columns[3].Footer("[green bold]$928,119,224[/]"));
                Update(400, () => table.Columns[4].Footer("[blue bold]$10,318,030,576[/]"));

                    // Title
                    Update(500, () => table.Title("Star Wars Movies"));
                Update(400, () => table.Title("[[ [yellow]Star Wars Movies[/] ]]"));

                    // Borders
                    Update(230, () => table.BorderColor(Color.Yellow));
                Update(230, () => table.MinimalBorder());
                Update(230, () => table.SimpleBorder());
                Update(230, () => table.SimpleHeavyBorder());

                    // Caption
                    Update(400, () => table.Caption("[[ [blue]THE END[/] ]]"));
            });
    }
}
