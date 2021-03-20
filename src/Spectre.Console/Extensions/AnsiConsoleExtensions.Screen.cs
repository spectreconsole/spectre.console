using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        /// <summary>
        /// Switches to an alternate screen buffer if the terminal supports it.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="action">The action to execute within the alternate screen buffer.</param>
        public static void AlternateScreen(this IAnsiConsole console, Action action)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            using (new Screen(console))
            {
                action();
            }
        }
    }
}
