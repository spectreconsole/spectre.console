using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="TableColumn"/>.
    /// </summary>
    public static class TableColumnExtensions
    {
        /// <summary>
        /// Sets the table column footer.
        /// </summary>
        /// <param name="column">The table column.</param>
        /// <param name="footer">The table column markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TableColumn Footer(this TableColumn column, string footer)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (footer is null)
            {
                throw new ArgumentNullException(nameof(footer));
            }

            column.Footer = new Markup(footer);
            return column;
        }

        /// <summary>
        /// Sets the table column footer.
        /// </summary>
        /// <param name="column">The table column.</param>
        /// <param name="footer">The table column footer.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TableColumn Footer(this TableColumn column, IRenderable footer)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (footer is null)
            {
                throw new ArgumentNullException(nameof(footer));
            }

            column.Footer = footer;
            return column;
        }
    }
}
