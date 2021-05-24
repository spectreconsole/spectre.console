using System;
using System.Collections.Generic;
using System.Threading;
using Spectre.Console;

namespace Generator.Commands.Samples
{
    internal class ProgressSample : BaseSample
    {
        public override (int Cols, int Rows) ConsoleSize => (base.ConsoleSize.Cols, 10);

        public override IEnumerable<(string Name, Action<Capabilities> CapabilitiesAction)> GetCapabilities()
        {
            yield return ("non-interactive", capabilities =>
            {
                capabilities.Ansi = false;
                capabilities.Interactive = false;
                capabilities.Legacy = false;
                capabilities.Unicode = true;
                capabilities.ColorSystem = ColorSystem.TrueColor;
            });

            foreach (var capability in base.GetCapabilities())
            {
                yield return capability;
            }
        }

        public override void Run(IAnsiConsole console)
        {
            // Show progress
            console.Progress()
                .AutoClear(false)
                .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
                .Start(ctx =>
                {
                    var random = new Random(122978);

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
        }

        private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext progress, Random random)
        {
            var tasks = new List<(ProgressTask, int)>();

            var names = new[]
            {
                "Retriculating algorithms", "Colliding splines", "Solving quarks", "Folding data structures",
                "Rerouting capacitators "
            };

            for (var i = 0; i < 5; i++)
            {
                tasks.Add((progress.AddTask(names[i]), random.Next(2, 10)));
            }

            return tasks;
        }
    }
}