using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable panel.
    /// </summary>
    public sealed class Panel : Renderable, IHasBoxBorder, IExpandable, IPaddable
    {
        private const int EdgeWidth = 2;

        private readonly IRenderable _child;

        /// <inheritdoc/>
        public BoxBorder Border { get; set; } = BoxBorder.Square;

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
        public Padding Padding { get; set; } = new Padding(1, 0, 1, 0);

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public Title? Header { get; set; }

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
            var child = new Padder(_child, Padding);
            var childWidth = ((IRenderable)child).Measure(context, maxWidth);
            return new Measurement(
                childWidth.Min + EdgeWidth,
                childWidth.Max + EdgeWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var border = Border.GetSafeBorder((context.LegacyConsole || !context.Unicode) && UseSafeBorder);
            var borderStyle = BorderStyle ?? Style.Plain;

            var child = new Padder(_child, Padding);
            var childWidth = maxWidth - EdgeWidth;

            if (!Expand)
            {
                var measurement = ((IRenderable)child).Measure(context, maxWidth - EdgeWidth);
                childWidth = measurement.Max;
            }

            var panelWidth = childWidth + EdgeWidth;
            panelWidth = Math.Min(panelWidth, maxWidth);

            var result = new List<Segment>();

            // Panel top
            AddTopBorder(result, context, border, borderStyle, panelWidth);

            // Split the child segments into lines.
            var childSegments = ((IRenderable)child).Render(context, childWidth);
            foreach (var line in Segment.SplitLines(childSegments, childWidth))
            {
                if (line.Count == 1 && line[0].IsWhiteSpace)
                {
                    // NOTE: This check might impact other things.
                    // Hopefully not, but there is a chance.
                    continue;
                }

                result.Add(new Segment(border.GetPart(BoxBorderPart.Left), borderStyle));

                var content = new List<Segment>();
                content.AddRange(line);

                // Do we need to pad the panel?
                var length = line.Sum(segment => segment.CellLength(context));
                if (length < childWidth)
                {
                    var diff = childWidth - length;
                    content.Add(new Segment(new string(' ', diff)));
                }

                result.AddRange(content);

                result.Add(new Segment(border.GetPart(BoxBorderPart.Right), borderStyle));
                result.Add(Segment.LineBreak);
            }

            // Panel bottom
            AddBottomBorder(result, border, borderStyle, panelWidth);

            return result;
        }

        private static void AddBottomBorder(List<Segment> result, BoxBorder border, Style borderStyle, int panelWidth)
        {
            result.Add(new Segment(border.GetPart(BoxBorderPart.BottomLeft), borderStyle));
            result.Add(new Segment(border.GetPart(BoxBorderPart.Bottom).Repeat(panelWidth - EdgeWidth), borderStyle));
            result.Add(new Segment(border.GetPart(BoxBorderPart.BottomRight), borderStyle));
            result.Add(Segment.LineBreak);
        }

        private void AddTopBorder(List<Segment> segments, RenderContext context, BoxBorder border, Style borderStyle, int panelWidth)
        {
            segments.Add(new Segment(border.GetPart(BoxBorderPart.TopLeft), borderStyle));

            if (Header != null)
            {
                var leftSpacing = 0;
                var rightSpacing = 0;

                var headerWidth = panelWidth - (EdgeWidth * 2);
                var header = Segment.TruncateWithEllipsis(Header.Text, Header.Style ?? borderStyle, context, headerWidth);

                var excessWidth = headerWidth - header.CellLength(context);
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

                segments.Add(new Segment(border.GetPart(BoxBorderPart.Top).Repeat(leftSpacing + 1), borderStyle));
                segments.Add(header);
                segments.Add(new Segment(border.GetPart(BoxBorderPart.Top).Repeat(rightSpacing + 1), borderStyle));
            }
            else
            {
                segments.Add(new Segment(border.GetPart(BoxBorderPart.Top).Repeat(panelWidth - EdgeWidth), borderStyle));
            }

            segments.Add(new Segment(border.GetPart(BoxBorderPart.TopRight), borderStyle));
            segments.Add(Segment.LineBreak);
        }
    }
}
