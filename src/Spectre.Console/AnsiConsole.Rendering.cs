using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        /// <summary>
        /// Renders the specified object to the console.
        /// </summary>
        /// <param name="renderable">The object to render.</param>
        public static void Render(IRenderable renderable)
        {
            Console.Render(renderable);
        }
    }
}
