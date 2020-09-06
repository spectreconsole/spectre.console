using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable panel.
    /// </summary>
    public sealed class Panel : Renderable, IHasBorder, IExpandable, IPaddable
    {
        private const int EdgeWidth = 2;

        private readonly IRenderable _child;

        /// <inheritdoc/>
        public Border Border { get; set; } = Border.Square;

        /// <inheritdoc/>
        public bool UseSafeBorder { get; set; } = true;

        /// <inheritdoc/>
        public Style? BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the panel should
        /// fit the available space. If <c>false</c>, the panel width will be
        /// auto calculated. Defaults to <c>false</c>.
        /// </summary>
        public bool Expand { get; set; }

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        public Padding Padding { get; set; } = new Padding(1, 1);

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public Header? Header { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="text">The panel content.</param>
        public Panel(string text)
            : this(new Markup(text))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="content">The panel content.</param>
        public Panel(IRenderable content)
        {
            _child = content ?? throw new System.ArgumentNullException(nameof(content));
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            var childWidth = _child.Measure(context, maxWidth);
            return new Measurement(
                childWidth.Min + EdgeWidth + Padding.GetHorizontalPadding(),
                childWidth.Max + EdgeWidth + Padding.GetHorizontalPadding());
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var border = Border.GetSafeBorder((context.LegacyConsole || !context.Unicode) && UseSafeBorder);
            var borderStyle = BorderStyle ?? Style.Plain;

            var paddingWidth = Padding.GetHorizontalPadding();
            var childWidth = maxWidth - EdgeWidth - paddingWidth;

            if (!Expand)
            {
                var measurement = _child.Measure(context, maxWidth - EdgeWidth - paddingWidth);
                childWidth = measurement.Max;
            }

            var panelWidth = childWidth + EdgeWidth + paddingWidth;
            panelWidth = Math.Min(panelWidth, maxWidth);

            var result = new List<Segment>();

            // Panel top
            AddTopBorder(result, context, border, borderStyle, panelWidth);

            // Split the child segments into lines.
            var childSegments = _child.Render(context, childWidth);
            foreach (var line in Segment.SplitLines(childSegments, panelWidth))
            {
                result.Add(new Segment(border.GetPart(BorderPart.CellLeft), borderStyle));

                // Left padding
                if (Padding.Left > 0)
                {
                    result.Add(new Segment(new string(' ', Padding.Left)));
                }

                var content = new List<Segment>();
                content.AddRange(line);

                // Do we need to pad the panel?
                var length = line.Sum(segment => segment.CellLength(context.Encoding));
                if (length < childWidth)
                {
                    var diff = childWidth - length;
                    content.Add(new Segment(new string(' ', diff)));
                }

                result.AddRange(content);

                // Right padding
                if (Padding.Right > 0)
                {
                    result.Add(new Segment(new string(' ', Padding.Right)));
                }

                result.Add(new Segment(border.GetPart(BorderPart.CellRight), borderStyle));
                result.Add(Segment.LineBreak);
            }

            // Panel bottom
            AddBottomBorder(result, border, borderStyle, panelWidth);

            return result;
        }

        private static void AddBottomBorder(List<Segment> result, Border border, Style borderStyle, int panelWidth)
        {
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomLeft), borderStyle));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, panelWidth - EdgeWidth), borderStyle));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomRight), borderStyle));
            result.Add(Segment.LineBreak);
        }

        private void AddTopBorder(List<Segment> segments, RenderContext context, Border border, Style borderStyle, int panelWidth)
        {
            segments.Add(new Segment(border.GetPart(BorderPart.HeaderTopLeft), borderStyle));

            if (Header != null)
            {
                var leftSpacing = 0;
                var rightSpacing = 0;

                var headerWidth = panelWidth - (EdgeWidth * 2);
                var header = Segment.TruncateWithEllipsis(Header.Text, Header.Style ?? borderStyle, context.Encoding, headerWidth);

                var excessWidth = headerWidth - header.CellLength(context.Encoding);
                if (excessWidth > 0)
                {
                    switch (Header.Alignment ?? Justify.Left)
                    {
                        case Justify.Left:
                            leftSpacing = 0;
                            rightSpacing = excessWidth;
                            break;
                        case Justify.Right:
                            leftSpacing = excessWidth;
                            rightSpacing = 0;
                            break;
                        case Justify.Center:
                            leftSpacing = excessWidth / 2;
                            rightSpacing = (excessWidth / 2) + (excessWidth % 2);
                            break;
                    }
                }

                segments.Add(new Segment(border.GetPart(BorderPart.HeaderTop, leftSpacing + 1), borderStyle));
                segments.Add(header);
                segments.Add(new Segment(border.GetPart(BorderPart.HeaderTop, rightSpacing + 1), borderStyle));
            }
            else
            {
                segments.Add(new Segment(border.GetPart(BorderPart.HeaderTop, panelWidth - EdgeWidth), borderStyle));
            }

            segments.Add(new Segment(border.GetPart(BorderPart.HeaderTopRight), borderStyle));
            segments.Add(Segment.LineBreak);
        }
    }
}
