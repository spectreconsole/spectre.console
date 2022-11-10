namespace Spectre.Console;

/// <summary>
/// Represents a collection holding table rows.
/// </summary>
public sealed class TableRowCollection : IReadOnlyList<TableRow>
{
    private readonly Table _table;
    private readonly IList<TableRow> _list;
    private readonly object _lock;

    /// <inheritdoc/>
    TableRow IReadOnlyList<TableRow>.this[int index]
    {
        get
        {
            lock (_lock)
            {
                return _list[index];
            }
        }
    }

    /// <summary>
    /// Gets the number of rows in the collection.
    /// </summary>
    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }
    }

    internal TableRowCollection(Table table)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        _list = new List<TableRow>();
        _lock = new object();
    }

    /// <summary>
    /// Adds a new row.
    /// </summary>
    /// <param name="columns">The columns that are part of the row to add.</param>
    /// <returns>The index of the added item.</returns>
    public int Add(IEnumerable<IRenderable> columns)
    {
        if (columns is null)
        {
            throw new ArgumentNullException(nameof(columns));
        }

        lock (_lock)
        {
            var row = CreateRow(columns);
            _list.Add(row);
            return _list.IndexOf(row);
        }
    }

    /// <summary>
    /// Inserts a new row at the specified index.
    /// </summary>
    /// <param name="index">The index to insert the row at.</param>
    /// <param name="columns">The columns that are part of the row to insert.</param>
    /// <returns>The index of the inserted item.</returns>
    public int Insert(int index, IEnumerable<IRenderable> columns)
    {
        if (columns is null)
        {
            throw new ArgumentNullException(nameof(columns));
        }

        lock (_lock)
        {
            var row = CreateRow(columns);
            _list.Insert(index, row);
            return _list.IndexOf(row);
        }
    }

    /// <summary>
    /// Update a table cell at the specified index.
    /// </summary>
    /// <param name="row">Index of cell row.</param>
    /// <param name="column">index of cell column.</param>
    /// <param name="cellData">The new cells details.</param>
    public void Update(int row, int column, IRenderable cellData)
    {
        if (cellData is null)
        {
            throw new ArgumentNullException(nameof(cellData));
        }

        lock (_lock)
        {
            if (row < 0)
            {
                throw new IndexOutOfRangeException("Table row index cannot be negative.");
            }
            else if (row >= _list.Count)
            {
                throw new IndexOutOfRangeException("Table row index cannot exceed the number of rows in the table.");
            }

            var tableRow = _list.ElementAt(row);
            var currentRenderables = tableRow.ToList();

            if (column < 0)
            {
                throw new IndexOutOfRangeException("Table column index cannot be negative.");
            }
            else if (column >= currentRenderables.Count)
            {
                throw new IndexOutOfRangeException("Table column index cannot exceed the number of rows in the table.");
            }

            currentRenderables.RemoveAt(column);

            currentRenderables.Insert(column, cellData);

            var newTableRow = new TableRow(currentRenderables);

            _list.RemoveAt(row);

            _list.Insert(row, newTableRow);
        }
    }

    /// <summary>
    /// Removes a row at the specified index.
    /// </summary>
    /// <param name="index">The index to remove a row at.</param>
    public void RemoveAt(int index)
    {
        lock (_lock)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("Table row index cannot be negative.");
            }
            else if (index >= _list.Count)
            {
                throw new IndexOutOfRangeException("Table row index cannot exceed the number of rows in the table.");
            }

            _list.RemoveAt(index);
        }
    }

    /// <summary>
    /// Clears all rows.
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _list.Clear();
        }
    }

    /// <inheritdoc/>
    public IEnumerator<TableRow> GetEnumerator()
    {
        lock (_lock)
        {
            var items = new TableRow[_list.Count];
            _list.CopyTo(items, 0);
            return new TableRowEnumerator(items);
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private TableRow CreateRow(IEnumerable<IRenderable> columns)
    {
        var row = new TableRow(columns);

        if (row.Count > _table.Columns.Count)
        {
            throw new InvalidOperationException("The number of row columns are greater than the number of table columns.");
        }

        // Need to add missing columns
        if (row.Count < _table.Columns.Count)
        {
            var diff = _table.Columns.Count - row.Count;
            Enumerable.Range(0, diff).ForEach(_ => row.Add(Text.Empty));
        }

        return row;
    }
}