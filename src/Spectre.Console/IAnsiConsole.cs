using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a console.
    /// </summary>
    public interface IAnsiConsole
    {
        /// <summary>
        /// Gets the console's capabilities.
        /// </summary>
        Capabilities Capabilities { get; }

        /// <summary>
        /// Gets the console output encoding.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets the buffer width of the console.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Writes a string followed by a line terminator to the console.
        /// </summary>
        /// <param name="segment">The segment to write.</param>
        void Write(Segment segment);
    }
}
