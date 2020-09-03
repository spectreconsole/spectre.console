using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents color and text decoration.
    /// </summary>
    public sealed partial class Style : IEquatable<Style>
    {
        /// <summary>
        /// Creates a new style from the specified foreground color.
        /// </summary>
        /// <param name="color">The foreground color.</param>
        /// <returns>A new <see cref="Style"/> with the specified foreground color.</returns>
        public static Style WithForeground(Color color)
        {
            return new Style(foreground: color);
        }

        /// <summary>
        /// Creates a new style from the specified background color.
        /// </summary>
        /// <param name="color">The background color.</param>
        /// <returns>A new <see cref="Style"/> with the specified background color.</returns>
        public static Style WithBackground(Color color)
        {
            return new Style(background: color);
        }

        /// <summary>
        /// Creates a new style from the specified text decoration.
        /// </summary>
        /// <param name="decoration">The text decoration.</param>
        /// <returns>A new <see cref="Style"/> with the specified text decoration.</returns>
        public static Style WithDecoration(Decoration decoration)
        {
            return new Style(decoration: decoration);
        }
    }
}
