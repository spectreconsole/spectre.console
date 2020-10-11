using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TableTransformerContext
    {
        private const int EdgeCount = 2;

        public RenderContext RenderContext { get; }
        public List<TableColumn> Columns { get; }
        public List<List<IRenderable>> Rows { get; }
        public int? Width { get; }
        public int MaxWidth { get; }
        public bool ShowHeaders { get; }
        public bool Expand { get; }
        public bool PadRightCell { get; }
        public TableBorder Border { get; }

        public TableTransformerContext(
            RenderContext renderContext,
            List<TableColumn> columns, List<List<IRenderable>> rows,
            int maxWidth, bool showHeaders, bool expand,
            bool padRightCell, TableBorder border, int? width)
        {
            RenderContext = renderContext ?? throw new ArgumentNullException(nameof(renderContext));
            Columns = new List<TableColumn>(columns ?? throw new ArgumentNullException(nameof(columns)));
            Rows = new List<List<IRenderable>>(rows ?? throw new ArgumentNullException(nameof(rows)));
            MaxWidth = maxWidth;
            ShowHeaders = showHeaders;
            Expand = expand;
            PadRightCell = padRightCell;
            Border = border ?? throw new ArgumentNullException(nameof(border));
            Width = width;
        }

        public void DropLastColumn()
        {
            Columns.RemoveAt(Columns.Count - 1);
        }

        public int GetExtraWidth()
        {
            var hideBorder = !Border.Visible;
            var separators = hideBorder ? 0 : Columns.Count - 1;
            var edges = hideBorder ? 0 : EdgeCount;
            var padding = Columns.Select(x => x.Padding.GetWidth()).Sum();

            if (!PadRightCell)
            {
                padding -= Columns.Last().Padding.Right;
            }

            return separators + edges + padding;
        }
    }
}
