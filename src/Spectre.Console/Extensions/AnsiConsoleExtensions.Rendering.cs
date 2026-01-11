namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <summary>
    /// Renders the specified object to the console.
    /// </summary>
    /// <param name="console">The console to render to.</param>
    /// <param name="renderable">The object to render.</param>
    [Obsolete("Consider using IAnsiConsole.Write instead.")]
    public static void Render(this IAnsiConsole console, IRenderable renderable)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(renderable);

        console.Write(renderable);
    }
}