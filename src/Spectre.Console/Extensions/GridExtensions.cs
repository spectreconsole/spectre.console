namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Grid"/>.
/// </summary>
public static class GridExtensions
{
    /// <param name="grid">The grid to add the column to.</param>
    extension(Grid grid)
    {
        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="count">The number of columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumns(int count)
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
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumns(params GridColumn[] columns)
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddEmptyRow()
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
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddRow(params string[] columns)
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
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid Width(int? width)
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