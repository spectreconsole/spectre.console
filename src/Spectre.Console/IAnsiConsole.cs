using System.Collections.Generic;
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
        /// Gets the console input.
        /// </summary>
        IAnsiConsoleInput Input { get; }

        /// <summary>
        /// Gets the render pipeline.
        /// </summary>
        RenderPipeline Pipeline { get; }

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
        /// Writes multiple segments to the console.
        /// </summary>
        /// <param name="segments">The segments to write.</param>
        void Write(IEnumerable<Segment> segments);
    }
}
