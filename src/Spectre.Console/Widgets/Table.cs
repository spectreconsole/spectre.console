using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A renderable table.
    /// </summary>
    public sealed class Table : Renderable, IHasTableBorder, IExpandable
    {
        private readonly List<TableColumn> _columns;
        private readonly List<List<IRenderable>> _rows;

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int ColumnCount => _columns.Count;

        /// <summary>
        /// Gets the number of rows in the table.
        /// </summary>
        public int RowCount => _rows.Count;

        /// <inheritdoc/>
        public TableBorder Border { get; set; } = TableBorder.Square;

        /// <inheritdoc/>
        public Style? BorderStyle { get; set; }

        /// <inheritdoc/>
        public bool UseSafeBorder { get; set; } = true;

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
        /// Gets or sets the table overflow strategy.
        /// </summary>
        public TableOverflow Overflow { get; set; } = TableOverflow.Default;

        /// <summary>
        /// Gets or sets the width of the table.
        /// </summary>
        public int? Width { get; set; }

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
            _rows = new List<List<IRenderable>>();
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumn(TableColumn column)
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
            return this;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(params IRenderable[] columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Length > _columns.Count)
            {
                throw new InvalidOperationException("The number of row columns are greater than the number of table columns.");
            }

            _rows.Add(columns.ToList());

            // Need to add missing columns?
            if (columns.Length < _columns.Count)
            {
                var diff = _columns.Count - columns.Length;
                Enumerable.Range(0, diff).ForEach(_ => _rows.Last().Add(Text.Empty));
            }

            return this;
        }

        /// <inheritdoc/>
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var tables = TransformTable(context, maxWidth);
            var measurements = tables.Select(x => x.Widths.Sum() + x.ExtraWidth);

            var min = measurements.Min();
            var max = measurements.Max();

            return new Measurement(min, max);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var result = new List<Segment>();

            var border = Border.GetSafeBorder((context.LegacyConsole || !context.Unicode) && UseSafeBorder);
            var borderStyle = BorderStyle ?? Style.Plain;

            var showBorder = Border.Visible;
            var hideBorder = !Border.Visible;
            var hasRows = _rows.Count > 0;

            // Transform the table into as many definitions needed depending on the overflow settings.
            foreach (var definition in TransformTable(context, maxWidth))
            {
                // Iterate all rows
                foreach (var (index, firstRow, lastRow, row) in definition.Rows.Enumerate())
                {
                    var cellHeight = 1;

                    // Get the list of cells for the row and calculate the cell height
                    var cells = new List<List<SegmentLine>>();
                    foreach (var (columnIndex, _, _, (rowWidth, cell)) in definition.Widths.Zip(row).Enumerate())
                    {
                        var justification = _columns[columnIndex].Alignment;
                        var childContext = context.WithJustification(justification);

                        var lines = Segment.SplitLines(cell.Render(childContext, rowWidth));
                        cellHeight = Math.Max(cellHeight, lines.Count);
                        cells.Add(lines);
                    }

                    // Show top of header?
                    if (firstRow && showBorder)
                    {
                        var separator = border.GetColumnRow(TablePart.Top, definition.Widths, _columns);
                        result.Add(new Segment(separator, borderStyle));
                        result.Add(Segment.LineBreak);
                    }

                    // Make cells the same shape
                    cells = Segment.MakeSameHeight(cellHeight, cells);

                    // Iterate through each cell row
                    foreach (var cellRowIndex in Enumerable.Range(0, cellHeight))
                    {
                        foreach (var (cellIndex, firstCell, lastCell, cell) in cells.Enumerate())
                        {
                            if (firstCell && showBorder)
                            {
                                // Show left column edge
                                var part = firstRow && ShowHeaders ? TableBorderPart.HeaderLeft : TableBorderPart.CellLeft;
                                result.Add(new Segment(border.GetPart(part), borderStyle));
                            }

                            // Pad column on left side.
                            if (showBorder || IsGrid)
                            {
                                var leftPadding = definition.Columns[cellIndex].Padding.Left;
                                if (leftPadding > 0)
                                {
                                    result.Add(new Segment(new string(' ', leftPadding)));
                                }
                            }

                            // Add content
                            result.AddRange(cell[cellRowIndex]);

                            // Pad cell content right
                            var length = cell[cellRowIndex].Sum(segment => segment.CellLength(context));
                            if (length < definition.Widths[cellIndex])
                            {
                                result.Add(new Segment(new string(' ', definition.Widths[cellIndex] - length)));
                            }

                            // Pad column on the right side
                            if (showBorder || (hideBorder && !lastCell) || (hideBorder && lastCell && IsGrid && PadRightCell))
                            {
                                var rightPadding = definition.Columns[cellIndex].Padding.Right;
                                if (rightPadding > 0)
                                {
                                    result.Add(new Segment(new string(' ', rightPadding)));
                                }
                            }

                            if (lastCell && showBorder)
                            {
                                // Add right column edge
                                var part = firstRow && ShowHeaders ? TableBorderPart.HeaderRight : TableBorderPart.CellRight;
                                result.Add(new Segment(border.GetPart(part), borderStyle));
                            }
                            else if (showBorder)
                            {
                                // Add column separator
                                var part = firstRow && ShowHeaders ? TableBorderPart.HeaderSeparator : TableBorderPart.CellSeparator;
                                result.Add(new Segment(border.GetPart(part), borderStyle));
                            }
                        }

                        result.Add(Segment.LineBreak);
                    }

                    // Show header separator?
                    if (firstRow && showBorder && ShowHeaders && hasRows)
                    {
                        var separator = border.GetColumnRow(TablePart.Separator, definition.Widths, definition.Columns);
                        result.Add(new Segment(separator, borderStyle));
                        result.Add(Segment.LineBreak);
                    }

                    // Show bottom of footer?
                    if (lastRow && showBorder)
                    {
                        var separator = border.GetColumnRow(TablePart.Bottom, definition.Widths, definition.Columns);
                        result.Add(new Segment(separator, borderStyle));
                        result.Add(Segment.LineBreak);
                    }
                }
            }

            return result;
        }

        private List<TransformedTable> TransformTable(RenderContext context, int maxWidth)
        {
            return TableTransformer.Transform(
                Overflow,
                new TableTransformerContext(
                    context, _columns, _rows, maxWidth,
                    ShowHeaders, Expand || Width != null,
                    PadRightCell, Border, Width));
        }
    }
}
