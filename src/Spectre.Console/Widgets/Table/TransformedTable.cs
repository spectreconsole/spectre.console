using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TransformedTable
    {
        public List<int> Widths { get; }
        public List<TableColumn> Columns { get; }
        public List<List<IRenderable>> Rows { get; }
        public int ExtraWidth { get; }

        public TransformedTable(
            List<TableColumn> columns,
            List<List<IRenderable>> rows,
            List<int> widths,
            int extraWidth)
        {
            Columns = columns ?? throw new System.ArgumentNullException(nameof(columns));
            Rows = rows ?? throw new System.ArgumentNullException(nameof(rows));
            Widths = widths ?? throw new System.ArgumentNullException(nameof(widths));
            ExtraWidth = extraWidth;
        }
    }
}
