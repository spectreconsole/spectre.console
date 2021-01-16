using System;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Table"/>.
    /// </summary>
    public static class TableExtensions
    {
        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="table">The table to add the column to.</param>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddColumns(this Table table, params TableColumn[] columns)
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
                table.AddColumn(column);
            }

            return table;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="table">The table to add the row to.</param>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddRow(this Table table, params IRenderable[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return table.AddRow(columns);
        }

        /// <summary>
        /// Adds an empty row to the table.
        /// </summary>
        /// <param name="table">The table to add the row to.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddEmptyRow(this Table table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var columns = new IRenderable[table.Columns.Count];
            Enumerable.Range(0, table.Columns.Count).ForEach(index => columns[index] = Text.Empty);
            table.AddRow(columns);
            return table;
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="table">The table to add the column to.</param>
        /// <param name="column">The column to add.</param>
        /// <param name="configure">Delegate that can be used to configure the added column.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddColumn(this Table table, string column, Action<TableColumn>? configure = null)
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
            configure?.Invoke(tableColumn);

            table.AddColumn(tableColumn);
            return table;
        }

        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="table">The table to add the columns to.</param>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddColumns(this Table table, params string[] columns)
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

            return table;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="table">The table to add the row to.</param>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table AddRow(this Table table, params string[] columns)
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
            return table;
        }

        /// <summary>
        /// Sets the table width.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table Width(this Table table, int? width)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Width = width;
            return table;
        }

        /// <summary>
        /// Shows table headers.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table ShowHeaders(this Table table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowHeaders = true;
            return table;
        }

        /// <summary>
        /// Hides table headers.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table HideHeaders(this Table table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowHeaders = false;
            return table;
        }

        /// <summary>
        /// Shows table footers.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table ShowFooters(this Table table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowFooters = true;
            return table;
        }

        /// <summary>
        /// Hides table footers.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table HideFooters(this Table table)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowFooters = false;
            return table;
        }

        /// <summary>
        /// Sets the table title.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="text">The table title markup text.</param>
        /// <param name="style">The table title style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table Title(this Table table, string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Title(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table title.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="title">The table title.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table Title(this Table table, TableTitle title)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Title = title;
            return table;
        }

        /// <summary>
        /// Sets the table caption.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="text">The caption markup text.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table Caption(this Table table, string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Caption(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table caption.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Table Caption(this Table table, TableTitle caption)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Caption = caption;
            return table;
        }
    }
}
