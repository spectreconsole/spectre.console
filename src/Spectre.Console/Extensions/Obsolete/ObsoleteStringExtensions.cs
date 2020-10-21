using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class ObsoleteStringExtensions
    {
        /// <summary>
        /// Escapes text so that it won’t be interpreted as markup.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        /// <returns>A string that is safe to use in markup.</returns>
        [Obsolete("Use EscapeMarkup(..) instead.", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string SafeMarkup(this string text)
        {
            return text.EscapeMarkup();
        }
    }
}
