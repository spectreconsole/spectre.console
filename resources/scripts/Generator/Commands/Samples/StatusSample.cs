using System.Threading;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class StatusSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 10);

        private static void WriteLogMessage(string message)
        {
            AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
        }

        public override void Run(IAnsiConsole console)
        {
            console.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .Start("[yellow]Initializing warp drive[/]", ctx =>
                {
                    // Initialize
                    Thread.Sleep(3000);
                    WriteLogMessage("Starting gravimetric field displacement manifold");
                    Thread.Sleep(1000);
                    WriteLogMessage("Warming up deuterium chamber");
                    Thread.Sleep(2000);
                    WriteLogMessage("Generating antideuterium");

                    // Warp nacelles
                    Thread.Sleep(3000);
                    ctx.Spinner(Spinner.Known.BouncingBar);
                    ctx.Status("[bold blue]Unfolding warp nacelles[/]");
                    WriteLogMessage("Unfolding left warp nacelle");
                    Thread.Sleep(2000);
                    WriteLogMessage("Left warp nacelle [green]online[/]");
                    WriteLogMessage("Unfolding right warp nacelle");
                    Thread.Sleep(1000);
                    WriteLogMessage("Right warp nacelle [green]online[/]");

                    // Warp bubble
                    Thread.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Star2);
                    ctx.Status("[bold blue]Generating warp bubble[/]");
                    Thread.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.Status("[bold blue]Stabilizing warp bubble[/]");

                    // Safety
                    ctx.Spinner(Spinner.Known.Monkey);
                    ctx.Status("[bold blue]Performing safety checks[/]");
                    WriteLogMessage("Enabling interior dampening");
                    Thread.Sleep(2000);
                    WriteLogMessage("Interior dampening [green]enabled[/]");

                    // Warp!
                    Thread.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Moon);
                    WriteLogMessage("Preparing for warp");
                    Thread.Sleep(1000);
                    for (var warp = 1; warp < 10; warp++)
                    {
                        ctx.Status($"[bold blue]Warp {warp}[/]");
                        Thread.Sleep(500);
                    }
                });

            // Done
            AnsiConsole.MarkupLine("[bold green]Crusing at Warp 9.8[/]");        }
    }
}