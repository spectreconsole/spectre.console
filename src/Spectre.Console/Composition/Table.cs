using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console.Composition;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a table.
    /// </summary>
    public sealed partial class Table : IRenderable
    {
        private readonly List<TableColumn> _columns;
        private readonly List<List<Text>> _rows;

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int ColumnCount => _columns.Count;

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int RowCount => _rows.Count;

        /// <summary>
        /// Gets or sets the kind of border to use.
        /// </summary>
        public BorderKind Border { get; set; } = BorderKind.Square;

        /// <summary>
        /// Gets or sets a value indicating whether or not table headers should be shown.
        /// </summary>
        public bool ShowHeaders { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether or not the table should
        /// fit the available space. If <c>false</c>, the table width will be
        /// auto calculated. Defaults to <c>false</c>.
        /// </summary>
        public bool Expand { get; set; } = false;

        /// <summary>
        /// Gets or sets the width of the table.
        /// </summary>
        public int? Width { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table()
        {
            _columns = new List<TableColumn>();
            _rows = new List<List<Text>>();
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(string column)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            _columns.Add(new TableColumn(column));
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        public void AddColumn(TableColumn column)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            _columns.Add(column);
        }

        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        public void AddColumns(params string[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            _columns.AddRange(columns.Select(column => new TableColumn(column)));
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        public void AddRow(params string[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Length < _columns.Count)
            {
                throw new InvalidOperationException("The number of row columns are less than the number of table columns.");
            }

            if (columns.Length > _columns.Count)
            {
                throw new InvalidOperationException("The number of row columns are greater than the number of table columns.");
            }

            _rows.Add(columns.Select(column => Text.New(column)).ToList());
        }

        /// <inheritdoc/>
        Measurement IRenderable.Measure(Encoding encoding, int maxWidth)
        {
            if (Width != null)
            {
                maxWidth = Math.Min(Width.Value, maxWidth);
            }

            maxWidth -= GetExtraWidth(includePadding: true);

            var measurements = _columns.Select(column => MeasureColumn(column, encoding, maxWidth)).ToList();
            var min = measurements.Sum(x => x.Min) + GetExtraWidth(includePadding: true);
            var max = Width ?? measurements.Sum(x => x.Max) + GetExtraWidth(includePadding: true);

            return new Measurement(min, max);
        }

        /// <inheritdoc/>
        IEnumerable<Segment> IRenderable.Render(Encoding encoding, int width)
        {
            var border = Composition.Border.GetBorder(Border);

            var showBorder = Border != BorderKind.None;
            var hideBorder = Border == BorderKind.None;

            var maxWidth = width;
            if (Width != null)
            {
                maxWidth = Math.Min(Width.Value, maxWidth);
            }

            maxWidth -= GetExtraWidth(includePadding: true);

            // Calculate the column and table widths
            var columnWidths = CalculateColumnWidths(encoding, maxWidth);

            // Update the table width.
            width = columnWidths.Sum() + GetExtraWidth(includePadding: false);

            var rows = new List<List<Text>>();
            if (ShowHeaders)
            {
                // Add columns to top of rows
                rows.Add(new List<Text>(_columns.Select(c => c.Text)));
            }

            // Add rows.
            rows.AddRange(_rows);

            // Iterate all rows
            var result = new List<Segment>();
            foreach (var (index, firstRow, lastRow, row) in rows.Enumerate())
            {
                var cellHeight = 1;

                // Get the list of cells for the row and calculate the cell height
                var cells = new List<List<SegmentLine>>();
                foreach (var (rowWidth, cell) in columnWidths.Zip(row, (f, s) => (f, s)))
                {
                    var lines = Segment.SplitLines(((IRenderable)cell).Render(encoding, rowWidth));
                    cellHeight = Math.Max(cellHeight, lines.Count);
                    cells.Add(lines);
                }

                // Show top of header?
                if (firstRow && showBorder)
                {
                    result.Add(new Segment(border.GetPart(BorderPart.HeaderTopLeft)));
                    foreach (var (columnIndex, _, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop))); // Left padding
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop))); // Right padding

                        if (!lastColumn)
                        {
                            result.Add(new Segment(border.GetPart(BorderPart.HeaderTopSeparator)));
                        }
                    }

                    result.Add(new Segment(border.GetPart(BorderPart.HeaderTopRight)));
                    result.Add(Segment.LineBreak());
                }

                // Iterate through each cell row
                foreach (var cellRowIndex in Enumerable.Range(0, cellHeight))
                {
                    // Make cells the same shape
                    cells = Segment.MakeSameHeight(cellHeight, cells);

                    foreach (var (cellIndex, firstCell, lastCell, cell) in cells.Enumerate())
                    {
                        if (firstCell && showBorder)
                        {
                            // Show left column edge
                            result.Add(new Segment(border.GetPart(BorderPart.CellLeft)));
                        }

                        // Pad column on left side.
                        if (showBorder)
                        {
                            var leftPadding = _columns[cellIndex].LeftPadding;
                            if (leftPadding > 0)
                            {
                                result.Add(new Segment(new string(' ', leftPadding)));
                            }
                        }

                        // Add content
                        result.AddRange(cell[cellRowIndex]);

                        // Pad cell content right
                        var length = cell[cellRowIndex].Sum(segment => segment.CellLength(encoding));
                        if (length < columnWidths[cellIndex])
                        {
                            result.Add(new Segment(new string(' ', columnWidths[cellIndex] - length)));
                        }

                        // Pad column on the right side
                        if (showBorder || (hideBorder && !lastCell))
                        {
                            var rightPadding = _columns[cellIndex].RightPadding;
                            if (rightPadding > 0)
                            {
                                result.Add(new Segment(new string(' ', rightPadding)));
                            }
                        }

                        if (lastCell && showBorder)
                        {
                            // Add right column edge
                            result.Add(new Segment(border.GetPart(BorderPart.CellRight)));
                        }
                        else if (showBorder || (hideBorder && !lastCell))
                        {
                            // Add column separator
                            result.Add(new Segment(border.GetPart(BorderPart.CellSeparator)));
                        }
                    }

                    result.Add(Segment.LineBreak());
                }

                // Show bottom of header?
                if (firstRow && showBorder && ShowHeaders)
                {
                    result.Add(new Segment(border.GetPart(BorderPart.HeaderBottomLeft)));
                    foreach (var (columnIndex, first, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom))); // Left padding
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom))); // Right padding

                        if (!lastColumn)
                        {
                            result.Add(new Segment(border.GetPart(BorderPart.HeaderBottomSeparator)));
                        }
                    }

                    result.Add(new Segment(border.GetPart(BorderPart.HeaderBottomRight)));
                    result.Add(Segment.LineBreak());
                }

                // Show bottom of footer?
                if (lastRow && showBorder)
                {
                    result.Add(new Segment(border.GetPart(BorderPart.FooterBottomLeft)));
                    foreach (var (columnIndex, first, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom)));
                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom)));

                        if (!lastColumn)
                        {
                            result.Add(new Segment(border.GetPart(BorderPart.FooterBottomSeparator)));
                        }
                    }

                    result.Add(new Segment(border.GetPart(BorderPart.FooterBottomRight)));
                    result.Add(Segment.LineBreak());
                }
            }

            return result;
        }

        private bool ShouldExpand()
        {
            return Expand || Width != null;
        }
    }
}
