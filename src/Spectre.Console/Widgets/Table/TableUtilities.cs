using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;

namespace Spectre.Console.Internal
{
    internal static class TableUtilities
    {
        public static int GetMaxWidth(TableTransformerContext context, int maxWidth)
        {
            if (context.Width != null)
            {
                maxWidth = Math.Min(context.Width.Value, maxWidth);
            }

            maxWidth -= context.GetExtraWidth();
            return maxWidth;
        }

        // Calculate the widths of each column, including padding, not including borders.
        // Ported from Rich by Will McGugan, licensed under MIT.
        // https://github.com/willmcgugan/rich/blob/527475837ebbfc427530b3ee0d4d0741d2d0fc6d/rich/table.py#L394
        public static (List<int> Widths, int ExtraWidth) CalculateColumnWidths(TableTransformerContext context)
        {
            var maxWidth = GetMaxWidth(context, context.MaxWidth);
            var extraWidth = context.GetExtraWidth();

            var width_ranges = context.Columns.Select(column => MeasureColumn(context, column)).ToArray();
            var widths = width_ranges.Select(range => range.Max).ToList();

            var tableWidth = widths.Sum();
            if (tableWidth > maxWidth)
            {
                var wrappable = context.Columns.Select(c => !c.NoWrap).ToList();
                widths = CollapseWidths(widths, wrappable, maxWidth);
                tableWidth = widths.Sum();

                // last resort, reduce columns evenly
                if (tableWidth > maxWidth)
                {
                    var excessWidth = tableWidth - maxWidth;
                    widths = Ratio.Reduce(excessWidth, widths.Select(_ => 1).ToList(), widths, widths);
                    tableWidth = widths.Sum();
                }
            }

            if (tableWidth < maxWidth && context.Expand)
            {
                var padWidths = Ratio.Distribute(maxWidth - tableWidth, widths);
                widths = widths.Zip(padWidths, (a, b) => (a, b)).Select(f => f.a + f.b).ToList();
            }

            return (widths, extraWidth);
        }

        // Reduce widths so that the total is less or equal to the max width.
        // Ported from Rich by Will McGugan, licensed under MIT.
        // https://github.com/willmcgugan/rich/blob/527475837ebbfc427530b3ee0d4d0741d2d0fc6d/rich/table.py#L442
        public static List<int> CollapseWidths(List<int> widths, List<bool> wrappable, int maxWidth)
        {
            var totalWidth = widths.Sum();
            var excessWidth = totalWidth - maxWidth;

            if (wrappable.AnyTrue())
            {
                while (totalWidth != 0 && excessWidth > 0)
                {
                    var maxColumn = widths.Zip(wrappable, (first, second) => (width: first, allowWrap: second))
                        .Where(x => x.allowWrap)
                        .Max(x => x.width);

                    var secondMaxColumn = widths.Zip(wrappable, (width, allowWrap) => allowWrap && width != maxColumn ? width : 1).Max();
                    var columnDifference = maxColumn - secondMaxColumn;

                    var ratios = widths.Zip(wrappable, (width, allowWrap) => width == maxColumn && allowWrap ? 1 : 0).ToList();
                    if (!ratios.Any(x => x != 0) || columnDifference == 0)
                    {
                        break;
                    }

                    var maxReduce = widths.Select(_ => Math.Min(excessWidth, columnDifference)).ToList();
                    widths = Ratio.Reduce(excessWidth, ratios, maxReduce, widths);

                    totalWidth = widths.Sum();
                    excessWidth = totalWidth - maxWidth;
                }
            }

            return widths;
        }

        public static (int Min, int Max) MeasureColumn(TableTransformerContext context, TableColumn column)
        {
            var padding = column.Padding.GetWidth();

            // Predetermined width?
            if (column.Width != null)
            {
                return (column.Width.Value + padding, column.Width.Value + padding);
            }

            var columnIndex = context.Columns.IndexOf(column);
            var columnRows = context.Rows.Select(row => row[columnIndex]);

            var minWidths = new List<int>();
            var maxWidths = new List<int>();

            // Include columns in measurement
            var measure = column.Text.Measure(context.RenderContext, context.MaxWidth);
            minWidths.Add(measure.Min);
            maxWidths.Add(measure.Max);

            foreach (var row in columnRows)
            {
                measure = row.Measure(context.RenderContext, context.MaxWidth);
                minWidths.Add(measure.Min);
                maxWidths.Add(measure.Max);
            }

            return (minWidths.Count > 0 ? minWidths.Max() : padding,
                    maxWidths.Count > 0 ? maxWidths.Max() : context.MaxWidth);
        }
    }
}
