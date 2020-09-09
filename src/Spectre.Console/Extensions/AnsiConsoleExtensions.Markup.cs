using System;
using System.Globalization;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Markup(this IAnsiConsole console, string format, params object[] args)
        {
            Markup(console, CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Markup(this IAnsiConsole console, IFormatProvider provider, string format, params object[] args)
        {
            console.Render(MarkupParser.Parse(string.Format(provider, format, args)));
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void MarkupLine(this IAnsiConsole console, string format, params object[] args)
        {
            MarkupLine(console, CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void MarkupLine(this IAnsiConsole console, IFormatProvider provider, string format, params object[] args)
        {
            Markup(console, provider, format, args);
            console.WriteLine();
        }
    }
}
