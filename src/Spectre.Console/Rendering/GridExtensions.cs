using System;
using System.Linq;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Grid"/>.
    /// </summary>
    public static class GridExtensions
    {
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
