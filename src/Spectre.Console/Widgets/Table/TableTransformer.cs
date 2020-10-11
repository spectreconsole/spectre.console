using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal static class TableTransformer
    {
        public static List<TransformedTable> Transform(
            TableOverflow overflow, TableTransformerContext context)
        {
            return overflow switch
            {
                TableOverflow.Default => DefaultOverflow(context),
                TableOverflow.Drop => DropOverflow(context),
                _ => throw new InvalidOperationException("Unknown table overflow strategy."),
            };
        }

        private static List<TransformedTable> DefaultOverflow(TableTransformerContext context)
        {
            var (widths, extraWidth) = TableUtilities.CalculateColumnWidths(context);
            var rows = new List<List<IRenderable>>();

            // Add columns to top of rows?
            if (context.ShowHeaders)
            {
                rows.Add(new List<IRenderable>(context.Columns.Select(c => c.Text)));
            }

            // Add rows
            rows.AddRange(context.Rows);

            return new List<TransformedTable>(new[]
            {
                new TransformedTable(context.Columns, rows, widths, extraWidth),
            });
        }

        private static List<TransformedTable> DropOverflow(TableTransformerContext context)
        {
            while (context.Columns.Count > 0)
            {
                var (widths, extraWidth) = TableUtilities.CalculateColumnWidths(context);
                if (widths.None(w => w <= 0))
                {
                    return DefaultOverflow(context);
                }

                context.DropLastColumn();
            }

            return DefaultOverflow(context);
        }
    }
}
