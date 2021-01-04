using System;
using System.Threading.Tasks;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a status display.
    /// </summary>
    public sealed class Status
    {
        private readonly IAnsiConsole _console;

        /// <summary>
        /// Gets or sets the spinner.
        /// </summary>
        public Spinner? Spinner { get; set; }

        /// <summary>
        /// Gets or sets the spinner style.
        /// </summary>
        public Style? SpinnerStyle { get; set; } = new Style(foreground: Color.Yellow);

        /// <summary>
        /// Gets or sets a value indicating whether or not status
        /// should auto refresh. Defaults to <c>true</c>.
        /// </summary>
        public bool AutoRefresh { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class.
        /// </summary>
        /// <param name="console">The console.</param>
        public Status(IAnsiConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <summary>
        /// Starts a new status display.
        /// </summary>
        /// <param name="status">The status to display.</param>
        /// <param name="action">he action to execute.</param>
        public void Start(string status, Action<StatusContext> action)
        {
            var task = StartAsync(status, ctx =>
            {
                action(ctx);
                return Task.CompletedTask;
            });

            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Starts a new status display.
        /// </summary>
        /// <param name="status">The status to display.</param>
        /// <param name="action">he action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task StartAsync(string status, Func<StatusContext, Task> action)
        {
            // Set the progress columns
            var spinnerColumn = new SpinnerColumn(Spinner ?? Spinner.Known.Default)
            {
                Style = SpinnerStyle ?? Style.Plain,
            };

            var progress = new Progress(_console)
            {
                FallbackRenderer = new StatusFallbackRenderer(),
                AutoClear = true,
                AutoRefresh = AutoRefresh,
            };

            progress.Columns(new ProgressColumn[]
            {
                spinnerColumn,
                new TaskDescriptionColumn(),
            });

            await progress.StartAsync(async ctx =>
            {
                var statusContext = new StatusContext(ctx, ctx.AddTask(status), spinnerColumn);
                await action(statusContext).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Starts a new status display.
        /// </summary>
        /// <param name="status">The status to display.</param>
        /// <param name="action">he action to execute.</param>
        /// <typeparam name="T">The result type of task.</typeparam>
        /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation.</returns>
        public async Task<T> StartAsync<T>(string status, Func<StatusContext, Task<T>> action)
        {
            // Set the progress columns
            var spinnerColumn = new SpinnerColumn(Spinner ?? Spinner.Known.Default)
            {
                Style = SpinnerStyle ?? Style.Plain,
            };

            var progress = new Progress(_console)
            {
                FallbackRenderer = new StatusFallbackRenderer(),
                AutoClear = true,
                AutoRefresh = AutoRefresh,
            };

            progress.Columns(new ProgressColumn[]
            {
                spinnerColumn,
                new TaskDescriptionColumn(),
            });

            return await progress.StartAsync(async ctx =>
            {
                var statusContext = new StatusContext(ctx, ctx.AddTask(status), spinnerColumn);
                return await action(statusContext).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
