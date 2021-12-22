namespace Spectre.Console;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Creates a new <see cref="LiveDisplay"/> instance.
    /// </summary>
    /// <param name="target">The target renderable to update.</param>
    /// <returns>A <see cref="LiveDisplay"/> instance.</returns>
    public static LiveDisplay Live(IRenderable target)
    {
        return Console.Live(target);
    }
}