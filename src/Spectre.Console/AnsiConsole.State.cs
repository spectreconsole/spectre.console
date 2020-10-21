namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        internal static Style CurrentStyle { get; private set; } = Style.Plain;
        internal static bool Created { get; private set; }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        public static Color Foreground
        {
            get => CurrentStyle.Foreground;
            set => CurrentStyle = CurrentStyle.Foreground(value);
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public static Color Background
        {
            get => CurrentStyle.Background;
            set => CurrentStyle = CurrentStyle.Background(value);
        }

        /// <summary>
        /// Gets or sets the text decoration.
        /// </summary>
        public static Decoration Decoration
        {
            get => CurrentStyle.Decoration;
            set => CurrentStyle = CurrentStyle.Decoration(value);
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
            CurrentStyle = Style.Plain;
        }
    }
}
