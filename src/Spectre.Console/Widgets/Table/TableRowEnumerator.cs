namespace Spectre.Console;

internal sealed class TableRowEnumerator : IEnumerator<TableRow>
{
    private readonly TableRow[] _items;
    private int _index;

    public TableRow Current => _items[_index];
    object? IEnumerator.Current => _items[_index];

    public TableRowEnumerator(TableRow[] items)
    {
        _items = items ?? throw new ArgumentNullException(nameof(items));
        _index = -1;
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        _index++;
        return _index < _items.Length;
    }

    public void Reset()
    {
        _index = -1;
    }
}