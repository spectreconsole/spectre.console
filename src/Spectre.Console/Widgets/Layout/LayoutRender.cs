namespace Spectre.Console;

[DebuggerDisplay("{Region,nq}")]
internal sealed class LayoutRender
{
    public Region Region { get; }
    public List<SegmentLine> Render { get; }

    public LayoutRender(Region region, List<SegmentLine> render)
    {
        Region = region;
        Render = render ?? throw new ArgumentNullException(nameof(render));
    }
}