namespace Spectre.Console;

internal static class AnsiMarkupHighlighter
{
    public static string Highlight(string markup, string query, Style style)
    {
        ArgumentNullException.ThrowIfNull(markup);
        ArgumentNullException.ThrowIfNull(query);

        if (query.Length == 0)
        {
            return markup;
        }

        var parts = IndexedMarkupSegment.Parse(markup);
        var plain = string.Concat(parts.Select(p => p.Text));
        var startIndex = plain.IndexOf(query, StringComparison.Ordinal);
        var endIndex = startIndex + query.Length;

        if (startIndex == -1)
        {
            return markup;
        }

        var result = new List<AnsiMarkupSegment>();
        var partFound = false;

        foreach (var part in parts)
        {
            // Not found the part with the search expression yet?
            if (!partFound)
            {
                if (startIndex >= part.StartIndex && startIndex <= part.EndIndex)
                {
                    var beginning = part.Text[0..(startIndex - part.StartIndex)];
                    result.Add(new AnsiMarkupSegment(beginning, part.Style, part.Link));

                    var centerStart = startIndex - part.StartIndex;
                    var centerEnd = Math.Min(endIndex - startIndex, part.Text.Length - beginning.Length);
                    var center = part.Text.Substring(centerStart, centerEnd);
                    result.Add(new AnsiMarkupSegment(center, style, part.Link));

                    var endStart = part.Text.Length - center.Length - beginning.Length;
                    if (endStart > 0)
                    {
                        // Got an end as well
                        result.Add(new AnsiMarkupSegment(part.Text[^endStart..], part.Style, part.Link));
                    }

                    partFound = true;
                }
                else
                {
                    result.Add(new AnsiMarkupSegment(part.Text, part.Style, part.Link));
                }

                continue;
            }

            // Now continue with everything after the query

            if (part.StartIndex < endIndex)
            {
                var remaining = endIndex - part.StartIndex;
                if (remaining > part.Text.Length)
                {
                    result.Add(new AnsiMarkupSegment(part.Text, style, part.Link));
                }
                else
                {
                    result.Add(new AnsiMarkupSegment(part.Text[..remaining], style, part.Link));

                    if (remaining < part.Text.Length)
                    {
                        result.Add(new AnsiMarkupSegment(part.Text[remaining..], part.Style, part.Link));
                    }
                }
            }
            else
            {
                result.Add(new AnsiMarkupSegment(part.Text, part.Style, part.Link));
            }
        }

        // Merge and render all the segments
        return string.Concat(
            MergeSegments(result)
                .Select(item => item.ToString()));
    }

    private static List<AnsiMarkupSegment> MergeSegments(IEnumerable<AnsiMarkupSegment> segments)
    {
        var result = new List<AnsiMarkupSegment>();

        foreach (var (item, index) in segments.Select((item, index) => (item, index)))
        {
            if (index > 0 && result.Count > 0)
            {
                if (result[^1].Style.Equals(item.Style))
                {
                    result[^1].Text += item.Text;
                }
                else
                {
                    result.Add(new AnsiMarkupSegment(
                        item.Text, item.Style, item.Link));
                }
            }
            else
            {
                result.Add(new AnsiMarkupSegment(
                    item.Text, item.Style, item.Link));
            }
        }

        return result;
    }
}

file sealed class IndexedMarkupSegment
{
    private readonly AnsiMarkupSegment _segment;

    public string Text => _segment.Text;
    public Style Style => _segment.Style;
    public Link? Link => _segment.Link;
    public int StartIndex { get; }
    public int EndIndex => StartIndex + _segment.Text.Length;

    private IndexedMarkupSegment(AnsiMarkupSegment segment, int startIndex)
    {
        _segment = segment ?? throw new ArgumentNullException(nameof(segment));
        StartIndex = startIndex;
    }

    public static IndexedMarkupSegment[] Parse(string value)
    {
        var currentIndex = 0;
        return AnsiMarkup.Parse(value).Select(segment =>
        {
            var result = new IndexedMarkupSegment(segment, currentIndex);
            currentIndex += segment.Text.Length;
            return result;
        }).ToArray();
    }
}