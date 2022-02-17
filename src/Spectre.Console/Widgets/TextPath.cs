namespace Spectre.Console;

/// <summary>
/// Representation of a file system path.
/// </summary>
public sealed class TextPath : IRenderable
{
    private readonly string[] _parts;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextPath"/> class.
    /// </summary>
    /// <param name="path">The path to render.</param>
    public TextPath(string path)
    {
        // Normalize the path
        path ??= string.Empty;
        path = path.Replace('\\', '/');
        path = path.TrimEnd('/').Trim();

        // Get the distinct parts
        _parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <inheritdoc/>
    public Measurement Measure(RenderContext context, int maxWidth)
    {
        var fitted = Fit(context, maxWidth);
        var length = fitted.Sum(f => f.Length) + fitted.Length - 1;

        return new Measurement(
            Math.Min(length, maxWidth),
            Math.Max(length, maxWidth));
    }

    /// <inheritdoc/>
    public IEnumerable<Segment> Render(RenderContext context, int maxWidth)
    {
        var fitted = Fit(context, maxWidth);

        var parts = new List<Segment>();
        foreach (var (_, _, last, item) in fitted.Enumerate())
        {
            parts.Add(new Segment(item));

            if (!last)
            {
                parts.Add(new Segment("/", new Style(Color.Grey)));
            }
        }

        return parts;
    }

    private string[] Fit(RenderContext context, int maxWidth)
    {
        // No parts?
        if (_parts.Length == 0)
        {
            return _parts;
        }

        // Will it fit as is?
        if (_parts.Sum(p => Cell.GetCellLength(p)) + (_parts.Length - 1) < maxWidth)
        {
            return _parts;
        }

        var ellipsis = context.Unicode ? "…" : "...";
        var ellipsisLength = Cell.GetCellLength(ellipsis);

        if (_parts.Length >= 2)
        {
            // Try popping parts until it fits
            var queue = new Queue<string>(_parts.Skip(1).Take(_parts.Length - 2));
            while (queue.Count > 0)
            {
                // Remove the first item
                queue.Dequeue();

                // Get the current queue width in cells
                var queueWidth =
                        Cell.GetCellLength(_parts[0]) // First
                        + ellipsisLength // Ellipsis
                        + queue.Sum(p => Cell.GetCellLength(p)) // Middle
                        + Cell.GetCellLength(_parts.Last()) // Last
                        + queue.Count + 2; // Separators

                // Will it fit?
                if (maxWidth >= queueWidth)
                {
                    var result = new List<string>();
                    result.Add(_parts[0]);
                    result.Add(ellipsis);
                    result.AddRange(queue);
                    result.Add(_parts.Last());
                    return result.ToArray();
                }
            }
        }

        // Just trim the last part so it fits
        var last = _parts.Last();
        var take = Math.Max(0, maxWidth - ellipsisLength);
        return new[] { string.Concat(ellipsis, last.Substring(last.Length - take, take)) };
    }
}
