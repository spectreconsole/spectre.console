namespace Spectre.Console
{
    /// <summary>
    /// Represents a grid column.
    /// </summary>
    public sealed class GridColumn : IColumn
    {
        private Padding _padding;

        /// <summary>
        /// Gets or sets the width of the column.
        /// If <c>null</c>, the column will adapt to it's contents.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether wrapping of
        /// text within the column should be prevented.
        /// </summary>
        public bool NoWrap { get; set; }

        /// <summary>
        /// Gets or sets the padding of the column.
        /// </summary>
        public Padding Padding
        {
            get => _padding;
            set
            {
                HasExplicitPadding = true;
                _padding = value;
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the column.
        /// </summary>
        public Justify? Alignment { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user
        /// has set an explicit padding for this column.
        /// </summary>
        internal bool HasExplicitPadding { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridColumn"/> class.
        /// </summary>
        public GridColumn()
        {
            _padding = new Padding(0, 2);
        }
    }
}
