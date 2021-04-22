using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Creates a recorder for the specified console.
        /// </summary>
        /// <param name="console">The console to record.</param>
        /// <returns>A recorder for the specified console.</returns>
        public static Recorder CreateRecorder(this IAnsiConsole console)
        {
            return new Recorder(console);
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        /// <param name="console">The console to clear.</param>
        public static void Clear(this IAnsiConsole console)
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
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void Write(this IAnsiConsole console, string text)
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
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style.</param>
        public static void Write(this IAnsiConsole console, string text, Style style)
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
        /// <param name="console">The console to write to.</param>
        public static void WriteLine(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(new Text(Environment.NewLine, Style.Plain));
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void WriteLine(this IAnsiConsole console, string text)
        {
            WriteLine(console, text, Style.Plain);
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style.</param>
        public static void WriteLine(this IAnsiConsole console, string text, Style style)
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
