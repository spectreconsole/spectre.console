namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public static Color Foreground
        {
            get => AnsiConsole.CurrentStyle.Foreground;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Foreground(value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public static Color Background
        {
            get => AnsiConsole.CurrentStyle.Background;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Background(value);
        }

        /// <summary>
        /// Gets or sets the text decoration.
        /// </summary>
        public static Decoration Decoration
        {
            get => AnsiConsole.CurrentStyle.Decoration;
            set => AnsiConsole.CurrentStyle = AnsiConsole.CurrentStyle.Decoration(value);
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
            AnsiConsole.Decoration = Decoration.None;
        }

        /// <summary>
        /// Resets the current applied foreground and background colors.
        /// </summary>
        public static void ResetColors()
        {
            AnsiConsole.CurrentStyle = Style.Plain;
        }
    }
}