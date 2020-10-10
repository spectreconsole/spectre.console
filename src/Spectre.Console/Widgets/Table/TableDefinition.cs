using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TableDefinition
    {
        public List<int> Widths { get; }
        public List<TableColumn> Columns { get; }
        public List<List<IRenderable>> Rows { get; }

        public TableDefinition(
            List<TableColumn> columns,
            List<List<IRenderable>> rows,
            List<int> widths)
        {
            Columns = columns ?? throw new System.ArgumentNullException(nameof(columns));
            Rows = rows ?? throw new System.ArgumentNullException(nameof(rows));
            Widths = widths ?? throw new System.ArgumentNullException(nameof(widths));
        }
    }
}
