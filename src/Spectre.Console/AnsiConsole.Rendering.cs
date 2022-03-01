namespace Spectre.Console;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
    /// <summary>
    /// Renders the specified object to the console.
    /// </summary>
    /// <param name="renderable">The object to render.</param>
    [Obsolete("Consider using AnsiConsole.Write instead.")]
    public static void Render(IRenderable renderable)
    {
        Write(renderable);
    }

    /// <summary>
    /// Renders the specified <see cref="IRenderable"/> to the console.
    /// </summary>
    /// <param name="renderable">The object to render.</param>
    public static void Write(IRenderable renderable)
    {
        if (renderable is null)
        {
            throw new ArgumentNullException(nameof(renderable));
        }

        Console.Write(renderable);
    }
}