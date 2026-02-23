namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Creates a new <see cref="Progress"/> instance for the console.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>A <see cref="Progress"/> instance.</returns>
    public static Progress Progress(this IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        return new Progress(console);
    }

    /// <summary>
    /// Creates a new <see cref="Status"/> instance for the console.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <returns>A <see cref="Status"/> instance.</returns>
    public static Status Status(this IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        return new Status(console);
    }
}