namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="ProgressBarColumn"/>.
/// </summary>
public static class ProgressBarColumnExtensions
{
    /// <param name="column">The column.</param>
    extension(ProgressBarColumn column)
    {
        /// <summary>
        /// Sets the style of completed portions of the progress bar.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressBarColumn CompletedStyle(Style style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            column.CompletedStyle = style;
            return column;
        }

        /// <summary>
        /// Sets the style of a finished progress bar.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressBarColumn FinishedStyle(Style style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            column.FinishedStyle = style;
            return column;
        }

        /// <summary>
        /// Sets the style of remaining portions of the progress bar.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public ProgressBarColumn RemainingStyle(Style style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            column.RemainingStyle = style;
            return column;
        }
    }
}