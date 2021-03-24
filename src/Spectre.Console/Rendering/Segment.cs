using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        /// Gets a value indicating whether or not this is an explicit line break
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
        /// Gets a value indicating whether or not his is a
        /// control code such as cursor movement.
        /// </summary>
        public bool IsControlCode { get; }

        /// <summary>
        /// Gets the segment style.
        /// </summary>
        public Style Style { get; }

        /// <summary>
        /// Gets a segment representing a line break.
        /// </summary>
        public static Segment LineBreak { get; } = new Segment(Environment.NewLine, Style.Plain, true, false);

        /// <summary>
        /// Gets an empty segment.
        /// </summary>
        public static Segment Empty { get; } = new Segment(string.Empty, Style.Plain, false, false);

        /// <summary>
        /// Creates padding segment.
        /// </summary>
        /// <param name="size">Number of whitespace characters.</param>
        /// <returns>Segment for specified padding size.</returns>
        public static Segment Padding(int size) => new(new string(' ', size));

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
            : this(text, style, false, false)
        {
        }

        private Segment(string text, Style style, bool lineBreak, bool control)
        {
            Text = text?.NormalizeNewLines() ?? throw new ArgumentNullException(nameof(text));
            Style = style ?? throw new ArgumentNullException(nameof(style));
            IsLineBreak = lineBreak;
            IsWhiteSpace = string.IsNullOrWhiteSpace(text);
            IsControlCode = control;
        }

        /// <summary>
        /// Creates a control segment.
        /// </summary>
        /// <param name="control">The control code.</param>
        /// <returns>A segment representing a control code.</returns>
        public static Segment Control(string control)
        {
            return new Segment(control, Style.Plain, false, true);
        }

        /// <summary>
        /// Gets the number of cells that this segment
        /// occupies in the console.
        /// </summary>
        /// <returns>The number of cells that this segment occupies in the console.</returns>
        public int CellCount()
        {
            if (IsControlCode)
            {
                return 0;
            }

            return Cell.GetCellLength(Text);
        }

        /// <summary>
        /// Gets the number of cells that the segments occupies in the console.
        /// </summary>
        /// <param name="segments">The segments to measure.</param>
        /// <returns>The number of cells that the segments occupies in the console.</returns>
        public static int CellCount(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var sum = 0;
            foreach (var segment in segments)
            {
                sum += segment.CellCount();
            }

            return sum;
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

            if (offset >= CellCount())
            {
                return (this, null);
            }

            var index = 0;
            if (offset > 0)
            {
                var accumulated = 0;
                foreach (var character in Text)
                {
                    index++;
                    accumulated += Cell.GetCellLength(character);
                    if (accumulated >= offset)
                    {
                        break;
                    }
                }
            }

            return (
                new Segment(Text.Substring(0, index), Style),
                new Segment(Text.Substring(index, Text.Length - index), Style));
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
        /// Splits the provided segments into lines.
        /// </summary>
        /// <param name="segments">The segments to split.</param>
        /// <returns>A collection of lines.</returns>
        public static List<SegmentLine> SplitLines(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

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
                var segmentLength = segment.CellCount();

                // Does this segment make the line exceed the max width?
                var lineLength = line.CellCount();
                if (lineLength + segmentLength > maxWidth)
                {
                    var diff = -(maxWidth - (lineLength + segmentLength));
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

                // Does the segment contain a newline?
                if (segment.Text.ContainsExact("\n"))
                {
                    // Is it a new line?
                    if (segment.Text == "\n")
                    {
                        if (line.Length != 0 || segment.IsLineBreak)
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

        /// <summary>
        /// Splits an overflowing segment into several new segments.
        /// </summary>
        /// <param name="segment">The segment to split.</param>
        /// <param name="overflow">The overflow strategy to use.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <returns>A list of segments that has been split.</returns>
        public static List<Segment> SplitOverflow(Segment segment, Overflow? overflow, int maxWidth)
        {
            if (segment is null)
            {
                throw new ArgumentNullException(nameof(segment));
            }

            if (segment.CellCount() <= maxWidth)
            {
                return new List<Segment>(1) { segment };
            }

            // Default to folding
            overflow ??= Overflow.Fold;

            var result = new List<Segment>();

            if (overflow == Overflow.Fold)
            {
                var totalLength = segment.Text.CellLength();
                var lengthLeft = totalLength;
                while (lengthLeft > 0)
                {
                    var index = totalLength - lengthLeft;

                    // How many characters should we take?
                    var take = Math.Min(maxWidth, totalLength - index);
                    if (take <= 0)
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
                if (Math.Max(0, maxWidth - 1) == 0)
                {
                    result.Add(new Segment(string.Empty, segment.Style));
                }
                else
                {
                    result.Add(new Segment(segment.Text.Substring(0, maxWidth), segment.Style));
                }
            }
            else if (overflow == Overflow.Ellipsis)
            {
                if (Math.Max(0, maxWidth - 1) == 0)
                {
                    result.Add(new Segment("…", segment.Style));
                }
                else
                {
                    result.Add(new Segment(segment.Text.Substring(0, maxWidth - 1) + "…", segment.Style));
                }
            }

            return result;
        }

        /// <summary>
        /// Truncates the segments to the specified width.
        /// </summary>
        /// <param name="segments">The segments to truncate.</param>
        /// <param name="maxWidth">The maximum width that the segments may occupy.</param>
        /// <returns>A list of segments that has been truncated.</returns>
        public static List<Segment> Truncate(IEnumerable<Segment> segments, int maxWidth)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var result = new List<Segment>();

            var totalWidth = 0;
            foreach (var segment in segments)
            {
                var segmentCellWidth = segment.CellCount();
                if (totalWidth + segmentCellWidth > maxWidth)
                {
                    break;
                }

                result.Add(segment);
                totalWidth += segmentCellWidth;
            }

            if (result.Count == 0 && segments.Any())
            {
                var segment = Truncate(segments.First(), maxWidth);
                if (segment != null)
                {
                    result.Add(segment);
                }
            }

            return result;
        }

        /// <summary>
        /// Truncates the segment to the specified width.
        /// </summary>
        /// <param name="segment">The segment to truncate.</param>
        /// <param name="maxWidth">The maximum width that the segment may occupy.</param>
        /// <returns>A new truncated segment, or <c>null</c>.</returns>
        public static Segment? Truncate(Segment? segment, int maxWidth)
        {
            if (segment is null)
            {
                return null;
            }

            if (segment.CellCount() <= maxWidth)
            {
                return segment;
            }

            var builder = new StringBuilder();
            foreach (var character in segment.Text)
            {
                var accumulatedCellWidth = builder.ToString().CellLength();
                if (accumulatedCellWidth >= maxWidth)
                {
                    break;
                }

                builder.Append(character);
            }

            if (builder.Length == 0)
            {
                return null;
            }

            return new Segment(builder.ToString(), segment.Style);
        }

        internal static IEnumerable<Segment> Merge(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var result = new List<Segment>();

            var previous = (Segment?)null;
            foreach (var segment in segments)
            {
                if (previous == null)
                {
                    previous = segment;
                    continue;
                }

                // Both control codes?
                if (segment.IsControlCode && previous.IsControlCode)
                {
                    previous = Control(previous.Text + segment.Text);
                    continue;
                }

                // Same style?
                if (previous.Style.Equals(segment.Style) && !previous.IsLineBreak && !previous.IsControlCode)
                {
                    previous = new Segment(previous.Text + segment.Text, previous.Style);
                    continue;
                }

                result.Add(previous);
                previous = segment;
            }

            if (previous != null)
            {
                result.Add(previous);
            }

            return result;
        }

        internal static List<Segment> TruncateWithEllipsis(IEnumerable<Segment> segments, int maxWidth)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            if (CellCount(segments) <= maxWidth)
            {
                return new List<Segment>(segments);
            }

            segments = TrimEnd(Truncate(segments, maxWidth - 1));
            if (!segments.Any())
            {
                return new List<Segment>(1);
            }

            var result = new List<Segment>(segments);
            result.Add(new Segment("…", result.Last().Style));
            return result;
        }

        internal static List<Segment> TrimEnd(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var stack = new Stack<Segment>();
            var checkForWhitespace = true;
            foreach (var segment in segments.Reverse())
            {
                if (checkForWhitespace)
                {
                    if (segment.IsWhiteSpace)
                    {
                        continue;
                    }

                    checkForWhitespace = false;
                }

                stack.Push(segment);
            }

            return stack.ToList();
        }

        // TODO: Move this to Table
        internal static List<List<SegmentLine>> MakeSameHeight(int cellHeight, List<List<SegmentLine>> cells)
        {
            if (cells is null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

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
