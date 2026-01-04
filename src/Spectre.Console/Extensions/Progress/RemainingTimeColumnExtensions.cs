namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="RemainingTimeColumn"/>.
/// </summary>
public static class RemainingTimeColumnExtensions
{
    /// <param name="column">The column.</param>
    extension(RemainingTimeColumn column)
    {
        /// <summary>
        /// Sets the style of the remaining time text.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public RemainingTimeColumn Style(Style style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            column.Style = style;
            return column;
        }
    }
}