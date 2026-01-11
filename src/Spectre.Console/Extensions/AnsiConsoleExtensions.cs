namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Clears the console.
    /// </summary>
    /// <param name="console">The console to clear.</param>
    public static void Clear(this IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        console.Clear(true);
    }
}