namespace Spectre.Console;

/// <summary>
/// Represents a table column.
/// </summary>
public sealed class TableColumn : IColumn
{
    /// <summary>
    /// Gets or sets the column header.
    /// </summary>
    public IRenderable Header { get; set; }

    /// <summary>
    /// Gets or sets the column footer.
    /// </summary>
    public IRenderable? Footer { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// If <c>null</c>, the column will adapt to its contents.
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

/// <summary>
/// Contains extension methods for <see cref="TableColumn"/>.
/// </summary>
public static class TableColumnExtensions
{
    /// <param name="column">The table column.</param>
    extension(TableColumn column)
    {
        /// <summary>
        /// Sets the table column header.
        /// </summary>
        /// <param name="header">The table column header markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableColumn Header(string header)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (header is null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            column.Header = new Markup(header);
            return column;
        }

        /// <summary>
        /// Sets the table column header.
        /// </summary>
        /// <param name="header">The table column header.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableColumn Header(IRenderable header)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (header is null)
            {
                throw new ArgumentNullException(nameof(header));
            }

            column.Header = header;
            return column;
        }

        /// <summary>
        /// Sets the table column footer.
        /// </summary>
        /// <param name="footer">The table column footer markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableColumn Footer(string footer)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (footer is null)
            {
                throw new ArgumentNullException(nameof(footer));
            }

            column.Footer = new Markup(footer);
            return column;
        }

        /// <summary>
        /// Sets the table column footer.
        /// </summary>
        /// <param name="footer">The table column footer.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TableColumn Footer(IRenderable footer)
        {
            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (footer is null)
            {
                throw new ArgumentNullException(nameof(footer));
            }

            column.Footer = footer;
            return column;
        }
    }
}