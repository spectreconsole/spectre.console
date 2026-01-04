namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static Color Foreground
        {
            get => AnsiConsole.CurrentStyle.Foreground;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Foreground(value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static Color Background
        {
            get => AnsiConsole.CurrentStyle.Background;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Background(value);
        }

        /// <summary>
        /// Gets or sets the text decoration.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static Decoration Decoration
        {
            get => AnsiConsole.CurrentStyle.Decoration;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Decoration(value);
        }

        /// <summary>
        /// Resets colors and text decorations.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static void Reset()
        {
            ResetColors();
            ResetDecoration();
        }

        /// <summary>
        /// Resets the current applied text decorations.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static void ResetDecoration()
        {
            AnsiConsole.Decoration = Decoration.None;
        }

        /// <summary>
        /// Resets the current applied foreground and background colors.
        /// </summary>
        [Obsolete("Static global state has been obsolete and will be removed in a future version")]
        public static void ResetColors()
        {
            AnsiConsole.CurrentStyle = Style.Plain;
        }
    }
}