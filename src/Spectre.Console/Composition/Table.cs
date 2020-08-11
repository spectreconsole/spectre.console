using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool Expand { get; set; }

        /// <summary>
        /// Gets or sets the width of the table.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use
        /// a "safe" border on legacy consoles that might not be able
        /// to render non-ASCII characters. Defaults to <c>true</c>.
        /// </summary>
        public bool SafeBorder { get; set; } = true;

        // Whether this is a grid or not.
        internal bool IsGrid { get; set; }

        // Whether or not the most right cell should be padded.
        // This is almost always the case, unless we're rendering
        // a grid without explicit padding in the last cell.
        internal bool PadRightCell { get; set; } = true;

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

            AddColumn(new TableColumn(column));
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

            if (_rows.Count > 0)
            {
                throw new InvalidOperationException("Cannot add new columns to table with existing rows.");
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

            foreach (var column in columns)
            {
                AddColumn(column);
            }
        }

        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        public void AddColumns(params TableColumn[] columns)
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
        /// Adds an empty row to the table.
        /// </summary>
        public void AddEmptyRow()
        {
            var columns = new string[ColumnCount];
            Enumerable.Range(0, ColumnCount).ForEach(index => columns[index] = string.Empty);
            AddRow(columns);
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
        Measurement IRenderable.Measure(RenderContext context, int maxWidth)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (Width != null)
            {
                maxWidth = Math.Min(Width.Value, maxWidth);
            }

            maxWidth -= GetExtraWidth(includePadding: true);

            var measurements = _columns.Select(column => MeasureColumn(column, context, maxWidth)).ToList();
            var min = measurements.Sum(x => x.Min) + GetExtraWidth(includePadding: true);
            var max = Width ?? measurements.Sum(x => x.Max) + GetExtraWidth(includePadding: true);

            return new Measurement(min, max);
        }

        /// <inheritdoc/>
        IEnumerable<Segment> IRenderable.Render(RenderContext context, int width)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var border = Composition.Border.GetBorder(Border, (context.LegacyConsole || !context.Unicode) && SafeBorder);

            var showBorder = Border != BorderKind.None;
            var hideBorder = Border == BorderKind.None;
            var hasRows = _rows.Count > 0;

            var maxWidth = width;
            if (Width != null)
            {
                maxWidth = Math.Min(Width.Value, maxWidth);
            }

            maxWidth -= GetExtraWidth(includePadding: true);

            // Calculate the column and table widths
            var columnWidths = CalculateColumnWidths(context, maxWidth);

            // Update the table width.
            width = columnWidths.Sum() + GetExtraWidth(includePadding: true);

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
                foreach (var (columnIndex, _, _, (rowWidth, cell)) in columnWidths.Zip(row).Enumerate())
                {
                    var justification = _columns[columnIndex].Alignment;
                    var childContext = context.WithJustification(justification);

                    var lines = Segment.SplitLines(((IRenderable)cell).Render(childContext, rowWidth));
                    cellHeight = Math.Max(cellHeight, lines.Count);
                    cells.Add(lines);
                }

                // Show top of header?
                if (firstRow && showBorder)
                {
                    result.Add(new Segment(border.GetPart(BorderPart.HeaderTopLeft)));
                    foreach (var (columnIndex, _, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        var padding = _columns[columnIndex].Padding;

                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop, padding.Left))); // Left padding
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderTop, padding.Right))); // Right padding

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
                        if (showBorder || IsGrid)
                        {
                            var leftPadding = _columns[cellIndex].Padding.Left;
                            if (leftPadding > 0)
                            {
                                result.Add(new Segment(new string(' ', leftPadding)));
                            }
                        }

                        // Add content
                        result.AddRange(cell[cellRowIndex]);

                        // Pad cell content right
                        var length = cell[cellRowIndex].Sum(segment => segment.CellLength(context.Encoding));
                        if (length < columnWidths[cellIndex])
                        {
                            result.Add(new Segment(new string(' ', columnWidths[cellIndex] - length)));
                        }

                        // Pad column on the right side
                        if (showBorder || (hideBorder && !lastCell) || (hideBorder && lastCell && IsGrid && PadRightCell))
                        {
                            var rightPadding = _columns[cellIndex].Padding.Right;
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
                        else if (showBorder)
                        {
                            // Add column separator
                            result.Add(new Segment(border.GetPart(BorderPart.CellSeparator)));
                        }
                    }

                    result.Add(Segment.LineBreak());
                }

                // Show header separator?
                if (firstRow && showBorder && ShowHeaders && hasRows)
                {
                    result.Add(new Segment(border.GetPart(BorderPart.HeaderBottomLeft)));
                    foreach (var (columnIndex, first, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        var padding = _columns[columnIndex].Padding;

                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom, padding.Left))); // Left padding
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.HeaderBottom, padding.Right))); // Right padding

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
                        var padding = _columns[columnIndex].Padding;

                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, padding.Left))); // Left padding
                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, columnWidth)));
                        result.Add(new Segment(border.GetPart(BorderPart.FooterBottom, padding.Right))); // Right padding

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
