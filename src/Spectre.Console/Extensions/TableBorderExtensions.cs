namespace Spectre.Console.Rendering;

/// <summary>
/// Contains extension methods for <see cref="TableBorder"/>.
/// </summary>
public static class TableBorderExtensions
{
    /// <param name="border">The border to get the safe border for.</param>
    extension(TableBorder border)
    {
        /// <summary>
        /// Gets the safe border for a border.
        /// </summary>
        /// <param name="safe">Whether or not to return the safe border.</param>
        /// <returns>The safe border if one exist, otherwise the original border.</returns>
        public TableBorder GetSafeBorder(bool safe)
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