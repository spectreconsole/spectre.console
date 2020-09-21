using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a console encoder that can encode
    /// recorded segments into a string.
    /// </summary>
    public interface IAnsiConsoleEncoder
    {
        /// <summary>
        /// Encodes the specified segments.
        /// </summary>
        /// <param name="segments">The segments to encode.</param>
        /// <returns>The encoded string.</returns>
        string Encode(IEnumerable<Segment> segments);
    }
}
