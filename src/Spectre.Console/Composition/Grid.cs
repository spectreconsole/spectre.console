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
            _table = new Table
            {
                Border = BorderKind.None,
                ShowHeaders = false,
            };
        }

        /// <inheritdoc/>
        public Measurement Measure(Encoding encoding, int maxWidth)
        {
            return ((IRenderable)_table).Measure(encoding, maxWidth);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(Encoding encoding, int width)
        {
            return ((IRenderable)_table).Render(encoding, width);
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        public void AddColumn()
        {
            _table.AddColumn(string.Empty);
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(GridColumn column)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            _table.AddColumn(new TableColumn(string.Empty)
            {
                Width = column.Width,
                NoWrap = column.NoWrap,
                LeftPadding = 0,
                RightPadding = 1,
            });
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
