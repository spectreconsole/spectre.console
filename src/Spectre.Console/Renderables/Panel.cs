using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <param name="fit">Whether or not to fit the panel to it's parent.</param>
        public Panel(IRenderable child, bool fit = false)
        {
            _child = child;
            _fit = fit;
        }

        /// <inheritdoc/>
        public int Measure(Encoding encoding, int maxWidth)
        {
            var childWidth = _child.Measure(encoding, maxWidth);
            return childWidth + 4;
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(Encoding encoding, int width)
        {
            var childWidth = width - 4;
            if (!_fit)
            {
                childWidth = _child.Measure(encoding, width - 2);
            }

            var result = new List<Segment>();
            var panelWidth = childWidth + 2;

            result.Add(new Segment("┌"));
            result.Add(new Segment(new string('─', panelWidth)));
            result.Add(new Segment("┐"));
            result.Add(new Segment("\n"));

            var childSegments = _child.Render(encoding, childWidth);
            foreach (var line in Segment.Split(childSegments))
            {
                result.Add(new Segment("│ "));

                foreach (var segment in line)
                {
                    result.Add(segment.StripLineEndings());
                }

                var length = line.Sum(segment => segment.CellLength(encoding));
                if (length < childWidth)
                {
                    var diff = childWidth - length;
                    result.Add(new Segment(new string(' ', diff)));
                }

                result.Add(new Segment(" │"));
                result.Add(new Segment("\n"));
            }

            result.Add(new Segment("└"));
            result.Add(new Segment(new string('─', panelWidth)));
            result.Add(new Segment("┘"));
            result.Add(new Segment("\n"));

            return result;
        }
    }
}
