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
        /// Gets or sets the left padding.
        /// </summary>
        public int LeftPadding { get; set; }

        /// <summary>
        /// Gets or sets the right padding.
        /// </summary>
        public int RightPadding { get; set; }

        /// <summary>
        /// Gets or sets the ratio to use when calculating column width.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Ratio { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wrapping of
        /// text within the column should be prevented.
        /// </summary>
        public bool NoWrap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumn"/> class.
        /// </summary>
        /// <param name="text">The table column text.</param>
        public TableColumn(string text)
        {
            Text = Text.New(text ?? throw new ArgumentNullException(nameof(text)));
            Width = null;
            LeftPadding = 1;
            RightPadding = 1;
            Ratio = null;
            NoWrap = false;
        }

        internal int GetPadding()
        {
            return LeftPadding + RightPadding;
        }

        internal bool IsFlexible()
        {
            return Width == null;
        }
    }
}
