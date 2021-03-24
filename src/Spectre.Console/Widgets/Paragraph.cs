using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A paragraph of text where different parts
    /// of the paragraph can have individual styling.
    /// </summary>
    [DebuggerDisplay("{_text,nq}")]
    public sealed class Paragraph : Renderable, IAlignable, IOverflowable
    {
        private readonly List<SegmentLine> _lines;

        /// <summary>
        /// Gets or sets the alignment of the whole paragraph.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Gets or sets the text overflow strategy.
        /// </summary>
        public Overflow? Overflow { get; set; }

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
        /// <param name="style">The style of the text.</param>
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
        /// <param name="style">The style of the appended text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Paragraph Append(string text, Style? style = null)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            foreach (var (_, first, last, part) in text.SplitLines().Enumerate())
            {
                var current = part;

                if (first)
                {
                    var line = _lines.LastOrDefault();
                    if (line == null)
                    {
                        _lines.Add(new SegmentLine());
                        line = _lines.Last();
                    }

                    if (string.IsNullOrEmpty(current))
                    {
                        line.Add(Segment.Empty);
                    }
                    else
                    {
                        foreach (var span in current.SplitWords())
                        {
                            line.Add(new Segment(span, style ?? Style.Plain));
                        }
                    }
                }
                else
                {
                    var line = new SegmentLine();

                    if (string.IsNullOrEmpty(current))
                    {
                        line.Add(Segment.Empty);
                    }
                    else
                    {
                        foreach (var span in current.SplitWords())
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
        protected override Measurement Measure(RenderContext context, int maxWidth)
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
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_lines.Count == 0)
            {
                return Array.Empty<Segment>();
            }

            var lines = context.SingleLine
                ? new List<SegmentLine>(_lines)
                : SplitLines(maxWidth);

            // Justify lines
            var justification = context.Justification ?? Alignment ?? Justify.Left;
            if (justification != Justify.Left)
            {
                foreach (var line in lines)
                {
                    Aligner.Align(context, line, justification, maxWidth);
                }
            }

            if (context.SingleLine)
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

            var newLine = true;

            using var iterator = new SegmentLineIterator(_lines);
            var queue = new Queue<Segment>();
            while (true)
            {
                var current = (Segment?)null;
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

                newLine = false;

                if (current.IsLineBreak)
                {
                    lines.Add(line);
                    line = new SegmentLine();
                    newLine = true;
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
                            newLine = true;

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

                newLine = false;

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
}
