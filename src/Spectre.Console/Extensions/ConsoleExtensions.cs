using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class ConsoleExtensions
    {
        /// <summary>
        /// Resets colors and text decorations.
        /// </summary>
        /// <param name="console">The console to reset.</param>
        public static void Reset(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.ResetColors();
            console.ResetDecoration();
        }

        /// <summary>
        /// Resets the current applied text decorations.
        /// </summary>
        /// <param name="console">The console to reset the text decorations for.</param>
        public static void ResetDecoration(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Decoration = Decoration.None;
        }

        /// <summary>
        /// Resets the current applied foreground and background colors.
        /// </summary>
        /// <param name="console">The console to reset colors for.</param>
        public static void ResetColors(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Foreground = Color.Default;
            console.Background = Color.Default;
        }
    }
}
