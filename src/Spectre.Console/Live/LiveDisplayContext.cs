namespace Spectre.Console;

/// <summary>
/// Represents a context that can be used to interact with a <see cref="LiveDisplay"/>.
/// </summary>
public sealed class LiveDisplayContext
{
    private readonly IAnsiConsole _console;

    internal object Lock { get; }
    internal LiveRenderable Live { get; }

    internal LiveDisplayContext(IAnsiConsole console, IRenderable target)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));

        Live = new LiveRenderable(_console, target);
        Lock = new object();
    }

    /// <summary>
    /// Updates the live display target.
    /// </summary>
    /// <param name="target">The new live display target.</param>
    public void UpdateTarget(IRenderable? target)
    {
        lock (Lock)
        {
            Live.SetRenderable(target);
            Refresh();
        }
    }

    /// <summary>
    /// Refreshes the live display.
    /// </summary>
    public void Refresh()
    {
        lock (Lock)
        {
            _console.Write(new ControlCode(string.Empty));
        }
    }

    internal void SetOverflow(VerticalOverflow overflow, VerticalOverflowCropping cropping)
    {
        Live.Overflow = overflow;
        Live.OverflowCropping = cropping;
    }
}