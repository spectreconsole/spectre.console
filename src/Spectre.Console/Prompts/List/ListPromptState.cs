namespace Spectre.Console;

internal sealed class ListPromptState<T>
    where T : notnull
{
    public int Index { get; private set; }
    public int ItemCount => Items.Count;
    public int PageSize { get; }
    public bool WrapAround { get; }
    public SelectionMode Mode { get; }
    public IReadOnlyList<ListPromptItem<T>> Items { get; }
    private readonly IReadOnlyList<int> _leafIndexes;

    public ListPromptItem<T> Current => Items[Index];
    public string SearchFilter { get; set; }

    public ListPromptState(IReadOnlyList<ListPromptItem<T>> items, int pageSize, bool wrapAround, SelectionMode mode)
    {
        Items = items;
        PageSize = pageSize;
        WrapAround = wrapAround;
        Mode = mode;
        SearchFilter = string.Empty;

        _leafIndexes = Items
            .Select((item, index) => new { item, index })
            .Where(x => !x.item.IsGroup)
            .Select(x => x.index)
            .ToList()
            .AsReadOnly();

        Index = _leafIndexes.FirstOrDefault();
    }

    public bool Update(ConsoleKeyInfo keyInfo)
    {
        var index = Index;
        if (Mode == SelectionMode.Leaf)
        {
            var currentLeafIndex = _leafIndexes.IndexOf(index);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentLeafIndex > 0)
                    {
                        index = _leafIndexes[currentLeafIndex - 1];
                    }
                    else if (WrapAround)
                    {
                        index = _leafIndexes.LastOrDefault();
                    }

                    break;

                case ConsoleKey.DownArrow:
                    if (currentLeafIndex < _leafIndexes.Count - 1)
                    {
                        index = _leafIndexes[currentLeafIndex + 1];
                    }
                    else if (WrapAround)
                    {
                        index = _leafIndexes.FirstOrDefault();
                    }

                    break;

                case ConsoleKey.Home:
                    index = _leafIndexes.FirstOrDefault();
                    break;

                case ConsoleKey.End:
                    index = _leafIndexes.LastOrDefault();
                    break;

                case ConsoleKey.PageUp:
                    index = Math.Max(currentLeafIndex - PageSize, 0);
                    if (index < _leafIndexes.Count)
                    {
                        index = _leafIndexes[index];
                    }

                    break;

                case ConsoleKey.PageDown:
                    index = Math.Min(currentLeafIndex + PageSize, _leafIndexes.Count - 1);
                    if (index < _leafIndexes.Count)
                    {
                        index = _leafIndexes[index];
                    }

                    break;
            }
        }
        else
        {
            index = keyInfo.Key switch
            {
                ConsoleKey.UpArrow => Index - 1,
                ConsoleKey.DownArrow => Index + 1,
                ConsoleKey.Home => 0,
                ConsoleKey.End => ItemCount - 1,
                ConsoleKey.PageUp => Index - PageSize,
                ConsoleKey.PageDown => Index + PageSize,
                _ => Index,
            };
        }

        var search = SearchFilter;

        // If is text input, append to search filter
        if (!char.IsControl(keyInfo.KeyChar))
        {
            search = SearchFilter + keyInfo.KeyChar;
            var item = Items.FirstOrDefault(x => x.Data.ToString()?.Contains(search, StringComparison.OrdinalIgnoreCase) == true && (!x.IsGroup || Mode != SelectionMode.Leaf));
            if (item != null)
            {
                index = Items.IndexOf(item);
            }
        }

        if (keyInfo.Key == ConsoleKey.Backspace)
        {
            if (search.Length > 0)
            {
                search = search.Substring(0, search.Length - 1);
            }

            var item = Items.FirstOrDefault(x => x.Data.ToString()?.Contains(search, StringComparison.OrdinalIgnoreCase) == true && (!x.IsGroup || Mode != SelectionMode.Leaf));
            if (item != null)
            {
                index = Items.IndexOf(item);
            }
        }

        index = WrapAround
            ? (ItemCount + (index % ItemCount)) % ItemCount
            : index.Clamp(0, ItemCount - 1);

        if (index != Index || SearchFilter != search)
        {
            Index = index;
            SearchFilter = search;
            return true;
        }

        return false;
    }
}