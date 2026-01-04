namespace Spectre.Console;

/// <summary>
/// Represents a task list.
/// </summary>
public sealed class Progress
{
    private readonly IAnsiConsole _console;

    /// <summary>
    /// Gets or sets a optional custom render function.
    /// </summary>
    public Func<IRenderable, IReadOnlyList<ProgressTask>, IRenderable> RenderHook { get; set; } = (renderable, _) => renderable;

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

    /// <summary>
    /// Gets or sets a value indicating whether or not the task list should
    /// only include tasks not completed
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool HideCompleted { get; set; }

    /// <summary>
    /// Gets or sets the refresh rate if <c>AutoRefresh</c> is enabled.
    /// Defaults to 10 times/second.
    /// </summary>
    public TimeSpan RefreshRate { get; set; } = TimeSpan.FromMilliseconds(100);

    internal List<ProgressColumn> Columns { get; }

    internal ProgressRenderer? FallbackRenderer { get; set; }

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
    /// Starts the progress task list and returns a result.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="func">he action to execute.</param>
    /// <returns>The result.</returns>
    public T Start<T>(Func<ProgressContext, T> func)
    {
        var task = StartAsync(ctx => Task.FromResult(func(ctx)));
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts the progress task list.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task StartAsync(Func<ProgressContext, Task> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        _ = await StartAsync<object?>(async progressContext =>
        {
            await action(progressContext).ConfigureAwait(false);
            return default;
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Starts the progress task list and returns a result.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <typeparam name="T">The result type of task.</typeparam>
    /// <returns>A <see cref="Task{T}"/> representing the asynchronous operation.</returns>
    public async Task<T> StartAsync<T>(Func<ProgressContext, Task<T>> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return await _console.RunExclusive(async () =>
        {
            var renderer = CreateRenderer();
            renderer.Started();

            T result;

            try
            {
                using (new RenderHookScope(_console, renderer))
                {
                    var context = new ProgressContext(_console, renderer);

                    if (AutoRefresh)
                    {
                        using (var thread = new ProgressRefreshThread(context, renderer.RefreshRate))
                        {
                            result = await action(context).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        result = await action(context).ConfigureAwait(false);
                    }

                    context.Refresh();
                }
            }
            finally
            {
                renderer.Completed(AutoClear);
            }

            return result;
        }).ConfigureAwait(false);
    }

    private ProgressRenderer CreateRenderer()
    {
        var caps = _console.Profile.Capabilities;
        var interactive = caps.Interactive && caps.Ansi;

        if (interactive)
        {
            var columns = new List<ProgressColumn>(Columns);
            return new DefaultProgressRenderer(_console, columns, RefreshRate, HideCompleted, RenderHook);
        }
        else
        {
            return FallbackRenderer ?? new FallbackProgressRenderer();
        }
    }
}

/// <summary>
/// Contains extension methods for <see cref="Progress"/>.
/// </summary>
public static class ProgressExtensions
{
    /// <param name="progress">The <see cref="Progress"/> instance.</param>
    extension(Progress progress)
    {
        /// <summary>
        /// Sets the columns to be used for an <see cref="Progress"/> instance.
        /// </summary>
        /// <param name="columns">The columns to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Progress Columns(params ProgressColumn[] columns)
        {
            ArgumentNullException.ThrowIfNull(progress);

            ArgumentNullException.ThrowIfNull(columns);

            if (!columns.Any())
            {
                throw new InvalidOperationException("At least one column must be specified.");
            }

            progress.Columns.Clear();
            progress.Columns.AddRange(columns);

            return progress;
        }

        /// <summary>
        /// Sets an optional hook to intercept rendering.
        /// </summary>
        /// <param name="renderHook">The custom render function.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Progress UseRenderHook(Func<IRenderable, IReadOnlyList<ProgressTask>, IRenderable> renderHook)
        {
            progress.RenderHook = renderHook;

            return progress;
        }

        /// <summary>
        /// Sets whether or not auto refresh is enabled.
        /// If disabled, you will manually have to refresh the progress.
        /// </summary>
        /// <param name="enabled">Whether or not auto refresh is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Progress AutoRefresh(bool enabled)
        {
            ArgumentNullException.ThrowIfNull(progress);

            progress.AutoRefresh = enabled;

            return progress;
        }

        /// <summary>
        /// Sets whether or not auto clear is enabled.
        /// If enabled, the task tabled will be removed once
        /// all tasks have completed.
        /// </summary>
        /// <param name="enabled">Whether or not auto clear is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Progress AutoClear(bool enabled)
        {
            ArgumentNullException.ThrowIfNull(progress);

            progress.AutoClear = enabled;

            return progress;
        }

        /// <summary>
        /// Sets whether or not hide completed is enabled.
        /// If enabled, the task tabled will be removed once it is
        /// completed.
        /// </summary>
        /// <param name="enabled">Whether or not hide completed is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Progress HideCompleted(bool enabled)
        {
            ArgumentNullException.ThrowIfNull(progress);

            progress.HideCompleted = enabled;

            return progress;
        }
    }
}