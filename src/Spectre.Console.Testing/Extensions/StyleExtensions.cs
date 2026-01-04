namespace Spectre.Console.Testing;

/// <summary>
/// Contains extensions for <see cref="Style"/>.
/// </summary>
public static class StyleExtensions
{
    /// <param name="style">The style.</param>
    extension(Style style)
    {
        /// <summary>
        /// Sets the foreground or background color of the specified style.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="foreground">Whether or not to set the foreground color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Style SetColor(Color color, bool foreground)
        {
            if (foreground)
            {
                return style.Foreground(color);
            }

            return style.Background(color);
        }
    }
}