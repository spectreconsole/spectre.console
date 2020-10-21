using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Contains extension methods for <see cref="TableBorder"/>.
    /// </summary>
    public static class TableBorderExtensions
    {
        /// <summary>
        /// Gets the safe border for a border.
        /// </summary>
        /// <param name="border">The border to get the safe border for.</param>
        /// <param name="safe">Whether or not to return the safe border.</param>
        /// <returns>The safe border if one exist, otherwise the original border.</returns>
        public static TableBorder GetSafeBorder(this TableBorder border, bool safe)
        {
            if (border is null)
            {
                throw new ArgumentNullException(nameof(border));
            }

            if (safe && border.SafeBorder != null)
            {
                border = border.SafeBorder;
            }

            return border;
        }
    }
}
