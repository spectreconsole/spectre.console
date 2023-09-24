using Spectre.Console;
using Spectre.Console.Json;

namespace Generator.Commands.Samples
{
    public class LayoutSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (80, 24);

        public override void Run(IAnsiConsole console)
        {
            var layout = new Layout("Root")
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right")
                        .SplitRows(
                            new Layout("Top"),
                            new Layout("Bottom")));

            layout["Left"].Update(
                new Panel(
                    Align.Center(
                        new Markup("Hello [blue]World![/]"),
                        VerticalAlignment.Middle))
                    .Expand());

            AnsiConsole.Write(layout);
        }
    }
}