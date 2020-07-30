using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents a line of segments.
    /// </summary>
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix")]
    public sealed class SegmentLine : List<Segment>
    {
        /// <summary>
        /// Gets the length of the line.
        /// </summary>
        public int Length => this.Sum(line => line.Text.Length);
    }
}
