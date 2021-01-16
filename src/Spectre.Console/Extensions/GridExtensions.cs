using System;
using System.Linq;
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Grid AddColumns(this Grid grid, int count)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            for (var index = 0; index < count; index++)
            {
                grid.AddColumn(new GridColumn());
            }

            return grid;
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the column to.</param>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Grid AddColumns(this Grid grid, params GridColumn[] columns)
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

            return grid;
        }

        /// <summary>
        /// Adds an empty row to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the row to.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Grid AddEmptyRow(this Grid grid)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            var columns = new IRenderable[grid.Columns.Count];
            Enumerable.Range(0, grid.Columns.Count).ForEach(index => columns[index] = Text.Empty);
            grid.AddRow(columns);

            return grid;
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        /// <param name="grid">The grid to add the row to.</param>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Grid AddRow(this Grid grid, params string[] columns)
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
            return grid;
        }

        /// <summary>
        /// Sets the grid width.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static Grid Width(this Grid grid, int? width)
        {
            if (grid is null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            grid.Width = width;
            return grid;
        }
    }
}
