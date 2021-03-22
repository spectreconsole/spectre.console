using System;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Markup(string value)
        {
            Console.Markup(value);
        }

        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Markup(string format, params object[] args)
        {
            Console.Markup(format, args);
        }

        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Markup(IFormatProvider provider, string format, params object[] args)
        {
            Console.Markup(provider, format, args);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void MarkupLine(string value)
        {
            Console.MarkupLine(value);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void MarkupLine(string format, params object[] args)
        {
            Console.MarkupLine(format, args);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void MarkupLine(IFormatProvider provider, string format, params object[] args)
        {
            Console.MarkupLine(provider, format, args);
        }
    }
}
