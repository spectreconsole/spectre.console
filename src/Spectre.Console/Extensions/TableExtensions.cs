namespace Spectre.Console;

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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, IEnumerable<IRenderable> columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, params IRenderable[] columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddEmptyRow(this Table table)
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
    /// <param name="table">The table to add the column to.</param>
    /// <param name="column">The column to add.</param>
    /// <param name="configure">Delegate that can be used to configure the added column.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddColumn(this Table table, string column, Action<TableColumn>? configure = null)
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
    /// <param name="table">The table to add the columns to.</param>
    /// <param name="columns">The columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddColumns(this Table table, params string[] columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table AddRow(this Table table, params string[] columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, IEnumerable<IRenderable> columns)
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
    /// <param name="table">The table to update.</param>
    /// <param name="rowIndex">The index of row to update.</param>
    /// <param name="columnIndex">The index of column to update.</param>
    /// <param name="cellData">New cell data.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table UpdateCell(this Table table, int rowIndex, int columnIndex, IRenderable cellData)
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
    /// <param name="table">The table to update.</param>
    /// <param name="rowIndex">The index of row to update.</param>
    /// <param name="columnIndex">The index of column to update.</param>
    /// <param name="cellData">New cell data.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table UpdateCell(this Table table, int rowIndex, int columnIndex, string cellData)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, params IRenderable[] columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The row columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table InsertRow(this Table table, int index, params string[] columns)
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
    /// <param name="table">The table to add the row to.</param>
    /// <param name="index">The index to remove the row at.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table RemoveRow(this Table table, int index)
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
    /// <param name="table">The table.</param>
    /// <param name="width">The width.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Width(this Table table, int? width)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowHeaders(this Table table)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideHeaders(this Table table)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowRowSeparators(this Table table)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideRowSeparators(this Table table)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table ShowFooters(this Table table)
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
    /// <param name="table">The table.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table HideFooters(this Table table)
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
    /// <param name="table">The table.</param>
    /// <param name="text">The table title markup text.</param>
    /// <param name="style">The table title style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Title(this Table table, string text, Style? style = null)
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
    /// <param name="table">The table.</param>
    /// <param name="title">The table title.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Title(this Table table, TableTitle title)
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
    /// <param name="table">The table.</param>
    /// <param name="text">The caption markup text.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Caption(this Table table, string text, Style? style = null)
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
    /// <param name="table">The table.</param>
    /// <param name="caption">The caption.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static Table Caption(this Table table, TableTitle caption)
    {
        if (table is null)
        {
            throw new ArgumentNullException(nameof(table));
        }

        table.Caption = caption;
        return table;
    }
}