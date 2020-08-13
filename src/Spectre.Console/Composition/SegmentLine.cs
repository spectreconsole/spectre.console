using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents a collection of segments.
    /// </summary>
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
    public sealed class SegmentLine : List<Segment>
    {
        /// <summary>
        /// Gets the width of the line.
        /// </summary>
        public int Width => this.Sum(line => line.Text.Length);

        /// <summary>
        /// Gets the cell width of the segment line.
        /// </summary>
        /// <param name="encoding">The encoding to use.</param>
        /// <returns>The cell width of the segment line.</returns>
        public int CellWidth(Encoding encoding)
        {
            return this.Sum(line => line.CellLength(encoding));
        }

        /// <summary>
        /// Preprends a segment to the line.
        /// </summary>
        /// <param name="segment">The segment to prepend.</param>
        public void Prepend(Segment segment)
        {
            Insert(0, segment);
        }
    }
}
