using System;
using System.Globalization;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class ConsoleExtensions
    {
        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, string value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (value != null)
            {
                console.Write(value);
            }
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, int value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, int value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, uint value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 32-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, uint value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, long value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// signed integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, long value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, ulong value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified 64-bit
        /// unsigned integer value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, ulong value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, float value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified single-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, float value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, double value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified double-precision
        /// floating-point value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, double value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Write(value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, decimal value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified decimal value, to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, decimal value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            Write(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, bool value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the text representation of the specified boolean value to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, bool value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            Write(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the specified Unicode character to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, char value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified Unicode character to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, char value)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            Write(console, value.ToString(provider));
        }

        /// <summary>
        /// Writes the specified array of Unicode characters to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, char[] value)
        {
            Write(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified array of Unicode characters to the console.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, char[] value)
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
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// to the console using the specified format information.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Write(this IAnsiConsole console, string format, params object[] args)
        {
            Write(console, CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects,
        /// to the console using the specified format information.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write.</param>
        public static void Write(this IAnsiConsole console, IFormatProvider provider, string format, params object[] args)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            Write(console, string.Format(provider, format, args));
        }
    }
}
