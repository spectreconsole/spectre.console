using System.Threading;

namespace Spectre.Console.Examples
{
    public static class Program
    {
        public static void Main()
        {
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Default)
                .Start("[yellow]Initializing warp drive[/]", ctx =>
                {
                    // Initialize
                    ThreadSleeper.Sleep(3000);
                    WriteLogMessage("Starting gravimetric field displacement manifold");
                    ThreadSleeper.Sleep(1000);
                    WriteLogMessage("Warming up deuterium chamber");
                    ThreadSleeper.Sleep(2000);
                    WriteLogMessage("Generating antideuterium");

                    // Warp nacelles
                    ThreadSleeper.Sleep(3000);
                    ctx.Spinner(Spinner.Known.BouncingBar);
                    ctx.Status("[bold blue]Unfolding warp nacelles[/]");
                    WriteLogMessage("Unfolding left warp nacelle");
                    ThreadSleeper.Sleep(2000);
                    WriteLogMessage("Left warp nacelle [green]online[/]");
                    WriteLogMessage("Unfolding right warp nacelle");
                    ThreadSleeper.Sleep(1000);
                    WriteLogMessage("Right warp nacelle [green]online[/]");

                    // Warp bubble
                    ThreadSleeper.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Star2);
                    ctx.Status("[bold blue]Generating warp bubble[/]");
                    ThreadSleeper.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.Status("[bold blue]Stabilizing warp bubble[/]");

                    // Safety
                    ctx.Spinner(Spinner.Known.Monkey);
                    ctx.Status("[bold blue]Performing safety checks[/]");
                    WriteLogMessage("Enabling interior dampening");
                    ThreadSleeper.Sleep(2000);
                    WriteLogMessage("Interior dampening [green]enabled[/]");

                    // Warp!
                    ThreadSleeper.Sleep(3000);
                    ctx.Spinner(Spinner.Known.Moon);
                    WriteLogMessage("Preparing for warp");
                    ThreadSleeper.Sleep(1000);
                    for (var warp = 1; warp < 10; warp++)
                    {
                        ctx.Status($"[bold blue]Warp {warp}[/]");
                        ThreadSleeper.Sleep(500);
                    }
                });

            // Done
            AnsiConsole.MarkupLine("[bold green]Crusing at Warp 9.8[/]");
        }

        private static void WriteLogMessage(string message)
        {
            AnsiConsole.MarkupLine($"[grey]LOG:[/] {message}[grey]...[/]");
        }
    }
}
