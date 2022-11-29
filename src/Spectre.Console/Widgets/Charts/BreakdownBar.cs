namespace Spectre.Console;

internal sealed class BreakdownBar : Renderable
{
    private readonly List<IBreakdownChartItem> _data;

    public int? Width { get; set; }

    public BreakdownBar(List<IBreakdownChartItem> data)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
    }

    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);
        return new Measurement(width, width);
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = Math.Min(Width ?? maxWidth, maxWidth);

        // Chart
        var maxValue = _data.Sum(i => i.Value);
        var items = _data.ToArray();
        var bars = Ratio.Distribute(width, items.Select(i => Math.Max(0, (int)(width * (i.Value / maxValue)))).ToArray());

        for (var index = 0; index < items.Length; index++)
        {
            yield return new Segment(new string('█', bars[index]), new Style(items[index].Color));
        }

        yield return Segment.LineBreak;
    }
}