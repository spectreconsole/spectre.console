using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a task list.
    /// </summary>
    public sealed class Progress
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Gets or sets a value indicating whether or not task list should auto refresh.
        /// Defaults to <c>true</c>.
        /// </summary>
        public bool AutoRefresh { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not the task list should
        /// be cleared once it completes.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool AutoClear { get; set; }

        internal List<ProgressColumn> Columns { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Progress"/> class.
        /// </summary>
        /// <param name="console">The console to render to.</param>
        public Progress(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));

            // Initialize with default columns
            Columns = new List<ProgressColumn>
            {
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
            };
        }

        /// <summary>
        /// Starts the progress task list.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public void Start(Action<ProgressContext> action)
        {
            var task = StartAsync(ctx =>
            {
                action(ctx);
                return Task.CompletedTask;
            });

            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Starts the progress task list.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync(Func<ProgressContext, Task> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var renderer = CreateRenderer();
            renderer.Started();

            try
            {
                using (new RenderHookScope(_console, renderer))
                {
                    var context = new ProgressContext(_console, renderer);

                    if (AutoRefresh)
                    {
                        using (var thread = new ProgressRefreshThread(context, renderer.RefreshRate))
                        {
                            await action(context).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        await action(context).ConfigureAwait(false);
                    }

                    context.Refresh();
                }
            }
            finally
            {
                renderer.Completed(AutoClear);
            }
        }

        private ProgressRenderer CreateRenderer()
        {
            var caps = _console.Capabilities;
            var interactive = caps.SupportsInteraction && caps.SupportsAnsi;

            if (interactive)
            {
                var columns = new List<ProgressColumn>(Columns);
                return new InteractiveProgressRenderer(_console, columns);
            }
            else
            {
                return new NonInteractiveProgressRenderer();
            }
        }
    }
}
