namespace Spectre.Console;

/// <summary>
/// A renderable table.
/// </summary>
public sealed class Table : Renderable, IHasTableBorder, IExpandable, IAlignable
{
    private readonly List<TableColumn> _columns;

    /// <summary>
    /// Gets the table columns.
    /// </summary>
    public IReadOnlyList<TableColumn> Columns => _columns;

    /// <summary>
    /// Gets the table rows.
    /// </summary>
    public TableRowCollection Rows { get; }

    /// <inheritdoc/>
    public TableBorder Border { get; set; } = TableBorder.Square;

    /// <inheritdoc/>
    public Style? BorderStyle { get; set; }

    /// <inheritdoc/>
    public bool UseSafeBorder { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not table headers should be shown.
    /// </summary>
    public bool ShowHeaders { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not row separators should be shown.
    /// </summary>
    public bool ShowRowSeparators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not table footers should be shown.
    /// </summary>
    public bool ShowFooters { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not the table should
    /// fit the available space. If <c>false</c>, the table width will be
    /// auto calculated. Defaults to <c>false</c>.
    /// </summary>
    public bool Expand { get; set; }

    /// <summary>
    /// Gets or sets the width of the table.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the table title.
    /// </summary>
    public TableTitle? Title { get; set; }

    /// <summary>
    /// Gets or sets the table footnote.
    /// </summary>
    public TableTitle? Caption { get; set; }

    /// <inheritdoc/>
    [Obsolete("Use the Align widget instead. This property will be removed in a later release.")]
    public Justify? Alignment { get; set; }

    // Whether this is a grid or not.
    internal bool IsGrid { get; set; }

    // Whether or not the most right cell should be padded.
    // This is almost always the case, unless we're rendering
    // a grid without explicit padding in the last cell.
    internal bool PadRightCell { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="Table"/> class.
    /// </summary>
    public Table()
    {
        _columns = new List<TableColumn>();
        Rows = new TableRowCollection(this);
    }

    /// <summary>
    /// Adds a column to the table.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Table AddColumn(TableColumn column)
    {
        if (column is null)
        {
            throw new ArgumentNullException(nameof(column));
        }

        if (Rows.Count > 0)
        {
            throw new InvalidOperationException("Cannot add new columns to table with existing rows.");
        }

        _columns.Add(column);
        return this;
    }

    /// <inheritdoc/>
    protected override Measurement Measure(RenderOptions options, int maxWidth)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var measurer = new TableMeasurer(this, options);

        // Calculate the total cell width
        var totalCellWidth = measurer.CalculateTotalCellWidth(maxWidth);

        // Calculate the minimum and maximum table width
        var measurements = _columns.Select(column => measurer.MeasureColumn(column, totalCellWidth));
        var minTableWidth = measurements.Sum(x => x.Min) + measurer.GetNonColumnWidth();
        var maxTableWidth = Width ?? measurements.Sum(x => x.Max) + measurer.GetNonColumnWidth();
        return new Measurement(minTableWidth, maxTableWidth);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        var measurer = new TableMeasurer(this, options);

        // Calculate the column and table width
        var totalCellWidth = measurer.CalculateTotalCellWidth(maxWidth);
        var columnWidths = measurer.CalculateColumnWidths(totalCellWidth);
        var tableWidth = columnWidths.Sum() + measurer.GetNonColumnWidth();

        // Get the rows to render
        var rows = GetRenderableRows();

        // Render the table
        return TableRenderer.Render(
            new TableRendererContext(this, options, rows, tableWidth, maxWidth),
            columnWidths);
    }

    private List<TableRow> GetRenderableRows()
    {
        var rows = new List<TableRow>();

        // Show headers?
        if (ShowHeaders)
        {
            rows.Add(TableRow.Header(_columns.Select(c => c.Header)));
        }

        // Add rows
        rows.AddRange(Rows);

        // Show footers?
        if (ShowFooters && _columns.Any(c => c.Footer != null))
        {
            rows.Add(TableRow.Footer(_columns.Select(c => c.Footer ?? Text.Empty)));
        }

        return rows;
    }
}

/// <summary>
/// Contains extension methods for <see cref="Table"/>.
/// </summary>
public static class TableExtensions
{
    /// <param name="table">The table to add the column to.</param>
    extension(Table table)
    {
        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumns(params TableColumn[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                table.AddColumn(column);
            }

            return table;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(IEnumerable<IRenderable> columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            table.Rows.Add(new TableRow(columns));
            return table;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(params IRenderable[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return table.AddRow((IEnumerable<IRenderable>)columns);
        }

        /// <summary>
        /// Adds an empty row to the table.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddEmptyRow()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            var columns = new IRenderable[table.Columns.Count];
            Enumerable.Range(0, table.Columns.Count).ForEach(index => columns[index] = Text.Empty);
            table.AddRow(columns);
            return table;
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <param name="configure">Delegate that can be used to configure the added column.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumn(string column, Action<TableColumn>? configure = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (column is null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var tableColumn = new TableColumn(column);
            configure?.Invoke(tableColumn);

            table.AddColumn(tableColumn);
            return table;
        }

        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddColumns(params string[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                AddColumn(table, column);
            }

            return table;
        }

        /// <summary>
        /// Adds a row to the table.
        /// </summary>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table AddRow(params string[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            table.AddRow(columns.Select(column => new Markup(column)).ToArray());
            return table;
        }

        /// <summary>
        /// Inserts a row in the table at the specified index.
        /// </summary>
        /// <param name="index">The index to insert the row at.</param>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table InsertRow(int index, IEnumerable<IRenderable> columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            table.Rows.Insert(index, new TableRow(columns));
            return table;
        }

        /// <summary>
        /// Updates a tables cell.
        /// </summary>
        /// <param name="rowIndex">The index of row to update.</param>
        /// <param name="columnIndex">The index of column to update.</param>
        /// <param name="cellData">New cell data.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table UpdateCell(int rowIndex, int columnIndex, IRenderable cellData)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (cellData is null)
            {
                throw new ArgumentNullException(nameof(cellData));
            }

            table.Rows.Update(rowIndex, columnIndex, cellData);

            return table;
        }

        /// <summary>
        /// Updates a tables cell.
        /// </summary>
        /// <param name="rowIndex">The index of row to update.</param>
        /// <param name="columnIndex">The index of column to update.</param>
        /// <param name="cellData">New cell data.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table UpdateCell(int rowIndex, int columnIndex, string cellData)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (cellData is null)
            {
                throw new ArgumentNullException(nameof(cellData));
            }

            table.Rows.Update(rowIndex, columnIndex, new Markup(cellData));

            return table;
        }

        /// <summary>
        /// Inserts a row in the table at the specified index.
        /// </summary>
        /// <param name="index">The index to insert the row at.</param>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table InsertRow(int index, params IRenderable[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return InsertRow(table, index, (IEnumerable<IRenderable>)columns);
        }

        /// <summary>
        /// Inserts a row in the table at the specified index.
        /// </summary>
        /// <param name="index">The index to insert the row at.</param>
        /// <param name="columns">The row columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table InsertRow(int index, params string[] columns)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return InsertRow(table, index, columns.Select(column => new Markup(column)));
        }

        /// <summary>
        /// Removes a row from the table with the specified index.
        /// </summary>
        /// <param name="index">The index to remove the row at.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table RemoveRow(int index)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Rows.RemoveAt(index);
            return table;
        }

        /// <summary>
        /// Sets the table width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table Width(int? width)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Width = width;
            return table;
        }

        /// <summary>
        /// Shows table headers.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table ShowHeaders()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowHeaders = true;
            return table;
        }

        /// <summary>
        /// Hides table headers.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table HideHeaders()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowHeaders = false;
            return table;
        }

        /// <summary>
        /// Shows row separators.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table ShowRowSeparators()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowRowSeparators = true;
            return table;
        }

        /// <summary>
        /// Hides row separators.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table HideRowSeparators()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowRowSeparators = false;
            return table;
        }

        /// <summary>
        /// Shows table footers.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table ShowFooters()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowFooters = true;
            return table;
        }

        /// <summary>
        /// Hides table footers.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table HideFooters()
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.ShowFooters = false;
            return table;
        }

        /// <summary>
        /// Sets the table title.
        /// </summary>
        /// <param name="text">The table title markup text.</param>
        /// <param name="style">The table title style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table Title(string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Title(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table title.
        /// </summary>
        /// <param name="title">The table title.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table Title(TableTitle title)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Title = title;
            return table;
        }

        /// <summary>
        /// Sets the table caption.
        /// </summary>
        /// <param name="text">The caption markup text.</param>
        /// <param name="style">The style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table Caption(string text, Style? style = null)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Caption(table, new TableTitle(text, style));
        }

        /// <summary>
        /// Sets the table caption.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Table Caption(TableTitle caption)
        {
            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            table.Caption = caption;
            return table;
        }
    }
}