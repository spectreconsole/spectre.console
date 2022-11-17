namespace Spectre.Console;

internal abstract class LayoutSplitter
{
    public static LayoutSplitter Column { get; } = new ColumnSplitter();
    public static LayoutSplitter Row { get; } = new RowSplitter();
    public static LayoutSplitter Null { get; } = new NullSplitter();

    public abstract IEnumerable<(Layout Child, Region Region)> Divide(Region region, IEnumerable<Layout> layouts);

    private sealed class NullSplitter : LayoutSplitter
    {
        public override IEnumerable<(Layout Child, Region Region)> Divide(Region region, IEnumerable<Layout> layouts)
        {
            yield break;
        }
    }

    private sealed class ColumnSplitter : LayoutSplitter
    {
        public override IEnumerable<(Layout Child, Region Region)> Divide(Region region, IEnumerable<Layout> children)
        {
            var widths = Ratio.Resolve(region.Width, children);
            var offset = 0;

            foreach (var (child, childWidth) in children.Zip(widths, (child, width) => (child, width)))
            {
                yield return (child, new Region(region.X + offset, region.Y, childWidth, region.Height));
                offset += childWidth;
            }
        }
    }

    private sealed class RowSplitter : LayoutSplitter
    {
        public override IEnumerable<(Layout Child, Region Region)> Divide(Region region, IEnumerable<Layout> children)
        {
            var heights = Ratio.Resolve(region.Height, children);
            var offset = 0;

            foreach (var (child, childHeight) in children.Zip(heights, (child, height) => (child, height)))
            {
                yield return (child, new Region(region.X, region.Y + offset, region.Width, childHeight));
                offset += childHeight;
            }
        }
    }
}
