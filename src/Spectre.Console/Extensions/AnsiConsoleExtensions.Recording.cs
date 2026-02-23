namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Creates a recorder for the specified console.
    /// </summary>
    /// <param name="console">The console to record.</param>
    /// <returns>A recorder for the specified console.</returns>
    public static Recorder CreateRecorder(this IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        return new Recorder(console);
    }
}