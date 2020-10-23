using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable horizontal rule.
    /// </summary>
    public sealed class Rule : Renderable, IAlignable
    {
        /// <summary>
        /// Gets or sets the rule title markup text.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the rule style.
        /// </summary>
        public Style? Style { get; set; }

        /// <summary>
        /// Gets or sets the rule's title alignment.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        public Rule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rule"/> class.
        /// </summary>
        /// <param name="title">The rule title markup text.</param>
        public Rule(string title)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            if (Title == null || maxWidth <= 6)
            {
                return GetLineWithoutTitle(maxWidth);
            }

            // Get the title and make sure it fits.
            var title = GetTitleSegments(context, Title, maxWidth - 6);
            if (Segment.CellCount(context, title) > maxWidth - 6)
            {
                // Truncate the title
                title = Segment.TruncateWithEllipsis(title, context, maxWidth - 6);
                if (!title.Any())
                {
                    // We couldn't fit the title at all.
                    return GetLineWithoutTitle(maxWidth);
                }
            }

            var (left, right) = GetLineSegments(context, maxWidth, title);

            var segments = new List<Segment>();
            segments.Add(left);
            segments.AddRange(title);
            segments.Add(right);
            segments.Add(Segment.LineBreak);

            return segments;
        }

        private IEnumerable<Segment> GetLineWithoutTitle(int maxWidth)
        {
            var text = new string('─', maxWidth);
            return new[]
            {
                new Segment(text, Style ?? Style.Plain),
                Segment.LineBreak,
            };
        }

        private (Segment Left, Segment Right) GetLineSegments(RenderContext context, int maxWidth, IEnumerable<Segment> title)
        {
            var alignment = Alignment ?? Justify.Center;

            var titleLength = Segment.CellCount(context, title);

            if (alignment == Justify.Left)
            {
                var left = new Segment(new string('─', 2) + " ", Style ?? Style.Plain);

                var rightLength = maxWidth - titleLength - left.CellCount(context) - 1;
                var right = new Segment(" " + new string('─', rightLength), Style ?? Style.Plain);

                return (left, right);
            }
            else if (alignment == Justify.Center)
            {
                var leftLength = ((maxWidth - titleLength) / 2) - 1;
                var left = new Segment(new string('─', leftLength) + " ", Style ?? Style.Plain);

                var rightLength = maxWidth - titleLength - left.CellCount(context) - 1;
                var right = new Segment(" " + new string('─', rightLength), Style ?? Style.Plain);

                return (left, right);
            }
            else if (alignment == Justify.Right)
            {
                var right = new Segment(" " + new string('─', 2), Style ?? Style.Plain);

                var leftLength = maxWidth - titleLength - right.CellCount(context) - 1;
                var left = new Segment(new string('─', leftLength) + " ", Style ?? Style.Plain);

                return (left, right);
            }

            throw new NotSupportedException("Unsupported alignment.");
        }

        private IEnumerable<Segment> GetTitleSegments(RenderContext context, string title, int width)
        {
            title = title.NormalizeLineEndings().Replace("\n", " ").Trim();
            var markup = new Markup(title, Style);
            return ((IRenderable)markup).Render(context.WithSingleLine(), width - 6);
        }
    }
}
