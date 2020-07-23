using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class ConsoleExtensions
    {
        /// <summary>
        /// Resets both colors and style for the console.
        /// </summary>
        /// <param name="console">The console to reset.</param>
        public static void Reset(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.ResetColors();
            console.ResetStyle();
        }

        /// <summary>
        /// Resets the current style back to the default one.
        /// </summary>
        /// <param name="console">The console to reset the style for.</param>
        public static void ResetStyle(this IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            console.Style = Styles.None;
        }

        /// <summary>
        /// Resets the foreground and background colors to the default ones.
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
