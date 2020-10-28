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
        /// Gets the console cursor.
        /// </summary>
        IAnsiConsoleCursor Cursor { get; }

        /// <summary>
        /// Gets the buffer width of the console.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="home">If the cursor should be moved to the home position.</param>
        void Clear(bool home);

        /// <summary>
        /// Writes a string followed by a line terminator to the console.
        /// </summary>
        /// <param name="segment">The segment to write.</param>
        void Write(Segment segment);
    }
}
