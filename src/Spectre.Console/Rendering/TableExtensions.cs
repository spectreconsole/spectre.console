using System;
using System.Linq;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Table"/>.
    /// </summary>
    public static class TableExtensions
    {
        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="table">The table to add the column to.</param>
        /// <param name="column">The column to add.</param>
        /// <returns>The added <see cref="TableColumn"/> instance.</returns>
        public static TableColumn AddColumn(this Table table, string column)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var tableColumn = new TableColumn(column);
            table.AddColumn(tableColumn);

            return tableColumn;
        }

        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="table">The table to add the columns to.</param>
        /// <param name="columns">The columns to add.</param>
        public static void AddColumns(this Table table, params string[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                AddColumn(table, column);
            }
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="table">The table to add the row to.</param>
        /// <param name="columns">The row columns to add.</param>
        public static void AddRow(this Table table, params string[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            table.AddRow(columns.Select(column => new Markup(column)).ToArray());
        }
    }
}
