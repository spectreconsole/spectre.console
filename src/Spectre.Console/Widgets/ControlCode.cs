namespace Spectre.Console;

/// <summary>
/// A control code.
/// </summary>
public sealed class ControlCode : Renderable
{
    private readonly Segment _segment;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlCode"/> class.
    /// </summary>
    /// <param name="control">The control code.</param>
    public ControlCode(string control)
    {
        _segment = Segment.Control(control);
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