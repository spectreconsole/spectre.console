using System;
using System.IO;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        private static ConsoleColor _defaultForeground;
        private static ConsoleColor _defaultBackground;

        internal static Style CurrentStyle { get; private set; } = Style.Plain;
        internal static bool Created { get; private set; }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public static Color Foreground
        {
            get => CurrentStyle.Foreground;
            set => CurrentStyle = CurrentStyle.WithForeground(value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public static Color Background
        {
            get => CurrentStyle.Background;
            set => CurrentStyle = CurrentStyle.WithBackground(value);
        }

        /// <summary>
        /// Gets or sets the text decoration.
        /// </summary>
        public static Decoration Decoration
        {
            get => CurrentStyle.Decoration;
            set => CurrentStyle = CurrentStyle.WithDecoration(value);
        }

        internal static void Initialize(TextWriter? @out)
        {
            if (@out?.IsStandardOut() ?? false)
            {
                Foreground = _defaultForeground = System.Console.ForegroundColor;
                Background = _defaultBackground = System.Console.BackgroundColor;
            }
            else
            {
                Foreground = _defaultForeground = Color.Silver;
                Background = _defaultBackground = Color.Black;
            }
        }

        /// <summary>
        /// Resets colors and text decorations.
        /// </summary>
        public static void Reset()
        {
            ResetColors();
            ResetDecoration();
        }

        /// <summary>
        /// Resets the current applied text decorations.
        /// </summary>
        public static void ResetDecoration()
        {
            Decoration = Decoration.None;
        }

        /// <summary>
        /// Resets the current applied foreground and background colors.
        /// </summary>
        public static void ResetColors()
        {
            Foreground = _defaultForeground;
            Background = _defaultBackground;
        }
    }
}
