using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Spectre.Console.Internal;

namespace Spectre.Console.Rendering
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
        /// Gets a value indicating whether or not this is a whitespace
        /// that should be preserved but not taken into account when
        /// layouting text.
        /// </summary>
        public bool IsWhiteSpace { get; }

        /// <summary>
        /// Gets the segment style.
        /// </summary>
        public Style Style { get; }

        /// <summary>
        /// Gets a segment representing a line break.
        /// </summary>
        public static Segment LineBreak { get; } = new Segment(Environment.NewLine, Style.Plain, true);

        /// <summary>
        /// Gets an empty segment.
        /// </summary>
        public static Segment Empty { get; } = new Segment(string.Empty, Style.Plain, false);

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
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            Text = text.NormalizeLineEndings();
            Style = style ?? throw new ArgumentNullException(nameof(style));
            IsLineBreak = lineBreak;
            IsWhiteSpace = string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Gets the number of cells that this segment
        /// occupies in the console.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <returns>The number of cells that this segment occupies in the console.</returns>
        public int CellLength(RenderContext context)
        {
            return Text.CellLength(context);
        }

        /// <summary>
        /// Returns a new segment without any trailing line endings.
        /// </summary>
        /// <returns>A new segment without any trailing line endings.</returns>
        public Segment StripLineEndings()
        {
            return new Segment(Text.TrimEnd('\n').TrimEnd('\r'), Style);
        }

        /// <summary>
        /// Splits the segment at the offset.
        /// </summary>
        /// <param name="offset">The offset where to split the segment.</param>
        /// <returns>One or two new segments representing the split.</returns>
        public (Segment First, Segment? Second) Split(int offset)
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

                if (line.Width + segment.Text.Length > maxWidth)
                {
                    var diff = -(maxWidth - (line.Width + segment.Text.Length));
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
                        if (line.Width > 0 || segment.IsLineBreak)
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
                            if (line.Width > 0)
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

        internal static IEnumerable<Segment> Merge(IEnumerable<Segment> segments)
        {
            var result = new List<Segment>();

            var previous = (Segment?)null;
            foreach (var segment in segments)
            {
                if (previous == null)
                {
                    previous = segment;
                    continue;
                }

                // Same style?
                if (previous.Style.Equals(segment.Style) && !previous.IsLineBreak)
                {
                    previous = new Segment(previous.Text + segment.Text, previous.Style);
                }
                else
                {
                    result.Add(previous);
                    previous = segment;
                }
            }

            if (previous != null)
            {
                result.Add(previous);
            }

            return result;
        }

        /// <summary>
        /// Clones the segment.
        /// </summary>
        /// <returns>A new segment that's identical to this one.</returns>
        public Segment Clone()
        {
            return new Segment(Text, Style);
        }

        /// <summary>
        /// Splits an overflowing segment into several new segments.
        /// </summary>
        /// <param name="segment">The segment to split.</param>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <param name="context">The render context.</param>
        /// <param name="width">The maximum width.</param>
        /// <returns>A list of segments that has been split.</returns>
        public static List<Segment> SplitOverflow(Segment segment, Overflow? overflow, RenderContext context, int width)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment.CellLength(context) <= width)
            {
                return new List<Segment>(1) { segment };
            }

            // Default to folding
            overflow ??= Overflow.Fold;

            var result = new List<Segment>();

            if (overflow == Overflow.Fold)
            {
                var totalLength = segment.Text.CellLength(context);
                var lengthLeft = totalLength;
                while (lengthLeft > 0)
                {
                    var index = totalLength - lengthLeft;

                    // How many characters should we take?
                    var take = Math.Min(width, totalLength - index);
                    if (take == 0)
                    {
                        // This shouldn't really occur, but I don't like
                        // never ending loops if it does...
                        return new List<Segment>();
                    }

                    result.Add(new Segment(segment.Text.Substring(index, take), segment.Style));
                    lengthLeft -= take;
                }
            }
            else if (overflow == Overflow.Crop)
            {
                result.Add(new Segment(segment.Text.Substring(0, width), segment.Style));
            }
            else if (overflow == Overflow.Ellipsis)
            {
                result.Add(new Segment(segment.Text.Substring(0, width - 1) + "…", segment.Style));
            }

            return result;
        }

        internal static Segment TruncateWithEllipsis(string text, Style style, RenderContext context, int maxWidth)
        {
            return SplitOverflow(
                new Segment(text, style),
                Overflow.Ellipsis,
                context,
                maxWidth)[0];
        }

        internal static List<List<SegmentLine>> MakeSameHeight(int cellHeight, List<List<SegmentLine>> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.Count < cellHeight)
                {
                    while (cell.Count != cellHeight)
                    {
                        cell.Add(new SegmentLine());
                    }
                }
            }

            return cells;
        }
    }
}
