using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class TableRendererContext : TableAccessor
    {
        private readonly Table _table;
        private readonly List<TableRow> _rows;

        public override IReadOnlyList<TableRow> Rows => _rows;

        public TableBorder Border { get; }
        public Style BorderStyle { get; }
        public bool ShowBorder { get; }
        public bool HasRows { get; }
        public bool HasFooters { get; }

        /// <summary>
        /// Gets the max width of the destination area.
        /// The table might take up less than this.
        /// </summary>
        public int MaxWidth { get; }

        /// <summary>
        /// Gets the width of the table.
        /// </summary>
        public int TableWidth { get; }

        public bool HideBorder => !ShowBorder;
        public bool ShowHeaders => _table.ShowHeaders;
        public bool ShowFooters => _table.ShowFooters;
        public bool IsGrid => _table.IsGrid;
        public bool PadRightCell => _table.PadRightCell;
        public TableTitle? Title => _table.Title;
        public TableTitle? Caption => _table.Caption;
        public Justify? Alignment => _table.Alignment;

        public TableRendererContext(Table table, RenderContext options, IEnumerable<TableRow> rows, int tableWidth, int maxWidth)
            : base(table, options)
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _rows = new List<TableRow>(rows ?? Enumerable.Empty<TableRow>());

            ShowBorder = _table.Border.Visible;
            HasRows = Rows.Any(row => !row.IsHeader && !row.IsFooter);
            HasFooters = Rows.Any(column => column.IsFooter);
            Border = table.Border.GetSafeBorder(!options.Unicode && table.UseSafeBorder);
            BorderStyle = table.BorderStyle ?? Style.Plain;

            TableWidth = tableWidth;
            MaxWidth = maxWidth;
        }
    }
}
