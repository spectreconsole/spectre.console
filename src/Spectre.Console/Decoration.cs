using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents text decoration.
    /// </summary>
    /// <remarks>
    /// Support for text decorations is up to the terminal.
    /// </remarks>
    [Flags]
    public enum Decoration
    {
        /// <summary>
        /// No text decoration.
        /// </summary>
        None = 0,

        /// <summary>
        /// Bold text.
        /// Not supported in every environment.
        /// </summary>
        Bold = 1 << 0,

        /// <summary>
        /// Dim or faint text.
        /// Not supported in every environment.
        /// </summary>
        Dim = 1 << 1,

        /// <summary>
        /// Italic text.
        /// Not supported in every environment.
        /// </summary>
        Italic = 1 << 2,

        /// <summary>
        /// Underlined text.
        /// Not supported in every environment.
        /// </summary>
        Underline = 1 << 3,

        /// <summary>
        /// Swaps the foreground and background colors.
        /// Not supported in every environment.
        /// </summary>
        Invert = 1 << 4,

        /// <summary>
        /// Hides the text.
        /// Not supported in every environment.
        /// </summary>
        Conceal = 1 << 5,

        /// <summary>
        /// Makes text blink.
        /// Normally less than 150 blinks per minute.
        /// Not supported in every environment.
        /// </summary>
        SlowBlink = 1 << 6,

        /// <summary>
        /// Makes text blink.
        /// Normally more than 150 blinks per minute.
        /// Not supported in every environment.
        /// </summary>
        RapidBlink = 1 << 7,

        /// <summary>
        /// Shows text with a horizontal line through the center.
        /// Not supported in every environment.
        /// </summary>
        Strikethrough = 1 << 8,
    }
}
