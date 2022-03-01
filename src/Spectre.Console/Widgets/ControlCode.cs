namespace Spectre.Console;

internal sealed class ControlCode : Renderable
{
    private readonly Segment _segment;

    public ControlCode(string control)
    {
        _segment = Segment.Control(control);
    }

    protected override Measurement Measure(RenderContext context, int maxWidth)
    {
        return new Measurement(0, 0);
    }

    protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
    {
        if (context.Ansi)
        {
            yield return _segment;
        }
    }
}