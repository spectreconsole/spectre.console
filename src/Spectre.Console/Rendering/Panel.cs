using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;
using SpectreBorder = Spectre.Console.Rendering.Border;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable panel.
    /// </summary>
    public sealed class Panel : Renderable
    {
        private const int EdgeWidth = 2;

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
        public Justify? Alignment { get; set; }

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
                childWidth.Min + 2 + Padding.GetHorizontalPadding(),
                childWidth.Max + 2 + Padding.GetHorizontalPadding());
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            var border = SpectreBorder.GetBorder(Border, (context.LegacyConsole || !context.Unicode) && SafeBorder);

            var paddingWidth = Padding.GetHorizontalPadding();
            var childWidth = maxWidth - EdgeWidth - paddingWidth;

            if (!Expand)
            {
                var measurement = _child.Measure(context, maxWidth - EdgeWidth - paddingWidth);
                childWidth = measurement.Max;
            }

            var panelWidth = childWidth + paddingWidth;

            // Panel top
            var result = new List<Segment>
            {
                new Segment(border.GetPart(BorderPart.HeaderTopLeft)),
                new Segment(border.GetPart(BorderPart.HeaderTop, panelWidth)),
                new Segment(border.GetPart(BorderPart.HeaderTopRight)),
                Segment.LineBreak,
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
                result.Add(Segment.LineBreak);
            }

            // Panel bottom
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomLeft)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, panelWidth)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomRight)));
            result.Add(Segment.LineBreak);

            return result;
        }
    }
}
