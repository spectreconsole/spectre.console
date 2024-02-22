namespace Spectre.Console;

/// <summary>
/// A paragraph of text where different parts
/// of the paragraph can have individual styling.
/// </summary>
public sealed class Paragraph : Renderable, IHasJustification, IOverflowable
{
    private readonly List<SegmentLine> _lines;

    /// <summary>
    /// Gets or sets the alignment of the whole paragraph.
    /// </summary>
    public Justify? Justification { get; set; }

    /// <summary>
    /// Gets or sets the text overflow strategy.
    /// </summary>
    public Overflow? Overflow { get; set; }

    /// <summary>
    /// Gets the character count of the paragraph.
    /// </summary>
    public int Length => _lines.Sum(line => line.Length) + Math.Max(0, Lines - 1);

    /// <summary>
    /// Gets the number of lines in the paragraph.
    /// </summary>
    public int Lines => _lines.Count;

    /// <summary>
    /// Initializes a new instance of the <see cref="Paragraph"/> class.
    /// </summary>
    public Paragraph()
    {
        _lines = new List<SegmentLine>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Paragraph"/> class.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style of the text or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
    public Paragraph(string text, Style? style = null)
        : this()
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        Append(text, style);
    }

    /// <summary>
    /// Appends some text to this paragraph.
    /// </summary>
    /// <param name="text">The text to append.</param>
    /// <param name="style">The style of the appended text or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Paragraph Append(string text, Style? style = null)
    {
        if (text is null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        foreach (var (_, first, last, part) in text.SplitLines().Enumerate())
        {
            if (first)
            {
                var line = _lines.LastOrDefault();
                if (line == null)
                {
                    _lines.Add(new SegmentLine());
                    line = _lines.Last();
                }

                if (string.IsNullOrEmpty(part))
                {
                    line.Add(Segment.Empty);
                }
                else
                {
                    foreach (var span in part.SplitWords())
                    {
                        line.Add(new Segment(span, style ?? Style.Plain));
                    }
                }
            }
            else
            {
                var line = new SegmentLine();

                if (string.IsNullOrEmpty(part))
                {
                    line.Add(Segment.Empty);
                }
                else
                {
                    foreach (var span in part.SplitWords())
                    {
                        line.Add(new Segment(span, style ?? Style.Plain));
                    }
                }

                _lines.Add(line);
            }
        }

        return this;
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (_lines.Count == 0)
        {
            return new Measurement(0, 0);
        }

        var min = _lines.Max(line => line.Max(segment => segment.CellCount()));
        var max = _lines.Max(x => x.CellCount());

        return new Measurement(min, Math.Min(max, maxWidth));
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (_lines.Count == 0)
        {
            return Array.Empty<Segment>();
        }

        var lines = options.SingleLine
            ? new List<SegmentLine>(_lines)
            : SplitLines(maxWidth);

        // Justify lines
        var justification = options.Justification ?? Justification ?? Console.Justify.Left;
        if (justification != Console.Justify.Left)
        {
            foreach (var line in lines)
            {
                Aligner.Align(line, justification, maxWidth);
            }
        }

        if (options.SingleLine)
        {
            // Return the first line
            return lines[0].Where(segment => !segment.IsLineBreak);
        }

        return new SegmentLineEnumerator(lines);
    }

    private List<SegmentLine> Clone()
    {
        var result = new List<SegmentLine>();

        foreach (var line in _lines)
        {
            var newLine = new SegmentLine();
            foreach (var segment in line)
            {
                newLine.Add(segment);
            }

            result.Add(newLine);
        }

        return result;
    }

    private List<SegmentLine> SplitLines(int maxWidth)
    {
        if (maxWidth <= 0)
        {
            // Nothing fits, so return an empty line.
            return new List<SegmentLine>();
        }

        if (_lines.Max(x => x.CellCount()) <= maxWidth)
        {
            return Clone();
        }

        var lines = new List<SegmentLine>();
        var line = new SegmentLine();

        using var iterator = new SegmentLineIterator(_lines);
        var queue = new Queue<Segment>();
        while (true)
        {
            Segment? current;
            if (queue.Count == 0)
            {
                if (!iterator.MoveNext())
                {
                    break;
                }

                current = iterator.Current;
            }
            else
            {
                current = queue.Dequeue();
            }

            if (current == null)
            {
                throw new InvalidOperationException("Iterator returned empty segment.");
            }

            var newLine = false;

            if (current.IsLineBreak)
            {
                lines.Add(line);
                line = new SegmentLine();
                continue;
            }

            var length = current.CellCount();
            if (length > maxWidth)
            {
                // The current segment is longer than the width of the console,
                // so we will need to crop it up, into new segments.
                var segments = Segment.SplitOverflow(current, Overflow, maxWidth);
                if (segments.Count > 0)
                {
                    if (line.CellCount() + segments[0].CellCount() > maxWidth)
                    {
                        lines.Add(line);
                        line = new SegmentLine();

                        segments.ForEach(s => queue.Enqueue(s));
                        continue;
                    }
                    else
                    {
                        // Add the segment and push the rest of them to the queue.
                        line.Add(segments[0]);
                        segments.Skip(1).ForEach(s => queue.Enqueue(s));
                        continue;
                    }
                }
            }
            else
            {
                if (line.CellCount() + length > maxWidth)
                {
                    line.Add(Segment.Empty);
                    lines.Add(line);
                    line = new SegmentLine();
                    newLine = true;
                }
            }

            if (newLine && current.IsWhiteSpace)
            {
                continue;
            }

            line.Add(current);
        }

        // Flush remaining.
        if (line.Count > 0)
        {
            lines.Add(line);
        }

        return lines;
    }
}
