using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Spectre.Console.Internal;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents a renderable segment.
    /// </summary>
    [DebuggerDisplay("{Text,nq}")]
    public class Segment
    {
        /// <summary>
        /// Gets the segment text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets a value indicating whether or not this is an expicit line break
        /// that should be preserved.
        /// </summary>
        public bool IsLineBreak { get; }

        /// <summary>
        /// Gets the segment style.
        /// </summary>
        public Style Style { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="text">The segment text.</param>
        public Segment(string text)
            : this(text, Style.Plain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="text">The segment text.</param>
        /// <param name="style">The segment style.</param>
        public Segment(string text, Style style)
            : this(text, style, false)
        {
        }

        private Segment(string text, Style style, bool lineBreak)
        {
            Text = text?.NormalizeLineEndings() ?? throw new ArgumentNullException(nameof(text));
            Style = style;
            IsLineBreak = lineBreak;
        }

        /// <summary>
        /// Creates a segment that represents an implicit line break.
        /// </summary>
        /// <returns>A segment that represents an implicit line break.</returns>
        public static Segment LineBreak()
        {
            return new Segment("\n", Style.Plain, true);
        }

        /// <summary>
        /// Gets the number of cells that this segment
        /// occupies in the console.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns>The number of cells that this segment occupies in the console.</returns>
        public int CellLength(Encoding encoding)
        {
            return Text.CellLength(encoding);
        }

        /// <summary>
        /// Returns a new segment without any trailing line endings.
        /// </summary>
        /// <returns>A new segment without any trailing line endings.</returns>
        public Segment StripLineEndings()
        {
            return new Segment(Text.TrimEnd('\n'), Style);
        }

        /// <summary>
        /// Splits the segment at the offset.
        /// </summary>
        /// <param name="offset">The offset where to split the segment.</param>
        /// <returns>One or two new segments representing the split.</returns>
        public (Segment First, Segment Second) Split(int offset)
        {
            if (offset < 0)
            {
                return (this, null);
            }

            if (offset >= Text.Length)
            {
                return (this, null);
            }

            return (
                new Segment(Text.Substring(0, offset), Style),
                new Segment(Text.Substring(offset, Text.Length - offset), Style));
        }

        /// <summary>
        /// Splits the provided segments into lines.
        /// </summary>
        /// <param name="segments">The segments to split.</param>
        /// <returns>A collection of lines.</returns>
        public static List<SegmentLine> SplitLines(IEnumerable<Segment> segments)
        {
            return SplitLines(segments, int.MaxValue);
        }

        /// <summary>
        /// Splits the provided segments into lines with a maximum width.
        /// </summary>
        /// <param name="segments">The segments to split into lines.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <returns>A list of lines.</returns>
        public static List<SegmentLine> SplitLines(IEnumerable<Segment> segments, int maxWidth)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var lines = new List<SegmentLine>();
            var line = new SegmentLine();

            var stack = new Stack<Segment>(segments.Reverse());

            while (stack.Count > 0)
            {
                var segment = stack.Pop();

                if (line.Length + segment.Text.Length > maxWidth)
                {
                    var diff = -(maxWidth - (line.Length + segment.Text.Length));
                    var offset = segment.Text.Length - diff;

                    var (first, second) = segment.Split(offset);

                    line.Add(first);
                    lines.Add(line);
                    line = new SegmentLine();

                    if (second != null)
                    {
                        stack.Push(second);
                    }

                    continue;
                }

                if (segment.Text.Contains("\n"))
                {
                    if (segment.Text == "\n")
                    {
                        if (line.Length > 0 || segment.IsLineBreak)
                        {
                            lines.Add(line);
                            line = new SegmentLine();
                        }

                        continue;
                    }

                    var text = segment.Text;
                    while (text != null)
                    {
                        var parts = text.SplitLines();
                        if (parts.Length > 0)
                        {
                            if (parts[0].Length > 0)
                            {
                                line.Add(new Segment(parts[0], segment.Style));
                            }
                        }

                        if (parts.Length > 1)
                        {
                            if (line.Length > 0)
                            {
                                lines.Add(line);
                                line = new SegmentLine();
                            }

                            text = string.Concat(parts.Skip(1).Take(parts.Length - 1));
                        }
                        else
                        {
                            text = null;
                        }
                    }
                }
                else
                {
                    line.Add(segment);
                }
            }

            if (line.Count > 0)
            {
                lines.Add(line);
            }

            return lines;
        }
    }
}
