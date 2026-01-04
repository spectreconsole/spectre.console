namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
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

            AnsiConsole.Console.Write(renderable);
        }
    }

    /// <param name="console">The console to render to.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Renders the specified object to the console.
        /// </summary>
        /// <param name="renderable">The object to render.</param>
        [Obsolete("Consider using IAnsiConsole.Write instead.")]
        public void Render(IRenderable renderable)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (renderable is null)
            {
                throw new ArgumentNullException(nameof(renderable));
            }

            console.Write(renderable);
        }
    }
}