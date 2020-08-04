using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Composition;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a grid.
    /// </summary>
    public sealed class Grid : IRenderable
    {
        private readonly Table _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            _table = new Table(BorderKind.None, showHeaders: false);
        }

        /// <inheritdoc/>
        public int Measure(Encoding encoding, int maxWidth)
        {
            return _table.Measure(encoding, maxWidth);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(Encoding encoding, int width)
        {
            return _table.Render(encoding, width);
        }

        /// <summary>
        /// Adds a single column to the grid.
        /// </summary>
        public void AddColumn()
        {
            _table.AddColumn(string.Empty);
        }

        /// <summary>
        /// Adds the specified number of columns to the grid.
        /// </summary>
        /// <param name="count">The number of columns.</param>
        public void AddColumns(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _table.AddColumn(string.Empty);
            }
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        public void AddRow(params string[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Length < _table.ColumnCount)
            {
                throw new InvalidOperationException("The number of row columns are less than the number of grid columns.");
            }

            if (columns.Length > _table.ColumnCount)
            {
                throw new InvalidOperationException("The number of row columns are greater than the number of grid columns.");
            }

            _table.AddRow(columns);
        }
    }
}
