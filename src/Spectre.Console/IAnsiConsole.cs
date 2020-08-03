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
        /// Gets the buffer width of the console.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the console output encoding.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets or sets the current text decoration.
        /// </summary>
        Decoration Decoration { get; set; }

        /// <summary>
        /// Gets or sets the current foreground.
        /// </summary>
        Color Foreground { get; set; }

        /// <summary>
        /// Gets or sets the current background.
        /// </summary>
        Color Background { get; set; }

        /// <summary>
        /// Writes a string followed by a line terminator to the console.
        /// </summary>
        /// <param name="text">The string to write.</param>
        void Write(string text);
    }
}
