using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal static class TableSplitter
    {
        public static List<TableDefinition> Split(
            TableOverflow overflow, TableSplitterContext context)
        {
            return overflow switch
            {
                TableOverflow.Default => DefaultSplitter(context),
                _ => throw new InvalidOperationException("Unknown table overflow splitting strategy."),
            };
        }

        private static List<TableDefinition> DefaultSplitter(TableSplitterContext context)
        {
            var widths = TableUtilities.CalculateColumnWidths(context);

            var rows = new List<List<IRenderable>>();
            if (context.ShowHeaders)
            {
                // Add columns to top of rows
                rows.Add(new List<IRenderable>(context.Columns.Select(c => c.Text)));
            }

            // Add rows.
            rows.AddRange(context.Rows);

            var definition = new TableDefinition(context.Columns, rows, widths);

            // Return the calculation.
            return new List<TableDefinition>(new[] { definition });
        }
    }
}
