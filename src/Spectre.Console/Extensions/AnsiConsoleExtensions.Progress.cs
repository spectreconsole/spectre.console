namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Creates a new <see cref="Progress"/> instance for the console.
        /// </summary>
        /// <returns>A <see cref="Progress"/> instance.</returns>
        public Progress Progress()
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            return new Progress(console);
        }

        /// <summary>
        /// Creates a new <see cref="Status"/> instance for the console.
        /// </summary>
        /// <returns>A <see cref="Status"/> instance.</returns>
        public Status Status()
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            return new Status(console);
        }
    }
}