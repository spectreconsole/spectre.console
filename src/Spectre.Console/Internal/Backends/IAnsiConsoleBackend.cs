using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a console backend.
    /// </summary>
    internal interface IAnsiConsoleBackend
    {
        /// <summary>
        /// Gets the console cursor for the backend.
        /// </summary>
        IAnsiConsoleCursor Cursor { get; }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="home">If the cursor should be moved to the home position.</param>
        void Clear(bool home);

        /// <summary>
        /// Renders segments to the console.
        /// </summary>
        /// <param name="segments">The segments to render.</param>
        void Render(IEnumerable<Segment> segments);
    }
}
