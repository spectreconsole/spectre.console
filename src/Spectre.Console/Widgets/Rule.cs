using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable horizontal rule.
    /// </summary>
    public sealed class Rule : Renderable, IAlignable, IHasBoxBorder
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

        /// <inheritdoc/>
        public BoxBorder Border { get; set; } = BoxBorder.Square;

        internal int TitlePadding { get; set; } = 2;
        internal int TitleSpacing { get; set; } = 1;

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
            var extraLength = (2 * TitlePadding) + (2 * TitleSpacing);

            if (Title == null || maxWidth <= extraLength)
            {
                return GetLineWithoutTitle(context, maxWidth);
            }

            // Get the title and make sure it fits.
            var title = GetTitleSegments(context, Title, maxWidth - extraLength);
            if (Segment.CellCount(title) > maxWidth - extraLength)
            {
                // Truncate the title
                title = Segment.TruncateWithEllipsis(title, maxWidth - extraLength);
                if (!title.Any())
                {
                    // We couldn't fit the title at all.
                    return GetLineWithoutTitle(context, maxWidth);
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

        private IEnumerable<Segment> GetLineWithoutTitle(RenderContext context, int maxWidth)
        {
            var border = Border.GetSafeBorder(safe: !context.Unicode);
            var text = border.GetPart(BoxBorderPart.Top).Repeat(maxWidth);

            return new[]
            {
                new Segment(text, Style ?? Style.Plain),
                Segment.LineBreak,
            };
        }

        private IEnumerable<Segment> GetTitleSegments(RenderContext context, string title, int width)
        {
            title = title.NormalizeNewLines().ReplaceExact("\n", " ").Trim();
            var markup = new Markup(title, Style);
            return ((IRenderable)markup).Render(context.WithSingleLine(), width);
        }

        private (Segment Left, Segment Right) GetLineSegments(RenderContext context, int width, IEnumerable<Segment> title)
        {
            var titleLength = Segment.CellCount(title);

            var border = Border.GetSafeBorder(safe: !context.Unicode);
            var borderPart = border.GetPart(BoxBorderPart.Top);

            var alignment = Alignment ?? Justify.Center;
            if (alignment == Justify.Left)
            {
                var left = new Segment(borderPart.Repeat(TitlePadding) + new string(' ', TitleSpacing), Style ?? Style.Plain);

                var rightLength = width - titleLength - left.CellCount() - TitleSpacing;
                var right = new Segment(new string(' ', TitleSpacing) + borderPart.Repeat(rightLength), Style ?? Style.Plain);

                return (left, right);
            }
            else if (alignment == Justify.Center)
            {
                var leftLength = ((width - titleLength) / 2) - TitleSpacing;
                var left = new Segment(borderPart.Repeat(leftLength) + new string(' ', TitleSpacing), Style ?? Style.Plain);

                var rightLength = width - titleLength - left.CellCount() - TitleSpacing;
                var right = new Segment(new string(' ', TitleSpacing) + borderPart.Repeat(rightLength), Style ?? Style.Plain);

                return (left, right);
            }
            else if (alignment == Justify.Right)
            {
                var right = new Segment(new string(' ', TitleSpacing) + borderPart.Repeat(TitlePadding), Style ?? Style.Plain);

                var leftLength = width - titleLength - right.CellCount() - TitleSpacing;
                var left = new Segment(borderPart.Repeat(leftLength) + new string(' ', TitleSpacing), Style ?? Style.Plain);

                return (left, right);
            }

            throw new NotSupportedException("Unsupported alignment.");
        }
    }
}
