using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable grid.
    /// </summary>
    public sealed class Grid : Renderable, IExpandable, IAlignable
    {
        private readonly Table _table;

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int ColumnCount => _table.ColumnCount;

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int RowCount => _table.RowCount;

        /// <inheritdoc/>
        public bool Expand
        {
            get => _table.Expand;
            set => _table.Expand = value;
        }

        /// <inheritdoc/>
        public Justify? Alignment
        {
            get => _table.Alignment;
            set => _table.Alignment = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class.
        /// </summary>
        public Grid()
        {
            _table = new Table
            {
                Border = TableBorder.None,
                ShowHeaders = false,
                IsGrid = true,
                PadRightCell = false,
            };
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return ((IRenderable)_table).Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int width)
        {
            return ((IRenderable)_table).Render(context, width);
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumn()
        {
            AddColumn(new GridColumn());
            return this;
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumn(GridColumn column)
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
            _table.PadRightCell = column.HasExplicitPadding;

            _table.AddColumn(new TableColumn(string.Empty)
            {
                Width = column.Width,
                NoWrap = column.NoWrap,
                Padding = column.Padding,
                Alignment = column.Alignment,
            });

            return this;
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddRow(params IRenderable[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Length > _table.ColumnCount)
            {
                throw new InvalidOperationException("The number of row columns are greater than the number of grid columns.");
            }

            _table.AddRow(columns);
            return this;
        }
    }
}
