using System;
using System.Globalization;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
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

            console.Write(Environment.NewLine);
        }

        /// <summary>
        /// Writes the specified string value, followed by the
        /// current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, string value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (value != null)
            {
                console.Write(value);
            }

            console.WriteLine();
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, int value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, int value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, uint value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, uint value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, long value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, long value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, ulong value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, ulong value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, float value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, float value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, double value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, double value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.WriteLine(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, decimal value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, decimal value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            WriteLine(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, bool value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, bool value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            WriteLine(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, char value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, char value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            WriteLine(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, char[] value)
        {
            WriteLine(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, char[] value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            for (var index = 0; index < value.Length; index++)
            {
                console.Write(value[index].ToString(provider));
            }

            console.WriteLine();
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// followed by the current line terminator, to the console
        /// using the specified format information.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void WriteLine(this IAnsiConsole console, string format, params object[] args)
        {
            WriteLine(console, CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// followed by the current line terminator, to the console
        /// using the specified format information.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void WriteLine(this IAnsiConsole console, IFormatProvider provider, string format, params object[] args)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            WriteLine(console, string.Format(provider, format, args));
        }
    }
}
