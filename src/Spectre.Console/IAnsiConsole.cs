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
        public AnsiConsoleCapabilities Capabilities { get; }

        /// <summary>
        /// Gets the buffer width of the console.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the buffer height of the console.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets or sets the current style.
        /// </summary>
        Styles Style { get; set; }

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

        /// <summary>
        /// Writes a string followed by a line terminator to the console.
        /// </summary>
        /// <param name="text">
        /// The string to write. If value is null, only the line terminator is written.
        /// </param>
        void WriteLine(string text);
    }
}
