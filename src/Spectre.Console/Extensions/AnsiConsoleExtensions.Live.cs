namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Creates a new <see cref="LiveDisplay"/> instance for the console.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="target">The target renderable to update.</param>
    /// <returns>A <see cref="LiveDisplay"/> instance.</returns>
    public static LiveDisplay Live(this IAnsiConsole console, IRenderable target)
    {
        if (console is null)
        {
            throw new ArgumentNullException(nameof(console));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        return new LiveDisplay(console, target);
    }
}