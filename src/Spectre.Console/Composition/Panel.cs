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
        private readonly bool _fit;
        private readonly Justify _content;
        private readonly BorderKind _border;

        /// <summary>
        /// Gets or sets a value indicating whether or not to use
        /// a "safe" border on legacy consoles that might not be able
        /// to render non-ASCII characters. Defaults to <c>true</c>.
        /// </summary>
        public bool SafeBorder { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="fit">Whether or not to fit the panel to it's parent.</param>
        /// <param name="content">The justification of the panel content.</param>
        /// <param name="border">The border to use.</param>
        public Panel(
            IRenderable child,
            bool fit = false,
            Justify content = Justify.Left,
            BorderKind border = BorderKind.Square)
        {
            _child = child ?? throw new System.ArgumentNullException(nameof(child));
            _fit = fit;
            _content = content;
            _border = border;
        }

        /// <inheritdoc/>
        Measurement IRenderable.Measure(RenderContext context, int maxWidth)
        {
            var childWidth = _child.Measure(context, maxWidth);
            return new Measurement(childWidth.Min + 4, childWidth.Max + 4);
        }

        /// <inheritdoc/>
        IEnumerable<Segment> IRenderable.Render(RenderContext context, int width)
        {
            var border = Border.GetBorder(_border, (context.LegacyConsole || !context.Unicode) && SafeBorder);

            var childWidth = width - 4;
            if (!_fit)
            {
                var measurement = _child.Measure(context, width - 2);
                childWidth = measurement.Max;
            }

            var result = new List<Segment>();
            var panelWidth = childWidth + 2;

            result.Add(new Segment(border.GetPart(BorderPart.HeaderTopLeft)));
            result.Add(new Segment(border.GetPart(BorderPart.HeaderTop, panelWidth)));
            result.Add(new Segment(border.GetPart(BorderPart.HeaderTopRight)));
            result.Add(new Segment("\n"));

            // Render the child.
            var childSegments = _child.Render(context, childWidth);

            // Split the child segments into lines.
            var lines = Segment.SplitLines(childSegments, childWidth);
            foreach (var line in lines)
            {
                result.Add(new Segment(border.GetPart(BorderPart.CellLeft)));
                result.Add(new Segment(" ")); // Left padding

                var content = new List<Segment>();

                var length = line.Sum(segment => segment.CellLength(context.Encoding));
                if (length < childWidth)
                {
                    // Justify right side
                    if (_content == Justify.Right)
                    {
                        var diff = childWidth - length;
                        content.Add(new Segment(new string(' ', diff)));
                    }
                    else if (_content == Justify.Center)
                    {
                        var diff = (childWidth - length) / 2;
                        content.Add(new Segment(new string(' ', diff)));
                    }
                }

                foreach (var segment in line)
                {
                    content.Add(segment.StripLineEndings());
                }

                // Justify left side
                if (length < childWidth)
                {
                    if (_content == Justify.Left)
                    {
                        var diff = childWidth - length;
                        content.Add(new Segment(new string(' ', diff)));
                    }
                    else if (_content == Justify.Center)
                    {
                        var diff = (childWidth - length) / 2;
                        content.Add(new Segment(new string(' ', diff)));

                        var remainder = (childWidth - length) % 2;
                        if (remainder != 0)
                        {
                            content.Add(new Segment(new string(' ', remainder)));
                        }
                    }
                }

                result.AddRange(content);

                result.Add(new Segment(" "));
                result.Add(new Segment(border.GetPart(BorderPart.CellRight)));
                result.Add(new Segment("\n"));
            }

            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomLeft)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, panelWidth)));
            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomRight)));
            result.Add(new Segment("\n"));

            return result;
        }
    }
}
