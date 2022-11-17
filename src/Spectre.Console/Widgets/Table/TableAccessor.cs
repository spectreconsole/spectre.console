namespace Spectre.Console;

internal abstract class TableAccessor
{
    private readonly Table _table;

    public RenderOptions Options { get; }
    public IReadOnlyList<TableColumn> Columns => _table.Columns;
    public virtual IReadOnlyList<TableRow> Rows => _table.Rows;
    public bool Expand => _table.Expand || _table.Width != null;

    protected TableAccessor(Table table, RenderOptions options)
    {
        _table = table ?? throw new ArgumentNullException(nameof(table));
        Options = options ?? throw new ArgumentNullException(nameof(options));
    }
}