using System;
using System.Globalization;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        /// <summary>
        /// Writes an empty line to the console.
        /// </summary>
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(string value)
        {
            Console.WriteLine(value, CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(int value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, int value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(uint value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, uint value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(long value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, long value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(ulong value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit unsigned integer value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, ulong value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(float value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, float value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(double value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision floating-point
        /// value, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, double value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(decimal value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, decimal value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(bool value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value,
        /// followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, bool value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(char value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified Unicode character, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, char value)
        {
            Console.WriteLine(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(char[] value)
        {
            WriteLine(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters, followed by the current
        /// line terminator, value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(IFormatProvider provider, char[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            for (var index = 0; index < value.Length; index++)
            {
                Console.Write(value[index].ToString(provider), CurrentStyle);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// followed by the current line terminator, to the console
        /// using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// followed by the current line terminator, to the console
        /// using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void WriteLine(IFormatProvider provider, string format, params object[] args)
        {
            Console.WriteLine(string.Format(provider, format, args), CurrentStyle);
        }
    }
}
