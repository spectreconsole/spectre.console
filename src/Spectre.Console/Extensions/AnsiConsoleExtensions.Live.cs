namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Creates a new <see cref="LiveDisplay"/> instance.
        /// </summary>
        /// <param name="target">The target renderable to update.</param>
        /// <returns>A <see cref="LiveDisplay"/> instance.</returns>
        public static LiveDisplay Live(IRenderable target)
        {
            return AnsiConsole.Console.Live(target);
        }
    }

    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Creates a new <see cref="LiveDisplay"/> instance for the console.
        /// </summary>
        /// <param name="target">The target renderable to update.</param>
        /// <returns>A <see cref="LiveDisplay"/> instance.</returns>
        public LiveDisplay Live(IRenderable target)
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
}