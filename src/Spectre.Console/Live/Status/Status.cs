namespace Spectre.Console;

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
    public Style? SpinnerStyle { get; set; } = Color.Yellow;

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
    /// <param name="action">The action to execute.</param>
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
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="status">The status to display.</param>
    /// <param name="func">The action to execute.</param>
    /// <returns>The result.</returns>
    public T Start<T>(string status, Func<StatusContext, T> func)
    {
        var task = StartAsync(status, ctx => Task.FromResult(func(ctx)));
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts a new status display.
    /// </summary>
    /// <param name="status">The status to display.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync(string status, Func<StatusContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _ = await StartAsync<object?>(status, async statusContext =>
        {
            await action(statusContext).ConfigureAwait(false);
            return default;
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Starts a new status display and returns a result.
    /// </summary>
    /// <typeparam name="T">The result type of task.</typeparam>
    /// <param name="status">The status to display.</param>
    /// <param name="func">The action to execute.</param>
    /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation.</returns>
    public async Task<T> StartAsync<T>(string status, Func<StatusContext, Task<T>> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        // Set the progress columns
        var spinnerColumn = new SpinnerColumn(Spinner ?? Spinner.Known.Default)
        {
            Style = SpinnerStyle ?? Style.Plain,
        };

        var progress = new Progress(_console)
        {
            FallbackRenderer = new FallbackStatusRenderer(),
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
            return await func(statusContext).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }
}

/// <summary>
/// Contains extension methods for <see cref="Status"/>.
/// </summary>
public static class StatusExtensions
{
    /// <param name="status">The <see cref="Status"/> instance.</param>
    extension(Status status)
    {
        /// <summary>
        /// Sets whether or not auto refresh is enabled.
        /// If disabled, you will manually have to refresh the progress.
        /// </summary>
        /// <param name="enabled">Whether or not auto refresh is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Status AutoRefresh(bool enabled)
        {
            ArgumentNullException.ThrowIfNull(status);

            status.AutoRefresh = enabled;
            return status;
        }

        /// <summary>
        /// Sets the spinner.
        /// </summary>
        /// <param name="status">The <see cref="Status"/> instance.</param>
        /// <param name="spinner">The spinner.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Status Spinner(Spinner spinner)
        {
            ArgumentNullException.ThrowIfNull(status);

            status.Spinner = spinner;
            return status;
        }

        /// <summary>
        /// Sets the spinner style.
        /// </summary>
        /// <param name="status">The <see cref="Status"/> instance.</param>
        /// <param name="style">The spinner style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Status SpinnerStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(status);

            status.SpinnerStyle = style;
            return status;
        }
    }
}