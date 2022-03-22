namespace Spectre.Console;

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
    /// <para/>
    /// All interpolation holes which contain a string are automatically escaped so you must not call <see cref="StringExtensions.EscapeMarkup"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// string input = args[0];
    /// string output = Process(input);
    /// AnsiConsole.MarkupInterpolated($"[blue]{input}[/] -> [green]{output}[/]");
    /// </code>
    /// </example>
    /// <param name="value">The interpolated string value to write.</param>
    public static void MarkupInterpolated(FormattableString value)
    {
        Console.MarkupInterpolated(value);
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
    /// Writes the specified markup to the console.
    /// <para/>
    /// All interpolation holes which contain a string are automatically escaped so you must not call <see cref="StringExtensions.EscapeMarkup"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// string input = args[0];
    /// string output = Process(input);
    /// AnsiConsole.MarkupInterpolated(CultureInfo.InvariantCulture, $"[blue]{input}[/] -> [green]{output}[/]");
    /// </code>
    /// </example>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="value">The interpolated string value to write.</param>
    public static void MarkupInterpolated(IFormatProvider provider, FormattableString value)
    {
        Console.MarkupInterpolated(provider, value);
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
    /// <para/>
    /// All interpolation holes which contain a string are automatically escaped so you must not call <see cref="StringExtensions.EscapeMarkup"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// string input = args[0];
    /// string output = Process(input);
    /// AnsiConsole.MarkupLineInterpolated($"[blue]{input}[/] -> [green]{output}[/]");
    /// </code>
    /// </example>
    /// <param name="value">The interpolated string value to write.</param>
    public static void MarkupLineInterpolated(FormattableString value)
    {
        Console.MarkupLineInterpolated(value);
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

    /// <summary>
    /// Writes the specified markup, followed by the current line terminator, to the console.
    /// <para/>
    /// All interpolation holes which contain a string are automatically escaped so you must not call <see cref="StringExtensions.EscapeMarkup"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// string input = args[0];
    /// string output = Process(input);
    /// AnsiConsole.MarkupLineInterpolated(CultureInfo.InvariantCulture, $"[blue]{input}[/] -> [green]{output}[/]");
    /// </code>
    /// </example>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="value">The interpolated string value to write.</param>
    public static void MarkupLineInterpolated(IFormatProvider provider, FormattableString value)
    {
        Console.MarkupLineInterpolated(provider, value);
    }
}