namespace Spectre.Console;

/// <summary>
/// Renders things in rows.
/// </summary>
public sealed class Rows : Renderable, IExpandable
{
    private readonly List<IRenderable> _children;

    /// <inheritdoc/>
    public bool Expand { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rows"/> class.
    /// </summary>
    /// <param name="items">The items to render as rows.</param>
    public Rows(params IRenderable[] items)
        : this((IEnumerable<IRenderable>)items)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rows"/> class.
    /// </summary>
    /// <param name="children">The items to render as rows.</param>
    public Rows(IEnumerable<IRenderable> children)
    {
        _children = new List<IRenderable>(children ?? throw new ArgumentNullException(nameof(children)));
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (Expand)
        {
            return new Measurement(maxWidth, maxWidth);
        }
        else
        {
            var measurements = _children.Select(c => c.Measure(options, maxWidth));
            return new Measurement(
                measurements.Min(c => c.Min),
                measurements.Min(c => c.Max));
        }
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var result = new List<Segment>();

        foreach (var child in _children)
        {
            var segments = child.Render(options, maxWidth);
            foreach (var (_, _, last, segment) in segments.Enumerate())
            {
                result.Add(segment);

                if (last)
                {
                    if (!segment.IsLineBreak)
                    {
                        result.Add(Segment.LineBreak);
                    }
                }
            }
        }

        return result;
    }
}