namespace Spectre.Console;

internal sealed class ListPromptState<T>
    where T : notnull
{
    public int Index { get; private set; }
    public int ItemCount => Items.Count;
    public int PageSize { get; }
    public bool WrapAround { get; }
    public IReadOnlyList<ListPromptItem<T>> Items { get; }

    public ListPromptItem<T> Current => Items[Index];

    public ListPromptState(IReadOnlyList<ListPromptItem<T>> items, int pageSize, bool wrapAround)
    {
        Index = 0;
        Items = items;
        PageSize = pageSize;
        WrapAround = wrapAround;
    }

    public bool Update(ConsoleKey key)
    {
        var index = key switch
        {
            ConsoleKey.UpArrow => Index - 1,
            ConsoleKey.DownArrow => Index + 1,
            ConsoleKey.Home => 0,
            ConsoleKey.End => ItemCount - 1,
            ConsoleKey.PageUp => Index - PageSize,
            ConsoleKey.PageDown => Index + PageSize,
            _ => Index,
        };

        index = WrapAround
            ? (ItemCount + (index % ItemCount)) % ItemCount
            : index.Clamp(0, ItemCount - 1);
        if (index != Index)
        {
            Index = index;
            return true;
        }

        return false;
    }
}