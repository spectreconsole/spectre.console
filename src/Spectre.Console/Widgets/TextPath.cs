namespace Spectre.Console;

/// <summary>
/// Representation of a file system path.
/// </summary>
public sealed class TextPath : IRenderable, IHasJustification
{
    private const string Ellipsis = "...";
    private const string UnicodeEllipsis = "…";

    private readonly string[] _parts;
    private readonly bool _rooted;
    private readonly bool _windows;

    /// <summary>
    /// Gets or sets the root style.
    /// </summary>
    public Style? RootStyle { get; set; }

    /// <summary>
    /// Gets or sets the separator style.
    /// </summary>
    public Style? SeparatorStyle { get; set; }

    /// <summary>
    /// Gets or sets the stem style.
    /// </summary>
    public Style? StemStyle { get; set; }

    /// <summary>
    /// Gets or sets the leaf style.
    /// </summary>
    public Style? LeafStyle { get; set; }

    /// <summary>
    /// Gets or sets the alignment.
    /// </summary>
    public Justify? Justification { get; set; }

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

        // Rooted Unix path?
        if (path.StartsWith("/"))
        {
            _rooted = true;
            _parts = new[] { "/" }.Concat(_parts).ToArray();
        }
        else if (_parts.Length > 0 && _parts[0].EndsWith(":"))
        {
            // Rooted Windows path
            _rooted = true;
            _windows = true;
        }
    }

    /// <inheritdoc/>
    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        var fitted = Fit(options, maxWidth);
        var separatorCount = fitted.Length - 1;
        var length = fitted.Sum(f => f.Length) + separatorCount;

        return new Measurement(
            Math.Min(length, maxWidth),
            Math.Max(length, maxWidth));
    }

    /// <inheritdoc/>
    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var rootStyle = RootStyle ?? Style.Plain;
        var separatorStyle = SeparatorStyle ?? Style.Plain;
        var stemStyle = StemStyle ?? Style.Plain;
        var leafStyle = LeafStyle ?? Style.Plain;

        var fitted = Fit(options, maxWidth);
        var parts = new List<Segment>();
        foreach (var (_, first, last, item) in fitted.Enumerate())
        {
            // Leaf?
            if (last)
            {
                parts.Add(new Segment(item, leafStyle));
            }
            else
            {
                if (first && _rooted)
                {
                    // Root
                    parts.Add(new Segment(item, rootStyle));

                    if (_windows)
                    {
                        // Windows root has a slash
                        parts.Add(new Segment("/", separatorStyle));
                    }
                }
                else
                {
                    // Normal path segment
                    parts.Add(new Segment(item, stemStyle));
                    parts.Add(new Segment("/", separatorStyle));
                }
            }
        }

        // Align the result
        Aligner.Align(parts, Justification, maxWidth);

        // Insert a line break
        parts.Add(Segment.LineBreak);

        return parts;
    }

    private string[] Fit(RenderOptions options, int maxWidth)
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

        var ellipsis = options.Unicode ? UnicodeEllipsis : Ellipsis;
        var ellipsisLength = Cell.GetCellLength(ellipsis);

        if (_parts.Length >= 2)
        {
            var skip = _rooted ? 1 : 0;
            var separatorCount = _rooted ? 2 : 1;
            var rootLength = _rooted ? Cell.GetCellLength(_parts[0]) : 0;

            // Try popping parts until it fits
            var queue = new Queue<string>(_parts.Skip(skip).Take(_parts.Length - separatorCount));
            while (queue.Count > 0)
            {
                // Remove the first item
                queue.Dequeue();

                // Get the current queue width in cells
                var queueWidth =
                        rootLength // Root (if rooted)
                        + ellipsisLength // Ellipsis
                        + queue.Sum(p => Cell.GetCellLength(p)) // Middle
                        + Cell.GetCellLength(_parts.Last()) // Last
                        + queue.Count + separatorCount; // Separators

                // Will it fit?
                if (maxWidth >= queueWidth)
                {
                    var result = new List<string>();

                    if (_rooted)
                    {
                        // Add the root
                        result.Add(_parts[0]);
                    }

                    result.Add(ellipsis);
                    result.AddRange(queue);
                    result.Add(_parts.Last());

                    return result.ToArray();
                }
            }
        }

        // Just trim the last part so it fits
        var last = _parts.Last();
        var take = Math.Min(last.Length, Math.Max(0, maxWidth - ellipsisLength));
        var start = Math.Max(0, last.Length - take);

        return new[] { string.Concat(ellipsis, last.Substring(start, take)) };
    }
}
