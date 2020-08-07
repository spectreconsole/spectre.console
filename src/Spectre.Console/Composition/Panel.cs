using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Composition;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a panel which contains another renderable item.
    /// </summary>
    public sealed class Panel : IRenderable
    {
        private readonly IRenderable _child;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use
        /// a "safe" border on legacy consoles that might not be able
        /// to render non-ASCII characters. Defaults to <c>true</c>.
        /// </summary>
        public bool SafeBorder { get; set; } = true;

        /// <summary>
        /// Gets or sets the kind of border to use.
        /// </summary>
        public BorderKind Border { get; set; } = BorderKind.Square;

        /// <summary>
        /// Gets or sets the alignment of the panel contents.
        /// </summary>
        public Justify? Alignment { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether or not the panel should
        /// fit the available space. If <c>false</c>, the panel width will be
        /// auto calculated. Defaults to <c>false</c>.
        /// </summary>
        public bool Expand { get; set; } = false;

        /// <summary>
        /// Gets or sets the padding.
        /// </summary>
        public Padding Padding { get; set; } = new Padding(1, 1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="content">The panel content.</param>
        public Panel(IRenderable content)
        {
            _child = content ?? throw new System.ArgumentNullException(nameof(content));
        }

        /// <inheritdoc/>
        Measurement IRenderable.Measure(RenderContext context, int maxWidth)
        {
            var childWidth = _child.Measure(context, maxWidth);
            return new Measurement(childWidth.Min + 2 + Padding.GetHorizontalPadding(), childWidth.Max + 2 + Padding.GetHorizontalPadding());
        }

        /// <inheritdoc/>
        IEnumerable<Segment> IRenderable.Render(RenderContext context, int width)
        {
            var border = Composition.Border.GetBorder(Border, (context.LegacyConsole || !context.Unicode) && SafeBorder);

            var edgeWidth = 2;
            var paddingWidth = Padding.GetHorizontalPadding();
            var childWidth = width - edgeWidth - paddingWidth;

            if (!Expand)
            {
                var measurement = _child.Measure(context, width - edgeWidth - paddingWidth);
                childWidth = measurement.Max;
            }

            var panelWidth = childWidth + paddingWidth;

            // Panel top
            var result = new List<Segment>
            {
                new Segment(border.GetPart(BorderPart.HeaderTopLeft)),
                new Segment(border.GetPart(BorderPart.HeaderTop, panelWidth)),
                new Segment(border.GetPart(BorderPart.HeaderTopRight)),
                new Segment("\n"),
            };

            // Render the child.
            var childContext = context.WithJustification(Alignment);
            var childSegments = _child.Render(childContext, childWidth);

            // Split the child segments into lines.
            foreach (var line in Segment.SplitLines(childSegments, panelWidth))
            {
                result.Add(new Segment(border.GetPart(BorderPart.CellLeft)));

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

                result.Add(new Segment(border.GetPart(BorderPart.CellRight)));
                result.Add(new Segment("\n"));
            }

            // Panel bottom
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomLeft)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, panelWidth)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomRight)));
            result.Add(new Segment("\n"));

            return result;
        }
    }
}
