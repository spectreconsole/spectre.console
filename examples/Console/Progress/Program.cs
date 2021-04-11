using System;
using System.Collections.Generic;
using System.Threading;

namespace Spectre.Console.Examples
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

                    // Create some tasks
                    var tasks = CreateTasks(ctx, random);
                    var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

                    // Wait for all tasks (except the indeterminate one) to complete
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

                    // Now start the "warp" task
                    warpTask.StartTask();
                    warpTask.IsIndeterminate(false);
                    while (!ctx.IsFinished)
                    {
                        warpTask.Increment(12 * random.NextDouble());

                        // Simulate some delay
                        Thread.Sleep(100);
                    }
                });

            // Done
            AnsiConsole.MarkupLine("[green]Done![/]");
        }

        private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext progress, Random random)
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
