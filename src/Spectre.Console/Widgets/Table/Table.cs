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
        _columns = [];
        Rows = new TableRowCollection(this);
    }

    /// <summary>
    /// Adds a column to the table.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Table AddColumn(TableColumn column)
    {
        ArgumentNullException.ThrowIfNull(column);

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
        ArgumentNullException.ThrowIfNull(options);

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
        ArgumentNullException.ThrowIfNull(options);

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
    /// <summary>
    /// Adds multiple columns to the table.
    /// </summary>
    /// <param name="table">The table to add the column to.</param>
    /// <param name="columns">The columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddColumns(this Table table, params TableColumn[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(columns);

        foreach (var column in columns)
        {
            table.AddColumn(column);
        }

        return table;
    }

    /// <summary>
    /// Adds a row to the table.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, IEnumerable<IRenderable> columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(columns);

        table.Rows.Add(new TableRow(columns));
        return table;
    }

    /// <summary>
    /// Adds a row to the table.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, params IRenderable[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        return table.AddRow((IEnumerable<IRenderable>)columns);
    }

    /// <summary>
    /// Adds an empty row to the table.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddEmptyRow(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        var columns = new IRenderable[table.Columns.Count];
        Enumerable.Range(0, table.Columns.Count).ForEach(index => columns[index] = Text.Empty);
        table.AddRow(columns);
        return table;
    }

    /// <summary>
    /// Adds a column to the table.
    /// </summary>
    /// <param name="table">The table to add the column to.</param>
    /// <param name="column">The column to add.</param>
    /// <param name="configure">Delegate that can be used to configure the added column.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddColumn(this Table table, string column, Action<TableColumn>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(column);

        var tableColumn = new TableColumn(column);
        configure?.Invoke(tableColumn);

        table.AddColumn(tableColumn);
        return table;
    }

    /// <summary>
    /// Adds multiple columns to the table.
    /// </summary>
    /// <param name="table">The table to add the columns to.</param>
    /// <param name="columns">The columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddColumns(this Table table, params string[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(columns);

        foreach (var column in columns)
        {
            AddColumn(table, column);
        }

        return table;
    }

    /// <summary>
    /// Adds a row to the table.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, params string[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(columns);

        table.AddRow(columns.Select(column => new Markup(column)).ToArray());
        return table;
    }

    /// <summary>
    /// Inserts a row in the table at the specified index.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, IEnumerable<IRenderable> columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(columns);

        table.Rows.Insert(index, new TableRow(columns));
        return table;
    }

    /// <summary>
    /// Updates a tables cell.
    /// </summary>
    /// <param name="table">The table to update.</param>
    /// <param name="rowIndex">The index of row to update.</param>
    /// <param name="columnIndex">The index of column to update.</param>
    /// <param name="cellData">New cell data.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table UpdateCell(this Table table, int rowIndex, int columnIndex, IRenderable cellData)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(cellData);

        table.Rows.Update(rowIndex, columnIndex, cellData);

        return table;
    }

    /// <summary>
    /// Updates a tables cell.
    /// </summary>
    /// <param name="table">The table to update.</param>
    /// <param name="rowIndex">The index of row to update.</param>
    /// <param name="columnIndex">The index of column to update.</param>
    /// <param name="cellData">New cell data.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table UpdateCell(this Table table, int rowIndex, int columnIndex, string cellData)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(cellData);

        table.Rows.Update(rowIndex, columnIndex, new Markup(cellData));

        return table;
    }

    /// <summary>
    /// Inserts a row in the table at the specified index.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, params IRenderable[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        return InsertRow(table, index, (IEnumerable<IRenderable>)columns);
    }

    /// <summary>
    /// Inserts a row in the table at the specified index.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, params string[] columns)
    {
        ArgumentNullException.ThrowIfNull(table);

        return InsertRow(table, index, columns.Select(column => new Markup(column)));
    }

    /// <summary>
    /// Removes a row from the table with the specified index.
    /// </summary>
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to remove the row at.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table RemoveRow(this Table table, int index)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.Rows.RemoveAt(index);
        return table;
    }

    /// <summary>
    /// Sets the table width.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="width">The width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Width(this Table table, int? width)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.Width = width;
        return table;
    }

    /// <summary>
    /// Shows table headers.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowHeaders(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowHeaders = true;
        return table;
    }

    /// <summary>
    /// Hides table headers.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideHeaders(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowHeaders = false;
        return table;
    }

    /// <summary>
    /// Shows row separators.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowRowSeparators(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowRowSeparators = true;
        return table;
    }

    /// <summary>
    /// Hides row separators.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideRowSeparators(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowRowSeparators = false;
        return table;
    }

    /// <summary>
    /// Shows table footers.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowFooters(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowFooters = true;
        return table;
    }

    /// <summary>
    /// Hides table footers.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideFooters(this Table table)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.ShowFooters = false;
        return table;
    }

    /// <summary>
    /// Sets the table title.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="text">The table title markup text.</param>
    /// <param name="style">The table title style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Title(this Table table, string text, Style? style = null)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(text);

        return Title(table, new TableTitle(text, style));
    }

    /// <summary>
    /// Sets the table title.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="title">The table title.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Title(this Table table, TableTitle title)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.Title = title;
        return table;
    }

    /// <summary>
    /// Sets the table caption.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="text">The caption markup text.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Caption(this Table table, string text, Style? style = null)
    {
        ArgumentNullException.ThrowIfNull(table);

        ArgumentNullException.ThrowIfNull(text);

        return Caption(table, new TableTitle(text, style));
    }

    /// <summary>
    /// Sets the table caption.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="caption">The caption.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Caption(this Table table, TableTitle caption)
    {
        ArgumentNullException.ThrowIfNull(table);

        table.Caption = caption;
        return table;
    }
}