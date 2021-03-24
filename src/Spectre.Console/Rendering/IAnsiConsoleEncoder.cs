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
        /// Encodes the specified <see cref="IRenderable"/> enumerator.
        /// </summary>
        /// <param name="console">The console to use when encoding.</param>
        /// <param name="renderable">The renderable objects to encode.</param>
        /// <returns>A string representing the encoded result.</returns>
        string Encode(IAnsiConsole console, IEnumerable<IRenderable> renderable);
    }
}
