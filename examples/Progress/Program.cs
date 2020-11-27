using System;
using System.Collections.Generic;
using System.Threading;
using Spectre.Console;

namespace ProgressExample
{
    public static class Program
    {
        public static void Main()
        {
            AnsiConsole.MarkupLine("[yellow]Initializing warp drive[/]...");

            // Show progress
            AnsiConsole.Progress()
                .AutoClear(false)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    new RemainingTimeColumn(),      // Remaining time
                    new SpinnerColumn(),            // Spinner
                })
                .Start(ctx =>
                {
                    var random = new Random(DateTime.Now.Millisecond);
                    var tasks = CreateTasks(ctx, random);

                    while (!ctx.IsFinished)
                    {
                        // Increment progress
                        foreach (var (task, increment) in tasks)
                        {
                            task.Increment(random.NextDouble() * increment);
                        }

                        // Write some random things to the terminal
                        if (random.NextDouble() < 0.1)
                        {
                            WriteLogMessage();
                        }

                        // Simulate some delay
                        Thread.Sleep(100);
                    }
                });

            // Done
            AnsiConsole.MarkupLine("[green]Done![/]");
        }

        private static List<(ProgressTask, int)> CreateTasks(ProgressContext progress, Random random)
        {
            var tasks = new List<(ProgressTask, int)>();
            while (tasks.Count < 5)
            {
                if (DescriptionGenerator.TryGenerate(out var name))
                {
                    tasks.Add((progress.AddTask(name), random.Next(2, 10)));
                }
            }

            return tasks;
        }

        private static void WriteLogMessage()
        {
            AnsiConsole.MarkupLine(
                "[grey]LOG:[/] " +
                DescriptionGenerator.Generate() +
                "[grey]...[/]");
        }
    }
}
