using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Console.Composition;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a piece of text.
    /// </summary>
    [DebuggerDisplay("{_text,nq}")]
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    public sealed class Text : IRenderable
    {
        private readonly List<SegmentLine> _lines;

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public Justify Alignment { get; set; } = Justify.Left;

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        public Text()
        {
            _lines = new List<SegmentLine>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="style">The style of the text.</param>
        public Text(string text, Style? style = null)
            : this()
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Append(text, style);
        }

        /// <summary>
        /// Creates a <see cref="Text"/> instance representing
        /// the specified markup text.
        /// </summary>
        /// <param name="text">The markup text.</param>
        /// <param name="style">The text style.</param>
        /// <returns>a <see cref="Text"/> instance representing the specified markup text.</returns>
        public static Text Markup(string text, Style? style = null)
        {
            var result = MarkupParser.Parse(text, style ?? Style.Plain);
            return result;
        }

        /// <inheritdoc/>
        public Measurement Measure(RenderContext context, int maxWidth)
        {
            if (_lines.Count == 0)
            {
                return new Measurement(0, 0);
            }

            var min = _lines.Max(line => line.Max(segment => segment.CellLength(context.Encoding)));
            var max = _lines.Max(x => x.CellWidth(context.Encoding));

            return new Measurement(min, max);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_lines.Count == 0)
            {
                return Array.Empty<Segment>();
            }

            var lines = SplitLines(context, maxWidth);

            // Justify lines
            var justification = context.Justification ?? Alignment;
            foreach (var (_, _, last, line) in lines.Enumerate())
            {
                var length = line.Sum(l => l.CellLength(context.Encoding));
                if (length < maxWidth)
                {
                    // Justify right side
                    if (justification == Justify.Right)
                    {
                        var diff = maxWidth - length;
                        line.Prepend(new Segment(new string(' ', diff)));
                    }
                    else if (justification == Justify.Center)
                    {
                        // Left side.
                        var diff = (maxWidth - length) / 2;
                        line.Prepend(new Segment(new string(' ', diff)));

                        // Right side
                        line.Add(new Segment(new string(' ', diff)));
                        var remainder = (maxWidth - length) % 2;
                        if (remainder != 0)
                        {
                            line.Add(new Segment(new string(' ', remainder)));
                        }
                    }
                }
            }

            return new SegmentLineEnumerator(lines);
        }

        /// <summary>
        /// Appends a piece of text.
        /// </summary>
        /// <param name="text">The text to append.</param>
        /// <param name="style">The style of the appended text.</param>
        public void Append(string text, Style? style = null)
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

        private List<SegmentLine> SplitLines(RenderContext context, int maxWidth)
        {
            if (_lines.Max(x => x.CellWidth(context.Encoding)) <= maxWidth)
            {
                return Clone();
            }

            var lines = new List<SegmentLine>();
            var line = new SegmentLine();

            var newLine = true;
            using (var iterator = new SegmentLineIterator(_lines))
            {
                while (iterator.MoveNext())
                {
                    var current = iterator.Current;
                    if (current == null)
                    {
                        throw new InvalidOperationException("Iterator returned empty segment.");
                    }

                    if (newLine && current.IsWhiteSpace && !current.IsLineBreak)
                    {
                        newLine = false;
                        continue;
                    }

                    newLine = false;

                    if (current.IsLineBreak)
                    {
                        line.Add(current);
                        lines.Add(line);
                        line = new SegmentLine();
                        newLine = true;
                        continue;
                    }

                    var length = current.CellLength(context.Encoding);
                    if (line.CellWidth(context.Encoding) + length > maxWidth)
                    {
                        line.Add(Segment.Empty);
                        lines.Add(line);
                        line = new SegmentLine();
                        newLine = true;
                    }

                    if (newLine && current.IsWhiteSpace)
                    {
                        continue;
                    }

                    newLine = false;

                    line.Add(current);
                }
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
