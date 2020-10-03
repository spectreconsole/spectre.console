namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to something that is safe to
        /// use in a markup string.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>A string that is safe to use in a markup string.</returns>
        public static string SafeMarkup(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text
                .Replace("[", "[[")
                .Replace("]", "]]");
        }
    }
}
