namespace Spectre.Console;

/// <summary>
/// A renderable grid.
/// </summary>
public sealed class Grid : JustInTimeRenderable, IExpandable, IAlignable
{
    private readonly ListWithCallback<GridColumn> _columns;
    private readonly ListWithCallback<GridRow> _rows;

    private bool _expand;
    private Justify? _alignment;
    private bool _padRightCell;

    /// <summary>
    /// Gets the grid columns.
    /// </summary>
    public IReadOnlyList<GridColumn> Columns => _columns;

    /// <summary>
    /// Gets the grid rows.
    /// </summary>
    public IReadOnlyList<GridRow> Rows => _rows;

    /// <inheritdoc/>
    public bool Expand
    {
        get => _expand;
        set => MarkAsDirty(() => _expand = value);
    }

    /// <inheritdoc/>
    [Obsolete("Use the Align widget instead. This property will be removed in a later release.")]
    public Justify? Alignment
    {
        get => _alignment;
        set => MarkAsDirty(() => _alignment = value);
    }

    /// <summary>
    /// Gets or sets the width of the grid.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Grid"/> class.
    /// </summary>
    public Grid()
    {
        _expand = false;
        _alignment = null;
        _columns = new ListWithCallback<GridColumn>(MarkAsDirty);
        _rows = new ListWithCallback<GridRow>(MarkAsDirty);
    }

    /// <summary>
    /// Adds a column to the grid.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Grid AddColumn()
    {
        AddColumn(new GridColumn());
        return this;
    }

    /// <summary>
    /// Adds a column to the grid.
    /// </summary>
    /// <param name="column">The column to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Grid AddColumn(GridColumn column)
    {
        ArgumentNullException.ThrowIfNull(column);

        if (_rows.Count > 0)
        {
            throw new InvalidOperationException("Cannot add new columns to grid with existing rows.");
        }

        // Only pad the most right cell if we've explicitly set a padding.
        _padRightCell = column.HasExplicitPadding;

        _columns.Add(column);

        return this;
    }

    /// <summary>
    /// Adds a new row to the grid.
    /// </summary>
    /// <param name="columns">The columns to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public Grid AddRow(params IRenderable[] columns)
    {
        ArgumentNullException.ThrowIfNull(columns);

        if (columns.Length > _columns.Count)
        {
            throw new InvalidOperationException("The number of row columns are greater than the number of grid columns.");
        }

        _rows.Add(new GridRow(columns));
        return this;
    }

    /// <inheritdoc/>
    protected override bool HasDirtyChildren()
    {
        return _columns.Any(c => ((IHasDirtyState)c).IsDirty);
    }

    /// <inheritdoc/>
    protected override IRenderable Build()
    {
        var table = new Table
        {
            Border = TableBorder.None,
            ShowHeaders = false,
            IsGrid = true,
            PadRightCell = _padRightCell,
            Width = Width,
        };

        foreach (var column in _columns)
        {
            table.AddColumn(new TableColumn(string.Empty)
            {
                Width = column.Width,
                NoWrap = column.NoWrap,
                Padding = column.Padding ?? new Padding(0, 0, 2, 0),
                Alignment = column.Alignment,
            });
        }

        foreach (var row in _rows)
        {
            table.AddRow(row);
        }

        return table;
    }
}

/// <summary>
/// Contains extension methods for <see cref="Grid"/>.
/// </summary>
public static class GridExtensions
{
    /// <param name="grid">The grid to add the column to.</param>
    extension(Grid grid)
    {
        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="count">The number of columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumns(int count)
        {
            ArgumentNullException.ThrowIfNull(grid);

            for (var index = 0; index < count; index++)
            {
                grid.AddColumn(new GridColumn());
            }

            return grid;
        }

        /// <summary>
        /// Adds a column to the grid.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddColumns(params GridColumn[] columns)
        {
            ArgumentNullException.ThrowIfNull(grid);

            ArgumentNullException.ThrowIfNull(columns);

            foreach (var column in columns)
            {
                grid.AddColumn(column);
            }

            return grid;
        }

        /// <summary>
        /// Adds an empty row to the grid.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddEmptyRow()
        {
            ArgumentNullException.ThrowIfNull(grid);

            var columns = new IRenderable[grid.Columns.Count];
            Enumerable.Range(0, grid.Columns.Count).ForEach(index => columns[index] = Text.Empty);
            grid.AddRow(columns);

            return grid;
        }

        /// <summary>
        /// Adds a new row to the grid.
        /// </summary>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid AddRow(params string[] columns)
        {
            ArgumentNullException.ThrowIfNull(grid);

            ArgumentNullException.ThrowIfNull(columns);

            grid.AddRow(columns.Select(column => new Markup(column)).ToArray());
            return grid;
        }

        /// <summary>
        /// Sets the grid width.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public Grid Width(int? width)
        {
            ArgumentNullException.ThrowIfNull(grid);

            grid.Width = width;
            return grid;
        }
    }
}