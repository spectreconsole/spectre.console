using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console.Internal;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents a renderable segment.
    /// </summary>
    public sealed class Segment
    {
        /// <summary>
        /// Gets the segment text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the appearance of the segment.
        /// </summary>
        public Appearance Appearance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="text">The segment text.</param>
        public Segment(string text)
            : this(text, Appearance.Plain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Segment"/> class.
        /// </summary>
        /// <param name="text">The segment text.</param>
        /// <param name="appearance">The segment appearance.</param>
        public Segment(string text, Appearance appearance)
        {
            Text = text?.NormalizeLineEndings() ?? throw new ArgumentNullException(nameof(text));
            Appearance = appearance;
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
            return new Segment(Text.TrimEnd('\n'), Appearance);
        }

        /// <summary>
        /// Splits the provided segments into lines.
        /// </summary>
        /// <param name="segments">The segments to split.</param>
        /// <returns>A collection of lines.</returns>
        public static List<SegmentLine> Split(IEnumerable<Segment> segments)
        {
            if (segments is null)
            {
                throw new ArgumentNullException(nameof(segments));
            }

            var lines = new List<SegmentLine>();
            var line = new SegmentLine();

            foreach (var segment in segments)
            {
                if (segment.Text.Contains("\n"))
                {
                    if (segment.Text == "\n")
                    {
                        lines.Add(line);
                        line = new SegmentLine();
                        continue;
                    }

                    var text = segment.Text;
                    while (text != null)
                    {
                        var parts = text.SplitLines();
                        if (parts.Length > 0)
                        {
                            line.Add(new Segment(parts[0], segment.Appearance));
                        }

                        if (parts.Length > 1)
                        {
                            lines.Add(line);
                            line = new SegmentLine();

                            text = string.Concat(parts.Skip(1).Take(parts.Length - 1));
                            if (string.IsNullOrWhiteSpace(text))
                            {
                                text = null;
                            }
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
