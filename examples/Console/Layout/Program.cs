using System;
using Spectre.Console;

namespace Layouts;

public static class Program
{
    public static void Main()
    {
        var layout = CreateLayout();
        AnsiConsole.Write(layout);

        Console.ReadKey(true);
    }

    private static Layout CreateLayout()
    {
        var layout = new Layout();

        layout.SplitRows(
            new Layout("Top")
                .SplitColumns(
                    new Layout("Left")
                        .SplitRows(
                            new Layout("LeftTop"),
                            new Layout("LeftBottom")),
                    new Layout("Right").Ratio(2),
                    new Layout("RightRight").Size(3)),
            new Layout("Bottom"));

        layout["LeftBottom"].Update(
            new Panel("[blink]PRESS ANY KEY TO QUIT[/]")
                .Expand()
                .BorderColor(Color.Yellow)
                .Padding(0, 0));

        layout["Right"].Update(
            new Panel(
                new Table()
                    .AddColumns("[blue]Qux[/]", "[green]Corgi[/]")
                    .AddRow("9", "8")
                    .AddRow("7", "6")
                    .Expand())
            .Header("A [yellow]Table[/] in a [blue]Panel[/] (Ratio=2)")
            .Expand());

        layout["RightRight"].Update(
            new Panel("Explicit-size-is-[yellow]3[/]")
                .BorderColor(Color.Yellow)
                .Padding(0, 0));

        layout["Bottom"].Update(
        new Panel(
                new FigletText("Hello World"))
            .Header("Some [green]Figlet[/] text")
            .Expand());

        return layout;
    }
}
