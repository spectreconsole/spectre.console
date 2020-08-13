using System;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a table column.
    /// </summary>
    public sealed class TableColumn
    {
        /// <summary>
        /// Gets the text associated with the column.
        /// </summary>
        public Text Text { get; }

        /// <summary>
        /// Gets or sets the width of the column.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the padding of the column.
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
        {
            Text = Text.Markup(text ?? throw new ArgumentNullException(nameof(text)));
            Width = null;
            Padding = new Padding(1, 1);
            NoWrap = false;
            Alignment = null;
        }
    }
}
