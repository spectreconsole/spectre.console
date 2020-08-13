using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Composition;
using Spectre.Console.Internal;

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
                IsGrid = true,
                PadRightCell = false,
            };
        }

        /// <inheritdoc/>
        public Measurement Measure(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_table).Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(RenderContext context, int width)
        {
            return ((IRenderable)_table).Render(context, width);
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        public void AddColumn()
        {
            AddColumn(new GridColumn());
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

            if (_table.RowCount > 0)
            {
                throw new InvalidOperationException("Cannot add new columns to grid with existing rows.");
            }

            // Only pad the most right cell if we've explicitly set a padding.
            _table.PadRightCell = column.Padding != null;

            _table.AddColumn(new TableColumn(string.Empty)
            {
                Width = column.Width,
                NoWrap = column.NoWrap,
                Padding = column.Padding ?? new Padding(0, 2),
                Alignment = column.Alignment,
            });
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="count">The number of columns to add.</param>
        public void AddColumns(int count)
        {
            for (var index = 0; index < count; index++)
            {
                AddColumn(new GridColumn());
            }
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        public void AddColumns(params GridColumn[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                AddColumn(column);
            }
        }

        /// <summary>
        /// Adds an empty row to the grid.
        /// </summary>
        public void AddEmptyRow()
        {
            var columns = new string[_table.ColumnCount];
            Enumerable.Range(0, _table.ColumnCount).ForEach(index => columns[index] = string.Empty);
            AddRow(columns);
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
