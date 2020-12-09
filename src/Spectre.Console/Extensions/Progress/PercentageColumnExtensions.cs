using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="PercentageColumn"/>.
    /// </summary>
    public static class PercentageColumnExtensions
    {
        /// <summary>
        /// Sets the style for a non-complete task.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PercentageColumn Style(this PercentageColumn column, Style style)
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

        /// <summary>
        /// Sets the style for a completed task.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static PercentageColumn CompletedStyle(this PercentageColumn column, Style style)
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
    }
}
