namespace Spectre.Console;

/// <summary>
/// Represents a live display.
/// </summary>
public sealed class LiveDisplay
{
    private readonly IAnsiConsole _console;
    private readonly IRenderable _target;

    /// <summary>
    /// Gets or sets a value indicating whether or not the live display should
    /// be cleared when it's done.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool AutoClear { get; set; }

    /// <summary>
    /// Gets or sets the vertical overflow strategy.
    /// </summary>
    public VerticalOverflow Overflow { get; set; } = VerticalOverflow.Ellipsis;

    /// <summary>
    /// Gets or sets the vertical overflow cropping strategy.
    /// </summary>
    public VerticalOverflowCropping Cropping { get; set; } = VerticalOverflowCropping.Top;

    /// <summary>
    /// Initializes a new instance of the <see cref="LiveDisplay"/> class.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="target">The target renderable to update.</param>
    public LiveDisplay(IAnsiConsole console, IRenderable target)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _target = target ?? throw new ArgumentNullException(nameof(target));
    }

    /// <summary>
    /// Starts the live display.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    public void Start(Action<LiveDisplayContext> action)
    {
        var task = StartAsync(ctx =>
        {
            action(ctx);
            return Task.CompletedTask;
        });

        task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts the live display.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="func">The action to execute.</param>
    /// <returns>The result.</returns>
    public T Start<T>(Func<LiveDisplayContext, T> func)
    {
        var task = StartAsync(ctx => Task.FromResult(func(ctx)));
        return task.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Starts the live display.
    /// </summary>
    /// <param name="func">The action to execute.</param>
    /// <returns>The result.</returns>
    public async Task StartAsync(Func<LiveDisplayContext, Task> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        _ = await StartAsync<object?>(async ctx =>
        {
            await func(ctx).ConfigureAwait(false);
            return default;
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Starts the live display.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="func">The action to execute.</param>
    /// <returns>The result.</returns>
    public async Task<T> StartAsync<T>(Func<LiveDisplayContext, Task<T>> func)
    {
        if (func is null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return await _console.RunExclusive(async () =>
        {
            var context = new LiveDisplayContext(_console, _target);
            context.SetOverflow(Overflow, Cropping);

            var renderer = new LiveDisplayRenderer(_console, context);
            renderer.Started();

            try
            {
                using (new RenderHookScope(_console, renderer))
                {
                    var result = await func(context).ConfigureAwait(false);
                    context.Refresh();
                    return result;
                }
            }
            finally
            {
                renderer.Completed(AutoClear);
            }
        }).ConfigureAwait(false);
    }
}

/// <summary>
/// Contains extension methods for <see cref="LiveDisplay"/>.
/// </summary>
public static class LiveDisplayExtensions
{
    /// <param name="live">The <see cref="LiveDisplay"/> instance.</param>
    extension(LiveDisplay live)
    {
        /// <summary>
        /// Sets whether or not auto clear is enabled.
        /// If enabled, the live display will be cleared when done.
        /// </summary>
        /// <param name="enabled">Whether or not auto clear is enabled.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay AutoClear(bool enabled)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.AutoClear = enabled;

            return live;
        }

        /// <summary>
        /// Sets the vertical overflow strategy.
        /// </summary>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay Overflow(VerticalOverflow overflow)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.Overflow = overflow;

            return live;
        }

        /// <summary>
        /// Sets the vertical overflow cropping strategy.
        /// </summary>
        /// <param name="cropping">The overflow cropping strategy to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public LiveDisplay Cropping(VerticalOverflowCropping cropping)
        {
            if (live is null)
            {
                throw new ArgumentNullException(nameof(live));
            }

            live.Cropping = cropping;

            return live;
        }
    }
}