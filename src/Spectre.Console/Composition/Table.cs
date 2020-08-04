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
    public sealed class Table : IRenderable
    {
        private readonly List<Text> _columns;
        private readonly List<List<Text>> _rows;
        private readonly Border _border;
        private readonly BorderKind _borderKind;
        private readonly bool _showHeaders;

        /// <summary>
        /// Gets the number of columns in the table.
        /// </summary>
        public int ColumnCount => _columns.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        /// <param name="border">The border to use.</param>
        /// <param name="showHeaders">Whether or not to show table headers.</param>
        public Table(BorderKind border = BorderKind.Square, bool showHeaders = true)
        {
            _columns = new List<Text>();
            _rows = new List<List<Text>>();
            _border = Border.GetBorder(border);
            _borderKind = border;
            _showHeaders = showHeaders;
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

            _columns.Add(Text.New(column));
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

            _columns.AddRange(columns.Select(column => Text.New(column)));
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
        public int Measure(Encoding encoding, int maxWidth)
        {
            // Calculate the max width for each column
            var maxColumnWidth = (maxWidth - (2 + (_columns.Count * 2) + (_columns.Count - 1))) / _columns.Count;
            var columnWidths = _columns.Select(c => c.Measure(encoding, maxColumnWidth)).ToArray();
            for (var rowIndex = 0; rowIndex < _rows.Count; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _rows[rowIndex].Count; columnIndex++)
                {
                    var columnWidth = _rows[rowIndex][columnIndex].Measure(encoding, maxColumnWidth);
                    if (columnWidth > columnWidths[columnIndex])
                    {
                        columnWidths[columnIndex] = columnWidth;
                    }
                }
            }

            // We now know the max width of each column, so let's recalculate the width
            return columnWidths.Sum() + 2 + (_columns.Count * 2) + (_columns.Count - 1);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(Encoding encoding, int width)
        {
            var showBorder = _borderKind != BorderKind.None;
            var hideBorder = _borderKind == BorderKind.None;

            var leftRightBorderWidth = _borderKind == BorderKind.None ? 0 : 2;
            var columnPadding = _borderKind == BorderKind.None ? _columns.Count : _columns.Count * 2;
            var separatorCount = _borderKind == BorderKind.None ? 0 : _columns.Count - 1;

            // Calculate the max width for each column.
            var maxColumnWidth = (width - (leftRightBorderWidth + columnPadding + separatorCount)) / _columns.Count;
            var columnWidths = _columns.Select(c => c.Measure(encoding, maxColumnWidth)).ToArray();
            for (var rowIndex = 0; rowIndex < _rows.Count; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _rows[rowIndex].Count; columnIndex++)
                {
                    var columnWidth = _rows[rowIndex][columnIndex].Measure(encoding, maxColumnWidth);
                    if (columnWidth > columnWidths[columnIndex])
                    {
                        columnWidths[columnIndex] = columnWidth;
                    }
                }
            }

            // We now know the max width of each column, so let's recalculate the width
            width = columnWidths.Sum() + leftRightBorderWidth + columnPadding + separatorCount;

            var rows = new List<List<Text>>();
            if (_showHeaders)
            {
                // Add columns to top of rows
                rows.Add(new List<Text>(_columns));
            }

            // Add tows.
            rows.AddRange(_rows);

            // Iterate all rows.
            var result = new List<Segment>();
            foreach (var (index, firstRow, lastRow, row) in rows.Enumerate())
            {
                var cellHeight = 1;

                // Get the list of cells for the row and calculate the cell height
                var cells = new List<List<SegmentLine>>();
                foreach (var (rowWidth, cell) in columnWidths.Zip(row, (f, s) => (f, s)))
                {
                    var lines = Segment.SplitLines(cell.Render(encoding, rowWidth));
                    cellHeight = Math.Max(cellHeight, lines.Count);
                    cells.Add(lines);
                }

                // Show top of header?
                if (firstRow && showBorder)
                {
                    result.Add(new Segment(_border.GetPart(BorderPart.HeaderTopLeft)));
                    foreach (var (columnIndex, _, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderTop))); // Left padding
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderTop, columnWidth)));
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderTop))); // Right padding

                        if (!lastColumn)
                        {
                            result.Add(new Segment(_border.GetPart(BorderPart.HeaderTopSeparator)));
                        }
                    }

                    result.Add(new Segment(_border.GetPart(BorderPart.HeaderTopRight)));
                    result.Add(Segment.LineBreak());
                }

                // Iterate through each cell row
                foreach (var cellRowIndex in Enumerable.Range(0, cellHeight))
                {
                    // Make cells the same shape
                    MakeSameHeight(cellHeight, cells);

                    var w00t = cells.Enumerate().ToArray();
                    foreach (var (cellIndex, firstCell, lastCell, cell) in w00t)
                    {
                        if (firstCell && showBorder)
                        {
                            // Show left column edge
                            result.Add(new Segment(_border.GetPart(BorderPart.CellLeft)));
                        }

                        // Pad column on left side.
                        if (showBorder)
                        {
                            result.Add(new Segment(" "));
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
                            result.Add(new Segment(" "));
                        }

                        if (lastCell && showBorder)
                        {
                            // Add right column edge
                            result.Add(new Segment(_border.GetPart(BorderPart.ColumnRight)));
                        }
                        else if (showBorder || (hideBorder && !lastCell))
                        {
                            // Add column separator
                            result.Add(new Segment(_border.GetPart(BorderPart.CellSeparator)));
                        }
                    }

                    result.Add(Segment.LineBreak());
                }

                // Show bottom of header?
                if (firstRow && showBorder)
                {
                    result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottomLeft)));
                    foreach (var (columnIndex, first, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottom))); // Left padding
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottom, columnWidth)));
                        result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottom))); // Right padding

                        if (!lastColumn)
                        {
                            result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottomSeparator)));
                        }
                    }

                    result.Add(new Segment(_border.GetPart(BorderPart.HeaderBottomRight)));
                    result.Add(Segment.LineBreak());
                }

                // Show bottom of footer?
                if (lastRow && showBorder)
                {
                    result.Add(new Segment(_border.GetPart(BorderPart.FooterBottomLeft)));
                    foreach (var (columnIndex, first, lastColumn, columnWidth) in columnWidths.Enumerate())
                    {
                        result.Add(new Segment(_border.GetPart(BorderPart.FooterBottom)));
                        result.Add(new Segment(_border.GetPart(BorderPart.FooterBottom, columnWidth)));
                        result.Add(new Segment(_border.GetPart(BorderPart.FooterBottom)));

                        if (!lastColumn)
                        {
                            result.Add(new Segment(_border.GetPart(BorderPart.FooterBottomSeparator)));
                        }
                    }

                    result.Add(new Segment(_border.GetPart(BorderPart.FooterBottomRight)));
                    result.Add(Segment.LineBreak());
                }
            }

            return result;
        }

        private static void MakeSameHeight(int cellHeight, List<List<SegmentLine>> cells)
        {
            foreach (var cell in cells)
            {
                if (cell.Count < cellHeight)
                {
                    while (cell.Count != cellHeight)
                    {
                        cell.Add(new SegmentLine());
                    }
                }
            }
        }
    }
}
