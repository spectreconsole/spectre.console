namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <param name="console">The console to record.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Creates a recorder for the specified console.
        /// </summary>
        /// <returns>A recorder for the specified console.</returns>
        public Recorder CreateRecorder()
        {
            return new Recorder(console);
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        public void Clear()
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Clear(true);
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void Write(string text)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(new Text(text, Style.Plain));
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
        public void Write(string text, Style? style)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(new Text(text, style));
        }

        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public void WriteLine()
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(Text.NewLine);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void WriteLine(string text)
        {
            WriteLine(console, text, Style.Plain);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
        public void WriteLine(string text, Style? style)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            console.Write(text + Environment.NewLine, style);
        }
    }
}