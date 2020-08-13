using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Text"/>.
    /// </summary>
    public static class TextExtensions
    {
        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="text">The <see cref="Text"/> instance.</param>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>The same <see cref="Text"/> instance.</returns>
        public static Text WithAlignment(this Text text, Justify alignment)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            text.Alignment = alignment;
            return text;
        }
    }
}
