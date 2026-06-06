namespace Spectre.Console;

/// <summary>
/// Renders things with line numbers.
/// </summary>
public sealed class LineNumbers : Renderable
{
    private readonly List<IRenderable> _children;

    /// <summary>
    /// Initializes a new instance of the <see cref="LineNumbers"/> class.
    /// </summary>
    /// <param name="items">The items to render with line numbers.</param>
    public LineNumbers(params IRenderable[] items)
        : this((IEnumerable<IRenderable>)items)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LineNumbers"/> class.
    /// </summary>
    /// <param name="children">The items to render with line numbers.</param>
    public LineNumbers(IEnumerable<IRenderable> children)
    {
        _children = new List<IRenderable>(children ?? throw new ArgumentNullException(nameof(children)));
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        var measurements = _children.Select(c => c.Measure(options, maxWidth)).ToArray();
        if (measurements.Length == 0)
        {
            return new Measurement(0, 0);
        }

        var totalLines = 0;

        if (_children.Count > 0)
        {
            totalLines = 1;
        }

        foreach (var child in _children)
        {
            var childSegments = child.Render(options, maxWidth);
            foreach (var (_, _, _, segment) in childSegments.Enumerate())
            {
                if (segment.IsLineBreak)
                {
                    totalLines++;
                }
            }
        }

        var padding = totalLines > 0 ? GetPadding(totalLines) : 0;

        return new Measurement(
            measurements.Max(c => c.Min + padding),
            measurements.Max(c => c.Max + padding));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var segments = new List<Segment>();
        var result = new List<Segment>();

        foreach (var child in _children)
        {
            var childSegments = child.Render(options, maxWidth);
            foreach (var (_, _, _, segment) in childSegments.Enumerate())
            {
                segments.Add(segment);
            }
        }

        var lineNumber = 1;
        var totalLines = segments.Count(s => s.IsLineBreak) + 1;
        var padding = GetPadding(totalLines);

        if (segments.Count > 0)
        {
            result.Add(GetLineNumber(lineNumber, padding));
        }

        foreach (var segment in segments)
        {
            result.Add(segment);

            if (segment.IsLineBreak)
            {
                lineNumber++;
                result.Add(GetLineNumber(lineNumber, padding));
            }
        }

        return result;
    }

    private static Segment GetLineNumber(int lineNumber, int padding)
    {
        var paddedLineNumber = lineNumber.ToString().PadRight(padding);
        return new Segment(paddedLineNumber);
    }

    private static int GetPadding(int totalLines)
    {
        return totalLines.ToString().Length + 1;
    }
}