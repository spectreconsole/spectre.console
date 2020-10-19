using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Escapes text so that it won’t be interpreted as markup.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <returns>A string that is safe to use in markup.</returns>
        public static string EscapeMarkup(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            return text
                .Replace("[", "[[")
                .Replace("]", "]]");
        }

        /// <summary>
        /// Escapes text so that it won’t be interpreted as markup.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <returns>A string that is safe to use in markup.</returns>
        [Obsolete("Use EscapeMarkup extension instead.", false)]
        public static string SafeMarkup(this string text)
        {
            return EscapeMarkup(text);
        }
    }
}
