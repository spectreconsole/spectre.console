using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Style"/>.
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// Creates a new style from the specified one with
        /// the specified foreground color.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="color">The foreground color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Style Foreground(this Style style, Color color)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            return new Style(
                foreground: color,
                background: style.Background,
                decoration: style.Decoration);
        }

        /// <summary>
        /// Creates a new style from the specified one with
        /// the specified background color.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="color">The background color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Style Background(this Style style, Color color)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            return new Style(
                foreground: style.Foreground,
                background: color,
                decoration: style.Decoration);
        }

        /// <summary>
        /// Creates a new style from the specified one with
        /// the specified text decoration.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="decoration">The text decoration.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Style Decoration(this Style style, Decoration decoration)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            return new Style(
                foreground: style.Foreground,
                background: style.Background,
                decoration: decoration);
        }

        /// <summary>
        /// Creates a new style from the specified one with
        /// the specified link.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="link">The link.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Style Link(this Style style, string link)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            return new Style(
                foreground: style.Foreground,
                background: style.Background,
                decoration: style.Decoration,
                link: link);
        }

        internal static Style Combine(this Style style, IEnumerable<Style> source)
        {
            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            var current = style;
            foreach (var item in source)
            {
                current = current.Combine(item);
            }

            return current;
        }
    }
}
