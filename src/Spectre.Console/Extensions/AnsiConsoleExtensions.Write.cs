namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Write(string value)
        {
            AnsiConsole.Write(value, AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            AnsiConsole.Console.Write(value.ToString(provider), AnsiConsole.CurrentStyle);
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
            ArgumentNullException.ThrowIfNull(value);

            for (var index = 0; index < value.Length; index++)
            {
                AnsiConsole.Console.Write(value[index].ToString(provider), AnsiConsole.CurrentStyle);
            }
        }
    }

    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        public void Write(string text)
        {
            ArgumentNullException.ThrowIfNull(console);

            console.Write(new Text(text, Style.Plain));
        }

        /// <summary>
        /// Writes the specified string value to the console.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="style">The text style or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
        public void Write(string text, Style? style)
        {
            ArgumentNullException.ThrowIfNull(console);

            console.Write(new Text(text, style));
        }
    }
}