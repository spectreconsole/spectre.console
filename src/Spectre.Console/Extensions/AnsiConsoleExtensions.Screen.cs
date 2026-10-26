namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Switches to an alternate screen buffer if the terminal supports it.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="action">The action to execute within the alternate screen buffer.</param>
    public static void AlternateScreen(this IAnsiConsole console, Action action)
    {
        ArgumentNullException.ThrowIfNull(console);

        if (!console.Profile.Capabilities.Ansi)
        {
            throw new NotSupportedException("Alternate buffers are not supported since your terminal does not support ANSI.");
        }

        if (!console.Profile.Capabilities.AlternateBuffer)
        {
            throw new NotSupportedException("Alternate buffers are not supported by your terminal.");
        }

        // Switch to alternate screen
        console.WriteAnsi(w =>
        {
            w.EnterAltScreen();
            w.CursorHome();
        });

        try
        {
            // Execute custom action
            action();
        }
        finally
        {
            // Switch back to primary screen
            console.WriteAnsi(w => w.ExitAltScreen());
        }
    }

    /// <summary>
    /// Switches to an alternate screen buffer if the terminal supports it.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="action">The action to execute within the alternate screen buffer.</param>
    /// <returns>A task that completes when the alternate screen has been closed.</returns>
    public static async Task AlternateScreenAsync(this IAnsiConsole console, Func<Task> action)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(action);

        if (!console.Profile.Capabilities.Ansi)
        {
            throw new NotSupportedException("Alternate buffers are not supported since your terminal does not support ANSI.");
        }

        if (!console.Profile.Capabilities.AlternateBuffer)
        {
            throw new NotSupportedException("Alternate buffers are not supported by your terminal.");
        }

        // Switch to alternate screen
        console.WriteAnsi(w =>
        {
            w.EnterAltScreen();
            w.CursorHome();
        });

        try
        {
            // Execute custom action
            await action();
        }
        finally
        {
            // Switch back to primary screen
            console.WriteAnsi(w => w.ExitAltScreen());
        }
    }
}