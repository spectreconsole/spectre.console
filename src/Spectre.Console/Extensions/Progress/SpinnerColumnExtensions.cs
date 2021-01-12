using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="SpinnerColumn"/>.
    /// </summary>
    public static class SpinnerColumnExtensions
    {
        /// <summary>
        /// Sets the style of the spinner.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SpinnerColumn Style(this SpinnerColumn column, Style? style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            column.Style = style;
            return column;
        }

        /// <summary>
        /// Sets the text that should be shown instead of the spinner
        /// once a task completes.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="text">The text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SpinnerColumn CompletedText(this SpinnerColumn column, string? text)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            column.CompletedText = text;
            return column;
        }

        /// <summary>
        /// Sets the completed style of the spinner.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SpinnerColumn CompletedStyle(this SpinnerColumn column, Style? style)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            column.CompletedStyle = style;
            return column;
        }
    }
}
