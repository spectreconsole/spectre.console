using System.IO;
using System.Text;

namespace Spectre.Console
{
    /// <summary>
    /// Represents console output.
    /// </summary>
    public interface IAnsiConsoleOutput
    {
        /// <summary>
        /// Gets the <see cref="TextWriter"/> used to write to the output.
        /// </summary>
        TextWriter Writer { get; }

        /// <summary>
        /// Gets a value indicating whether or not the output is a terminal.
        /// </summary>
        bool IsTerminal { get; }

        /// <summary>
        /// Gets the output width.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the output height.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Sets the output encoding.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        void SetEncoding(Encoding encoding);
    }
}
