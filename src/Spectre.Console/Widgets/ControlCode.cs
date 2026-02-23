namespace Spectre.Console;

/// <summary>
/// A control code.
/// </summary>
public sealed class ControlCode : Renderable
{
    private readonly Segment _segment;

    internal static ControlCode Empty { get; } = new(string.Empty);

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlCode"/> class.
    /// </summary>
    /// <param name="control">The control code.</param>
    public ControlCode(string control)
    {
        _segment = Segment.Control(control);
    }

    /// <summary>
    /// Creates a new <see cref="ControlCode"/> using a <see cref="AnsiWriter"/>.
    /// </summary>
    /// <param name="capabilities">The capabilities.</param>
    /// <param name="action">The <see cref="AnsiWriter"/> action.</param>
    /// <returns>A new <see cref="ControlCode"/> instance.</returns>
    public static ControlCode Create(
        IReadOnlyCapabilities capabilities,
        Action<AnsiWriter> action)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(action);

        return new ControlCode(
            AnsiStringWriter.Shared.Write(
                capabilities, action));
    }

    /// <summary>
    /// Creates a new <see cref="ControlCode"/> using a <see cref="AnsiWriter"/>.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="action">The <see cref="AnsiWriter"/> action.</param>
    /// <returns>A new <see cref="ControlCode"/> instance.</returns>
    public static ControlCode Create(
        IAnsiConsole console,
        Action<AnsiWriter> action)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(action);

        return new ControlCode(
            AnsiStringWriter.Shared.Write(
                console.Profile.Capabilities, action));
    }

    /// <inheritdoc />
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        return new Measurement(0, 0);
    }

    /// <inheritdoc />
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options.Ansi)
        {
            yield return _segment;
        }
    }
}