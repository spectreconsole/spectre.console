namespace Spectre.Console;

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
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

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
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        Markup(console, string.Format(provider, format, args));
    }

    /// <summary>
    /// Writes the specified markup, followed by the current line terminator, to the console.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void MarkupLine(this IAnsiConsole console, string format, params object[] args)
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

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
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        Markup(console, provider, format + Environment.NewLine, args);
    }

    extension(AnsiConsole)
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void Markup(string value)
        {
            AnsiConsole.Console.Markup(value);
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
            AnsiConsole.Console.MarkupInterpolated(value);
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
            AnsiConsole.Console.MarkupInterpolated(provider, value);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public static void MarkupLine(string value)
        {
            AnsiConsole.Console.MarkupLine(value);
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
            AnsiConsole.Console.MarkupLineInterpolated(value);
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
            AnsiConsole.Console.MarkupLineInterpolated(provider, value);
        }
    }

    /// <param name="console">The console to write to.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Writes the specified markup to the console.
        /// <para/>
        /// All interpolation holes which contain a string are automatically escaped so you must not call <see cref="StringExtensions.EscapeMarkup"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// string input = args[0];
        /// string output = Process(input);
        /// console.MarkupInterpolated($"[blue]{input}[/] -> [green]{output}[/]");
        /// </code>
        /// </example>
        /// <param name="value">The interpolated string value to write.</param>
        public void MarkupInterpolated(FormattableString value)
        {
            MarkupInterpolated(console, CultureInfo.CurrentCulture, value);
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
        /// console.MarkupInterpolated(CultureInfo.InvariantCulture, $"[blue]{input}[/] -> [green]{output}[/]");
        /// </code>
        /// </example>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The interpolated string value to write.</param>
        public void MarkupInterpolated(IFormatProvider provider, FormattableString value)
        {
            Markup(console, Console.Markup.EscapeInterpolated(provider, value));
        }

        /// <summary>
        /// Writes the specified markup to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void Markup(string value)
        {
            console.Write(MarkupParser.Parse(value));
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
        /// console.MarkupLineInterpolated($"[blue]{input}[/] -> [green]{output}[/]");
        /// </code>
        /// </example>
        /// <param name="value">The interpolated string value to write.</param>
        public void MarkupLineInterpolated(FormattableString value)
        {
            MarkupLineInterpolated(console, CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Writes the specified markup, followed by the current line terminator, to the console.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void MarkupLine(string value)
        {
            Markup(console, value + Environment.NewLine);
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
        /// console.MarkupLineInterpolated(CultureInfo.InvariantCulture, $"[blue]{input}[/] -> [green]{output}[/]");
        /// </code>
        /// </example>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="value">The interpolated string value to write.</param>
        public void MarkupLineInterpolated(IFormatProvider provider, FormattableString value)
        {
            MarkupLine(console, Console.Markup.EscapeInterpolated(provider, value));
        }
    }
}