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
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        if (!console.Profile.Capabilities.Ansi)
        {
            throw new NotSupportedException("Alternate buffers are not supported since your terminal does not support ANSI.");
        }

        if (!console.Profile.Capabilities.AlternateBuffer)
        {
            throw new NotSupportedException("Alternate buffers are not supported by your terminal.");
        }

        // Switch to alternate screen
        console.Write(new ControlCode("\u001b[?1049h\u001b[H"));

        try
        {
            // Execute custom action
            action();
        }
        finally
        {
            // Switch back to primary screen
            console.Write(new ControlCode("\u001b[?1049l"));
        }
    }

    /// <summary>
    /// Switches to an alternate screen buffer asynchronously if the terminal supports it.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="action">The action to execute within the alternate screen buffer.</param>
    /// <returns>The result of the function.</returns>
    public static async Task AlternateScreenAsync(this IAnsiConsole console, Func<Task> action)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        if (!console.Profile.Capabilities.Ansi)
        {
            throw new NotSupportedException("Alternate buffers are not supported since your terminal does not support ANSI.");
        }

        if (!console.Profile.Capabilities.AlternateBuffer)
        {
            throw new NotSupportedException("Alternate buffers are not supported by your terminal.");
        }

        // Switch to alternate screen
        console.Write(new ControlCode("\u001b[?1049h\u001b[H"));

        try
        {
            // Execute custom action
            await action();
        }
        finally
        {
            // Switch back to primary screen
            console.Write(new ControlCode("\u001b[?1049l"));
        }
    }
}