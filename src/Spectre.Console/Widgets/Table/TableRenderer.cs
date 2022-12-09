namespace Spectre.Console;

internal static class TableRenderer
{
    private static readonly Style _defaultHeadingStyle = new Style(Color.Silver);
    private static readonly Style _defaultCaptionStyle = new Style(Color.Grey);

    public static List<Segment> Render(TableRendererContext context, List<int> columnWidths)
    {
        // Can't render the table?
        if (context.TableWidth <= 0 || context.TableWidth > context.MaxWidth || columnWidths.Any(c => c <= 0))
        {
            return new List<Segment>(new[] { new Segment("…", context.BorderStyle ?? Style.Plain) });
        }

        var result = new List<Segment>();
        result.AddRange(RenderAnnotation(context, context.Title, _defaultHeadingStyle));

        // Iterate all rows
        foreach (var (index, isFirstRow, isLastRow, row) in context.Rows.Enumerate())
        {
            var cellHeight = 1;

            // Get the list of cells for the row and calculate the cell height
            var cells = new List<List<SegmentLine>>();
            foreach (var (columnIndex, _, _, (rowWidth, cell)) in columnWidths.Zip(row).Enumerate())
            {
                var justification = context.Columns[columnIndex].Alignment;
                var childContext = context.Options with { Justification = justification };

                var lines = Segment.SplitLines(cell.Render(childContext, rowWidth));
                cellHeight = Math.Max(cellHeight, lines.Count);
                cells.Add(lines);
            }

            // Show top of header?
            if (isFirstRow && context.ShowBorder)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.Top, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Segment(separator, context.BorderStyle));
                result.Add(Segment.LineBreak);
            }

            // Show footer separator?
            if (context.ShowFooters && isLastRow && context.ShowBorder && context.HasFooters)
            {
                var textBorder = context.Border.GetColumnRow(TablePart.FooterSeparator, columnWidths, context.Columns);
                if (!string.IsNullOrEmpty(textBorder))
                {
                    var separator = Aligner.Align(textBorder, context.Alignment, context.MaxWidth);
                    result.Add(new Segment(separator, context.BorderStyle));
                    result.Add(Segment.LineBreak);
                }
            }

            // Make cells the same shape
            cells = Segment.MakeSameHeight(cellHeight, cells);

            // Iterate through each cell row
            foreach (var cellRowIndex in Enumerable.Range(0, cellHeight))
            {
                var rowResult = new List<Segment>();

                foreach (var (cellIndex, isFirstCell, isLastCell, cell) in cells.Enumerate())
                {
                    if (isFirstCell && context.ShowBorder)
                    {
                        // Show left column edge
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderLeft : TableBorderPart.CellLeft;
                        rowResult.Add(new Segment(context.Border.GetPart(part), context.BorderStyle));
                    }

                    // Pad column on left side.
                    if (context.ShowBorder || context.IsGrid)
                    {
                        var leftPadding = context.Columns[cellIndex].Padding.GetLeftSafe();
                        if (leftPadding > 0)
                        {
                            rowResult.Add(new Segment(new string(' ', leftPadding)));
                        }
                    }

                    // Add content
                    rowResult.AddRange(cell[cellRowIndex]);

                    // Pad cell content right
                    var length = cell[cellRowIndex].Sum(segment => segment.CellCount());
                    if (length < columnWidths[cellIndex])
                    {
                        rowResult.Add(new Segment(new string(' ', columnWidths[cellIndex] - length)));
                    }

                    // Pad column on the right side
                    if (context.ShowBorder || (context.HideBorder && !isLastCell) || (context.HideBorder && isLastCell && context.IsGrid && context.PadRightCell))
                    {
                        var rightPadding = context.Columns[cellIndex].Padding.GetRightSafe();
                        if (rightPadding > 0)
                        {
                            rowResult.Add(new Segment(new string(' ', rightPadding)));
                        }
                    }

                    if (isLastCell && context.ShowBorder)
                    {
                        // Add right column edge
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderRight : TableBorderPart.CellRight;
                        rowResult.Add(new Segment(context.Border.GetPart(part), context.BorderStyle));
                    }
                    else if (context.ShowBorder)
                    {
                        // Add column separator
                        var part = isFirstRow && context.ShowHeaders ? TableBorderPart.HeaderSeparator : TableBorderPart.CellSeparator;
                        rowResult.Add(new Segment(context.Border.GetPart(part), context.BorderStyle));
                    }
                }

                // Align the row result.
                Aligner.Align(rowResult, context.Alignment, context.MaxWidth);

                // Is the row larger than the allowed max width?
                if (Segment.CellCount(rowResult) > context.MaxWidth)
                {
                    result.AddRange(Segment.Truncate(rowResult, context.MaxWidth));
                }
                else
                {
                    result.AddRange(rowResult);
                }

                result.Add(Segment.LineBreak);
            }

            // Show header separator?
            if (isFirstRow && context.ShowBorder && context.ShowHeaders && context.HasRows)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.HeaderSeparator, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Segment(separator, context.BorderStyle));
                result.Add(Segment.LineBreak);
            }

            // Show bottom of footer?
            if (isLastRow && context.ShowBorder)
            {
                var separator = Aligner.Align(context.Border.GetColumnRow(TablePart.Bottom, columnWidths, context.Columns), context.Alignment, context.MaxWidth);
                result.Add(new Segment(separator, context.BorderStyle));
                result.Add(Segment.LineBreak);
            }
        }

        result.AddRange(RenderAnnotation(context, context.Caption, _defaultCaptionStyle));
        return result;
    }

    private static IEnumerable<Segment> RenderAnnotation(TableRendererContext context, TableTitle? header, Style defaultStyle)
    {
        if (header == null)
        {
            return Array.Empty<Segment>();
        }

        var paragraph = new Markup(header.Text, header.Style ?? defaultStyle)
            .Justify(Justify.Center)
            .Overflow(Overflow.Ellipsis);

        // Render the paragraphs
        var segments = new List<Segment>();
        segments.AddRange(((IRenderable)paragraph).Render(context.Options, context.TableWidth));

        // Align over the whole buffer area
        Aligner.Align(segments, context.Alignment, context.MaxWidth);

        segments.Add(Segment.LineBreak);
        return segments;
    }
}