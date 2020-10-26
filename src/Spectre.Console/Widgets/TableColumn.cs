using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a table column.
    /// </summary>
    public sealed class TableColumn : IColumn
    {
        /// <summary>
        /// Gets the column header.
        /// </summary>
        public IRenderable Header { get; }

        /// <summary>
        /// Gets or sets the column footer.
        /// </summary>
        public IRenderable? Footer { get; set; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the padding of the column.
        /// Vertical padding (top and bottom) is ignored.
        /// </summary>
        public Padding? Padding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wrapping of
        /// text within the column should be prevented.
        /// </summary>
        public bool NoWrap { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the column.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// </summary>
        /// <param name="header">The table column header.</param>
        public TableColumn(string header)
            : this(new Markup(header).Overflow(Overflow.Ellipsis))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// </summary>
        /// <param name="header">The <see cref="IRenderable"/> instance to use as the table column header.</param>
        public TableColumn(IRenderable header)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Width = null;
            Padding = new Padding(1, 0, 1, 0);
            NoWrap = false;
            Alignment = null;
        }
    }
}
