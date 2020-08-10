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
    /// Represents text with color and decorations.
    /// </summary>
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    [DebuggerDisplay("{_text,nq}")]
    public sealed class Text : IRenderable
    {
        private readonly List<Span> _spans;
        private string _text;

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public Justify Alignment { get; set; } = Justify.Left;

        private sealed class Span
        {
            public int Start { get; }
            public int End { get; }
            public Style Style { get; }

            public Span(int start, int end, Style style)
            {
                Start = start;
                End = end;
                Style = style ?? Style.Plain;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Console.Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        internal Text(string text)
        {
            _text = text?.NormalizeLineEndings() ?? throw new ArgumentNullException(nameof(text));
            _spans = new List<Span>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="foreground">The foreground.</param>
        /// <param name="background">The background.</param>
        /// <param name="decoration">The text decoration.</param>
        /// <returns>A <see cref="Text"/> instance.</returns>
        public static Text New(
            string text,
            Color? foreground = null,
            Color? background = null,
            Decoration? decoration = null)
        {
            var result = MarkupParser.Parse(text, new Style(foreground, background, decoration));
            return result;
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>The same <see cref="Text"/> instance.</returns>
        public Text WithAlignment(Justify alignment)
        {
            Alignment = alignment;
            return this;
        }

        /// <summary>
        /// Appends some text with the specified color and decorations.
        /// </summary>
        /// <param name="text">The text to append.</param>
        /// <param name="style">The text style.</param>
        public void Append(string text, Style style)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var start = _text.Length;
            var end = _text.Length + text.Length;

            _text += text;

            Stylize(start, end, style);
        }

        /// <summary>
        /// Stylizes a part of the text.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        /// <param name="style">The style to apply.</param>
        public void Stylize(int start, int end, Style style)
        {
            if (start >= end)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start position must be less than the end position.");
            }

            start = Math.Max(start, 0);
            end = Math.Min(end, _text.Length);

            _spans.Add(new Span(start, end, style));
        }

        /// <inheritdoc/>
        Measurement IRenderable.Measure(RenderContext context, int maxWidth)
        {
            if (string.IsNullOrEmpty(_text))
            {
                return new Measurement(1, 1);
            }

            // TODO: Write some kind of tokenizer for this
            var min = Segment.SplitLines(((IRenderable)this).Render(context, maxWidth))
                .SelectMany(line => line.Select(segment => segment.Text.Length))
                .Max();

            var max = _text.SplitLines().Max(x => Cell.GetCellLength(context.Encoding, x));

            return new Measurement(min, max);
        }

        /// <inheritdoc/>
        IEnumerable<Segment> IRenderable.Render(RenderContext context, int width)
        {
            if (string.IsNullOrEmpty(_text))
            {
                return Array.Empty<Segment>();
            }

            if (width == 0)
            {
                return Array.Empty<Segment>();
            }

            var result = new List<Segment>();
            var segments = SplitLineBreaks(CreateSegments());

            var justification = context.Justification ?? Alignment;

            foreach (var (_, _, last, line) in Segment.SplitLines(segments, width).Enumerate())
            {
                var length = line.Sum(l => l.StripLineEndings().CellLength(context.Encoding));

                if (length < width)
                {
                    // Justify right side
                    if (justification == Justify.Right)
                    {
                        var diff = width - length;
                        result.Add(new Segment(new string(' ', diff)));
                    }
                    else if (justification == Justify.Center)
                    {
                        var diff = (width - length) / 2;
                        result.Add(new Segment(new string(' ', diff)));
                    }
                }

                // Render the line.
                foreach (var segment in line)
                {
                    result.Add(segment.StripLineEndings());
                }

                // Justify left side
                if (length < width)
                {
                    if (justification == Justify.Center)
                    {
                        var diff = (width - length) / 2;
                        result.Add(new Segment(new string(' ', diff)));

                        var remainder = (width - length) % 2;
                        if (remainder != 0)
                        {
                            result.Add(new Segment(new string(' ', remainder)));
                        }
                    }
                }

                if (!last || line.Count == 0)
                {
                    result.Add(Segment.LineBreak());
                }
            }

            return result;
        }

        private IEnumerable<Segment> SplitLineBreaks(IEnumerable<Segment> segments)
        {
            // Creates individual segments of line breaks.
            var result = new List<Segment>();
            var queue = new Stack<Segment>(segments.Reverse());

            while (queue.Count > 0)
            {
                var segment = queue.Pop();

                var index = segment.Text.IndexOf("\n", StringComparison.OrdinalIgnoreCase);
                if (index == -1)
                {
                    if (!string.IsNullOrEmpty(segment.Text))
                    {
                        result.Add(segment);
                    }
                }
                else
                {
                    var (first, second) = segment.Split(index);
                    if (!string.IsNullOrEmpty(first.Text))
                    {
                        result.Add(first);
                    }

                    result.Add(Segment.LineBreak());
                    queue.Push(new Segment(second.Text.Substring(1), second.Style));
                }
            }

            return result;
        }

        private IEnumerable<Segment> CreateSegments()
        {
            // This excellent algorithm to sort spans was ported and adapted from
            // https://github.com/willmcgugan/rich/blob/eb2f0d5277c159d8693636ec60c79c5442fd2e43/rich/text.py#L492

            // Create the style map.
            var styleMap = _spans.SelectIndex((span, index) => (span, index)).ToDictionary(x => x.index + 1, x => x.span.Style);
            styleMap[0] = Style.Plain;

            // Create a span list.
            var spans = new List<(int Offset, bool Leaving, int Style)>();
            spans.AddRange(_spans.SelectIndex((span, index) => (span.Start, false, index + 1)));
            spans.AddRange(_spans.SelectIndex((span, index) => (span.End, true, index + 1)));
            spans = spans.OrderBy(x => x.Offset).ThenBy(x => !x.Leaving).ToList();

            // Keep track of applied styles using a stack
            var styleStack = new Stack<int>();

            // Now build the segments.
            var result = new List<Segment>();
            foreach (var (offset, leaving, style, nextOffset) in BuildSkipList(spans))
            {
                if (leaving)
                {
                    // Leaving
                    styleStack.Pop();
                }
                else
                {
                    // Entering
                    styleStack.Push(style);
                }

                if (nextOffset > offset)
                {
                    // Build the current style from the stack
                    var styleIndices = styleStack.OrderBy(index => index).ToArray();
                    var currentStyle = Style.Plain.Combine(styleIndices.Select(index => styleMap[index]));

                    // Create segment
                    var text = _text.Substring(offset, Math.Min(_text.Length - offset, nextOffset - offset));
                    result.Add(new Segment(text, currentStyle));
                }
            }

            return result;
        }

        private static IEnumerable<(int Offset, bool Leaving, int Style, int NextOffset)> BuildSkipList(
            List<(int Offset, bool Leaving, int Style)> spans)
        {
            return spans.Zip(spans.Skip(1), (first, second) => (first, second)).Select(
                    x => (x.first.Offset, x.first.Leaving, x.first.Style, NextOffset: x.second.Offset));
        }
    }
}
