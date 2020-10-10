using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TableSplitterContext
    {
        public RenderContext RenderContext { get; }
        public List<TableColumn> Columns { get; }
        public List<List<IRenderable>> Rows { get; }
        public int MaxWidth { get; }
        public bool ShowHeaders { get; }
        public bool Expand { get; }

        public TableSplitterContext(
            RenderContext renderContext,
            List<TableColumn> columns, List<List<IRenderable>> rows,
            int maxWidth, bool showHeaders, bool expand)
        {
            RenderContext = renderContext ?? throw new ArgumentNullException(nameof(renderContext));
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
            MaxWidth = maxWidth;
            ShowHeaders = showHeaders;
            Expand = expand;
        }
    }
}
