using System.Text;

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
        /// <param name="text">The string to write.</param>
        /// <param name="style">The style to use.</param>
        void Write(string text, Style style);
    }
}
