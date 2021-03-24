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
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(string value)
        {
            Write(value, CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(int value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, int value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(uint value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, uint value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(long value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, long value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(ulong value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, ulong value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(float value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, float value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(double value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, double value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(decimal value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value, to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, decimal value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(bool value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, bool value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the specified Unicode character to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(char value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified Unicode character to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, char value)
        {
            Console.Write(value.ToString(provider), CurrentStyle);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(char[] value)
        {
            Write(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters to the console.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(IFormatProvider provider, char[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            for (var index = 0; index < value.Length; index++)
            {
                Console.Write(value[index].ToString(provider), CurrentStyle);
            }
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// to the console using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Write(string format, params object[] args)
        {
            Write(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// to the console using the specified format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Write(IFormatProvider provider, string format, params object[] args)
        {
            Console.Write(string.Format(provider, format, args), CurrentStyle);
        }
    }
}
