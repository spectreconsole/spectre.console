namespace Spectre.Console;

internal sealed class ListPromptState<T>
    where T : notnull
{
    private readonly Func<T, string> _converter;

    public int Index { get; private set; }
    public int ItemCount => Items.Count;
    public int PageSize { get; }
    public bool WrapAround { get; }
    public SelectionMode Mode { get; }
    public bool SkipUnselectableItems { get; private set; }
    public bool SearchEnabled { get; }
    public IReadOnlyList<ListPromptItem<T>> Items { get; }
    private readonly IReadOnlyList<int>? _leafIndexes;

    public ListPromptItem<T> Current => Items[Index];
    public string SearchText { get; private set; }

    public ListPromptState(
        IReadOnlyList<ListPromptItem<T>> items,
        Func<T, string> converter,
        int pageSize, bool wrapAround,
        SelectionMode mode,
        bool skipUnselectableItems,
        bool searchEnabled,
        int initialIndex)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        Items = items;
        PageSize = pageSize;
        WrapAround = wrapAround;
        Mode = mode;
        SkipUnselectableItems = skipUnselectableItems;
        SearchEnabled = searchEnabled;
        SearchText = string.Empty;

        if (SkipUnselectableItems && mode == SelectionMode.Leaf)
        {
            _leafIndexes =
                Items
                    .Select((item, index) => new { item, index })
                    .Where(x => !x.item.IsGroup)
                    .Select(x => x.index)
                    .ToList()
                    .AsReadOnly();

            if (_leafIndexes.Contains(initialIndex))
            {
                Index = initialIndex;
            }
            else
            {
                Index = _leafIndexes.FirstOrDefault();
            }
        }
        else
        {
            Index = initialIndex;
        }
    }

    public bool Update(ConsoleKeyInfo keyInfo)
    {
        var index = Index;
        if (SkipUnselectableItems && Mode == SelectionMode.Leaf)
        {
            Debug.Assert(_leafIndexes != null, nameof(_leafIndexes) + " != null");
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

        var search = SearchText;

        if (SearchEnabled)
        {
            // If is text input, append to search filter
            if (!char.IsControl(keyInfo.KeyChar))
            {
                search = SearchText + keyInfo.KeyChar;

                var item = Items.FirstOrDefault(x =>
                    _converter.Invoke(x.Data).Contains(search, StringComparison.OrdinalIgnoreCase)
                    && (!x.IsGroup || Mode != SelectionMode.Leaf));

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

                var item = Items.FirstOrDefault(x =>
                    _converter.Invoke(x.Data).Contains(search, StringComparison.OrdinalIgnoreCase) &&
                    (!x.IsGroup || Mode != SelectionMode.Leaf));

                if (item != null)
                {
                    index = Items.IndexOf(item);
                }
            }
        }

        index = WrapAround
            ? (ItemCount + (index % ItemCount)) % ItemCount
            : index.Clamp(0, ItemCount - 1);

        if (index != Index || SearchText != search)
        {
            Index = index;
            SearchText = search;
            return true;
        }

        return false;
    }
}