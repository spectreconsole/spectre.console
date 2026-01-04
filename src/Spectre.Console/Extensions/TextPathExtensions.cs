namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="TextPath"/>.
/// </summary>
public static class TextPathExtensions
{
    /// <param name="obj">The path.</param>
    extension(TextPath obj)
    {
        /// <summary>
        /// Sets the separator style.
        /// </summary>
        /// <param name="style">The separator style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath SeparatorStyle(Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SeparatorStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the separator color.
        /// </summary>
        /// <param name="color">The separator color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath SeparatorColor(Color color)
        {
            return SeparatorStyle(obj, new Style(foreground: color));
        }

        /// <summary>
        /// Sets the root style.
        /// </summary>
        /// <param name="style">The root style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath RootStyle(Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.RootStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the root color.
        /// </summary>
        /// <param name="color">The root color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath RootColor(Color color)
        {
            return RootStyle(obj, new Style(foreground: color));
        }

        /// <summary>
        /// Sets the stem style.
        /// </summary>
        /// <param name="style">The stem style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath StemStyle(Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.StemStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the stem color.
        /// </summary>
        /// <param name="color">The stem color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath StemColor(Color color)
        {
            return StemStyle(obj, new Style(foreground: color));
        }

        /// <summary>
        /// Sets the leaf style.
        /// </summary>
        /// <param name="style">The stem leaf to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath LeafStyle(Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.LeafStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the leaf color.
        /// </summary>
        /// <param name="color">The leaf color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPath LeafColor(Color color)
        {
            return LeafStyle(obj, new Style(foreground: color));
        }
    }
}