namespace Spectre.Console;

internal sealed class BreakdownTags : Renderable
{
    private readonly List<IBreakdownChartItem> _data;

    public int? Width { get; set; }
    public CultureInfo? Culture { get; set; }
    public bool ShowTagValues { get; set; } = true;
    public Func<double, CultureInfo, string>? ValueFormatter { get; set; }

    public BreakdownTags(List<IBreakdownChartItem> data)
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
        var culture = Culture ?? CultureInfo.InvariantCulture;

        var panels = new List<Panel>();
        foreach (var item in _data)
        {
            var panel = new Panel(GetTag(item, culture));
            panel.Inline = true;
            panel.Padding = new Padding(0, 0, 2, 0);
            panel.NoBorder();

            panels.Add(panel);
        }

        foreach (var segment in ((IRenderable)new Columns(panels).Padding(0, 0)).Render(options, maxWidth))
        {
            yield return segment;
        }
    }

    private string GetTag(IBreakdownChartItem item, CultureInfo culture)
    {
        return string.Format(
            culture, "[{0}]■[/] {1}",
            item.Color.ToMarkup() ?? "default",
            FormatValue(item, culture)).Trim();
    }

    private string FormatValue(IBreakdownChartItem item, CultureInfo culture)
    {
        var formatter = ValueFormatter ?? DefaultFormatter;

        if (ShowTagValues)
        {
            return string.Format(culture, "{0} [grey]{1}[/]",
                item.Label.EscapeMarkup(),
                formatter(item.Value, culture));
        }

        return item.Label.EscapeMarkup();
    }

    private static string DefaultFormatter(double value, CultureInfo culture)
    {
        return value.ToString(culture);
    }
}