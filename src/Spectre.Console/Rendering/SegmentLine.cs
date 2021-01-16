using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a collection of segments.
    /// </summary>
    public sealed class SegmentLine : List<Segment>
    {
        /// <summary>
        /// Gets the width of the line.
        /// </summary>
        public int Length => this.Sum(line => line.Text.Length);

        /// <summary>
        /// Gets the number of cells the segment line occupies.
        /// </summary>
        /// <param name="context">The render context.</param>
        /// <returns>The cell width of the segment line.</returns>
        public int CellCount(RenderContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            return Segment.CellCount(context, this);
        }

        /// <summary>
        /// Preprends a segment to the line.
        /// </summary>
        /// <param name="segment">The segment to prepend.</param>
        public void Prepend(Segment segment)
        {
            if (segment is null)
            {
                throw new System.ArgumentNullException(nameof(segment));
            }

            Insert(0, segment);
        }
    }
}
