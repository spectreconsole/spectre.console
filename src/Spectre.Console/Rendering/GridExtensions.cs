using System;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Grid"/>.
    /// </summary>
    public static class GridExtensions
    {
        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the column to.</param>
        /// <param name="count">The number of columns to add.</param>
        public static void AddColumns(this Grid grid, int count)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            for (var index = 0; index < count; index++)
            {
                grid.AddColumn(new GridColumn());
            }
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the column to.</param>
        /// <param name="columns">The columns to add.</param>
        public static void AddColumns(this Grid grid, params GridColumn[] columns)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                grid.AddColumn(column);
            }
        }

        /// <summary>
        /// Adds an empty row to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the row to.</param>
        public static void AddEmptyRow(this Grid grid)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            var columns = new IRenderable[grid.ColumnCount];
            Enumerable.Range(0, grid.ColumnCount).ForEach(index => columns[index] = Text.Empty);
            grid.AddRow(columns);
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the row to.</param>
        /// <param name="columns">The columns to add.</param>
        public static void AddRow(this Grid grid, params string[] columns)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            grid.AddRow(columns.Select(column => new Markup(column)).ToArray());
        }
    }
}
