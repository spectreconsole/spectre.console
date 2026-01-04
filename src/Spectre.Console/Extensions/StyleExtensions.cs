namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Style"/>.
/// </summary>
public static class StyleExtensions
{
    /// <param name="style">The style.</param>
    extension(Style style)
    {
        /// <summary>
        /// Creates a new style from the specified one with
        /// the specified foreground color.
        /// </summary>
        /// <param name="color">The foreground color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Style Foreground(Color color)
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
        /// <param name="color">The background color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Style Background(Color color)
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
        /// <param name="decoration">The text decoration.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Style Decoration(Decoration decoration)
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
        /// <param name="link">The link.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Style Link(string link)
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

        internal Style Combine(IEnumerable<Style> source)
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