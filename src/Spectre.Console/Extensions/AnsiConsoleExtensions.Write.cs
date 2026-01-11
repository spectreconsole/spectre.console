namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Writes the specified string value to the console.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="text">The text to write.</param>
    public static void Write(this IAnsiConsole console, string text)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(text);

        console.Write(new Text(text, Style.Plain));
    }

    /// <summary>
    /// Writes the specified string value to the console.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="text">The text to write.</param>
    /// <param name="style">The text style or <see cref="Style.Plain"/> if <see langword="null"/>.</param>
    public static void Write(this IAnsiConsole console, string text, Style? style)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(text);

        console.Write(new Text(text, style));
    }
}