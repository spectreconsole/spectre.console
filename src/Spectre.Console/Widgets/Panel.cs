namespace Spectre.Console;

/// <summary>
/// A renderable panel.
/// </summary>
public sealed class Panel : Renderable, IHasBoxBorder, IHasBorder, IExpandable, IPaddable
{
    private const int EdgeWidth = 2;

    private readonly IRenderable _child;

    /// <inheritdoc/>
    public BoxBorder Border { get; set; } = BoxBorder.Square;

    /// <inheritdoc/>
    public bool UseSafeBorder { get; set; } = true;

    /// <inheritdoc/>
    public Style? BorderStyle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the panel should
    /// fit the available space. If <c>false</c>, the panel width will be
    /// auto calculated. Defaults to <c>false</c>.
    /// </summary>
    public bool Expand { get; set; }

    /// <summary>
    /// Gets or sets the padding.
    /// </summary>
    public Padding? Padding { get; set; } = new Padding(1, 0, 1, 0);

    /// <summary>
    /// Gets or sets the header.
    /// </summary>
    public PanelHeader? Header { get; set; }

    /// <summary>
    /// Gets or sets the width of the panel.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the panel.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the panel is inlined.
    /// </summary>
    internal bool Inline { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Panel"/> class.
    /// </summary>
    /// <param name="text">The panel content.</param>
    public Panel(string text)
        : this(new Markup(text))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Panel"/> class.
    /// </summary>
    /// <param name="content">The panel content.</param>
    public Panel(IRenderable content)
    {
        _child = content ?? throw new ArgumentNullException(nameof(content));
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var child = new Padder(_child, Padding);
        return Measure(options, maxWidth, child);
    }

    private Measurement Measure(RenderOptions options, int maxWidth, IRenderable child)
    {
        var edgeWidth = (options.GetSafeBorder(this) is not NoBoxBorder) ? EdgeWidth : 0;
        var childWidth = child.Measure(options, maxWidth - edgeWidth);

        if (Width != null)
        {
            var width = Width.Value - edgeWidth;
            if (width > childWidth.Max)
            {
                childWidth = new Measurement(
                    childWidth.Min,
                    width);
            }
        }

        return new Measurement(
            childWidth.Min + edgeWidth,
            childWidth.Max + edgeWidth);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var border = options.GetSafeBorder(this);
        var borderStyle = BorderStyle ?? Style.Plain;

        var showBorder = border is not NoBoxBorder;
        var edgeWidth = showBorder ? EdgeWidth : 0;

        var child = new Padder(_child, Padding);
        var width = Measure(options, maxWidth, child);

        var panelWidth = Math.Min(!Expand ? width.Max : maxWidth, maxWidth);
        var innerWidth = panelWidth - edgeWidth;

        var height = Height != null
            ? Height - 2
            : options.Height != null
                ? options.Height - 2
                : null;

        if (!Expand)
        {
            // Set the height to the explicit height (or null)
            // if the panel isn't expandable.
            height = Height != null ? Height - 2 : null;
        }

        // Start building the panel
        var result = new List<Segment>();

        // Panel top
        if (showBorder)
        {
            AddTopBorder(result, options, border, borderStyle, panelWidth);
        }

        // Split the child segments into lines.
        var childSegments = ((IRenderable)child).Render(options with { Height = height }, innerWidth);
        foreach (var (_, _, last, line) in Segment.SplitLines(childSegments, innerWidth, height).Enumerate())
        {
            if (line.Count == 1 && line[0].IsWhiteSpace)
            {
                // NOTE: This check might impact other things.
                // Hopefully not, but there is a chance.
                continue;
            }

            if (showBorder)
            {
                result.Add(new Segment(border.GetPart(BoxBorderPart.Left), borderStyle));
            }

            var content = new List<Segment>();
            content.AddRange(line);

            // Do we need to pad the panel?
            var length = line.Sum(segment => segment.CellCount());
            if (length < innerWidth)
            {
                var diff = innerWidth - length;
                content.Add(Segment.Padding(diff));
            }

            result.AddRange(content);

            if (showBorder)
            {
                result.Add(new Segment(border.GetPart(BoxBorderPart.Right), borderStyle));
            }

            // Don't emit a line break if this is the last
            // line, we're not showing the border, and we're
            // not rendering this inline.
            var emitLinebreak = !(last && !showBorder && !Inline);
            if (!emitLinebreak)
            {
                continue;
            }

            result.Add(Segment.LineBreak);
        }

        // Panel bottom
        if (showBorder)
        {
            result.Add(new Segment(border.GetPart(BoxBorderPart.BottomLeft), borderStyle));
            result.Add(new Segment(border.GetPart(BoxBorderPart.Bottom).Repeat(panelWidth - EdgeWidth), borderStyle));
            result.Add(new Segment(border.GetPart(BoxBorderPart.BottomRight), borderStyle));
        }

        // TODO: Need a better name for this?
        // If we're rendering this as part of an inline parent renderable,
        // such as columns, we should not emit the last line break.
        if (!Inline)
        {
            result.Add(Segment.LineBreak);
        }

        return result;
    }

    private void AddTopBorder(
        List<Segment> result, RenderOptions options, BoxBorder border,
        Style borderStyle, int panelWidth)
    {
        var rule = new Rule
        {
            Style = borderStyle,
            Border = border,
            TitlePadding = 1,
            TitleSpacing = 0,
            Title = Header?.Text,
            Justification = Header?.Justification ?? Justify.Left,
        };

        // Top left border
        result.Add(new Segment(border.GetPart(BoxBorderPart.TopLeft), borderStyle));

        // Top border (and header text if specified)
        result.AddRange(((IRenderable)rule).Render(options, panelWidth - 2).Where(x => !x.IsLineBreak));

        // Top right border
        result.Add(new Segment(border.GetPart(BoxBorderPart.TopRight), borderStyle));
        result.Add(Segment.LineBreak);
    }
}