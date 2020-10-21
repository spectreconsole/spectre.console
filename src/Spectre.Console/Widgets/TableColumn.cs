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
        /// Gets the text associated with the column.
        /// </summary>
        public IRenderable Text { get; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the padding of the column.
        /// Vertical padding (top and bottom) is ignored.
        /// </summary>
        public Padding Padding { get; set; }

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
        /// <param name="text">The table column text.</param>
        public TableColumn(string text)
            : this(new Markup(text).Overflow(Overflow.Ellipsis))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// </summary>
        /// <param name="renderable">The <see cref="IRenderable"/> instance to use as the table column.</param>
        public TableColumn(IRenderable renderable)
        {
            Text = renderable ?? throw new ArgumentNullException(nameof(renderable));
            Width = null;
            Padding = new Padding(1, 0, 1, 0);
            NoWrap = false;
            Alignment = null;
        }
    }
}
