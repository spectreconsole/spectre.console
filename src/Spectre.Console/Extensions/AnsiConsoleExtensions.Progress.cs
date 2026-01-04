namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Creates a new <see cref="Spectre.Console.Progress"/> instance.
        /// </summary>
        /// <returns>A <see cref="Spectre.Console.Progress"/> instance.</returns>
        public static Progress Progress()
        {
            return AnsiConsole.Console.Progress();
        }

        /// <summary>
        /// Creates a new <see cref="Spectre.Console.Status"/> instance.
        /// </summary>
        /// <returns>A <see cref="Spectre.Console.Status"/> instance.</returns>
        public static Status Status()
        {
            return AnsiConsole.Console.Status();
        }
    }

    /// <param name="console">The console.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Creates a new <see cref="Spectre.Console.Progress"/> instance for the console.
        /// </summary>
        /// <returns>A <see cref="Spectre.Console.Progress"/> instance.</returns>
        public Progress Progress()
        {
            ArgumentNullException.ThrowIfNull(console);

            return new Progress(console);
        }

        /// <summary>
        /// Creates a new <see cref="Spectre.Console.Status"/> instance for the console.
        /// </summary>
        /// <returns>A <see cref="Spectre.Console.Status"/> instance.</returns>
        public Status Status()
        {
            ArgumentNullException.ThrowIfNull(console);

            return new Status(console);
        }
    }
}