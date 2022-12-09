namespace Spectre.Console;

/// <summary>
/// Represents padding around a <see cref="IRenderable"/> object.
/// </summary>
public sealed class Padder : Renderable, IPaddable, IExpandable
{
    private readonly IRenderable _child;

    /// <inheritdoc/>
    public Padding? Padding { get; set; } = new Padding(1, 1, 1, 1);

    /// <summary>
    /// Gets or sets a value indicating whether or not the padding should
    /// fit the available space. If <c>false</c>, the padding width will be
    /// auto calculated. Defaults to <c>false</c>.
    /// </summary>
    public bool Expand { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Padder"/> class.
    /// </summary>
    /// <param name="child">The thing to pad.</param>
    /// <param name="padding">The padding. Defaults to <c>1,1,1,1</c> if null.</param>
    public Padder(IRenderable child, Padding? padding = null)
    {
        _child = child;
        Padding = padding ?? Padding;
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var paddingWidth = Padding?.GetWidth() ?? 0;
        var measurement = _child.Measure(options, maxWidth - paddingWidth);

        return new Measurement(
            measurement.Min + paddingWidth,
            measurement.Max + paddingWidth);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var paddingWidth = Padding?.GetWidth() ?? 0;
        var childWidth = maxWidth - paddingWidth;

        if (!Expand)
        {
            var measurement = _child.Measure(options, maxWidth - paddingWidth);
            childWidth = measurement.Max;
        }

        var width = childWidth + paddingWidth;
        var result = new List<Segment>();

        if (width > maxWidth)
        {
            width = maxWidth;
        }

        // Top padding
        for (var i = 0; i < Padding.GetTopSafe(); i++)
        {
            result.Add(Segment.Padding(width));
            result.Add(Segment.LineBreak);
        }

        var child = _child.Render(options, maxWidth - paddingWidth);
        foreach (var line in Segment.SplitLines(child))
        {
            // Left padding
            if (Padding.GetLeftSafe() != 0)
            {
                result.Add(Segment.Padding(Padding.GetLeftSafe()));
            }

            result.AddRange(line);

            // Right padding
            if (Padding.GetRightSafe() != 0)
            {
                result.Add(Segment.Padding(Padding.GetRightSafe()));
            }

            // Missing space on right side?
            var lineWidth = line.CellCount();
            var diff = width - lineWidth - Padding.GetLeftSafe() - Padding.GetRightSafe();
            if (diff > 0)
            {
                result.Add(Segment.Padding(diff));
            }

            result.Add(Segment.LineBreak);
        }

        // Bottom padding
        for (var i = 0; i < Padding.GetBottomSafe(); i++)
        {
            result.Add(Segment.Padding(width));
            result.Add(Segment.LineBreak);
        }

        return result;
    }
}