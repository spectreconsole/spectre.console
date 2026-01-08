namespace Spectre.Console;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Switches to an alternate screen buffer if the terminal supports it.
    /// </summary>
    /// <param name="action">The action to execute within the alternate screen buffer.</param>
    public static void AlternateScreen(Action action)
    {
        Console.AlternateScreen(action);
    }
}