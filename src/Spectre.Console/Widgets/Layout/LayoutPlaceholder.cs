namespace Spectre.Console;

internal sealed class LayoutPlaceholder : Renderable
{
    public Layout Layout { get; }

    public LayoutPlaceholder(Layout layout)
    {
        Layout = layout ?? throw new ArgumentNullException(nameof(layout));
    }

    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var width = maxWidth;
        var height = options.Height ?? options.ConsoleSize.Height;
        var title = Layout.Name != null
            ? $"{Layout.Name} ({width} x {height})"
            : $"{width} x {height}";

        var renderable = (IRenderable)new Panel(string.Empty)
        {
            Width = maxWidth,
            Height = options.Height ?? options.ConsoleSize.Height,
            Header = new PanelHeader(title),
            Border = BoxBorder.Rounded,
        };

        return renderable.Render(options, maxWidth);
    }
}
