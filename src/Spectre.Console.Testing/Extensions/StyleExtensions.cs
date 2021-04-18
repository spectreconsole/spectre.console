namespace Spectre.Console.Testing
{
    /// <summary>
    /// Contains extensions for <see cref="Style"/>.
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// Sets the foreground or background color of the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="color">The color.</param>
        /// <param name="foreground">Whether or not to set the foreground color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Style SetColor(this Style style, Color color, bool foreground)
        {
            if (foreground)
            {
                return style.Foreground(color);
            }

            return style.Background(color);
        }
    }
}
