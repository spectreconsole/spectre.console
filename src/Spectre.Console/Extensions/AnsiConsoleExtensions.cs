namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    /// <param name="console">The console to record.</param>
    extension(IAnsiConsole console)
    {
        /// <summary>
        /// Clears the console.
        /// </summary>
        public void Clear()
        {
            ArgumentNullException.ThrowIfNull(console);

            console.Clear(true);
        }
    }
}