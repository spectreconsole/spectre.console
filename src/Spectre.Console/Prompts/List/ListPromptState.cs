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
        bool searchEnabled)
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

            Index = _leafIndexes.FirstOrDefault();
        }
        else
        {
            Index = 0;
        }
    }

    public bool Update(ConsoleKeyInfo keyInfo)
    {
        var index = Index;

        // Flag to indicate if a key press should prevent search text modification
        var keyHandledForNavigation = false;
        if (SkipUnselectableItems && Mode == SelectionMode.Leaf)
        {
            Debug.Assert(_leafIndexes != null, nameof(_leafIndexes) + " != null");
            var currentLeafIndex = _leafIndexes.IndexOf(index);
            switch (keyInfo.Key)
            {
                case ConsoleKey.K:
                    if (currentLeafIndex > 0)
                    {
                        index = _leafIndexes[currentLeafIndex - 1];
                    }
                    else if (WrapAround)
                    {
                        index = _leafIndexes.LastOrDefault();
                    }

                    break;

                case ConsoleKey.UpArrow:
                    if (currentLeafIndex > 0)
                    {
                        index = _leafIndexes[currentLeafIndex - 1];
                    }
                    else if (WrapAround)
                    {
                        index = _leafIndexes.LastOrDefault();
                    }

                    keyHandledForNavigation = true;
                    break;

                case ConsoleKey.J:
                    if (currentLeafIndex < _leafIndexes.Count - 1)
                    {
                        index = _leafIndexes[currentLeafIndex + 1];
                    }
                    else if (WrapAround)
                    {
                        index = _leafIndexes.FirstOrDefault();
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

                    keyHandledForNavigation = true;
                    break;

                case ConsoleKey.Home:
                    index = _leafIndexes.FirstOrDefault();
                    keyHandledForNavigation = true;
                    break;

                case ConsoleKey.End:
                    index = _leafIndexes.LastOrDefault();
                    keyHandledForNavigation = true;
                    break;

                case ConsoleKey.PageUp:
                    index = Math.Max(currentLeafIndex - PageSize, 0);
                    if (index < _leafIndexes.Count)
                    {
                        index = _leafIndexes[index];
                    }

                    keyHandledForNavigation = true;
                    break;

                case ConsoleKey.PageDown:
                    index = Math.Min(currentLeafIndex + PageSize, _leafIndexes.Count - 1);
                    if (index < _leafIndexes.Count)
                    {
                        index = _leafIndexes[index];
                    }

                    keyHandledForNavigation = true;
                    break;
            }
        }
        else
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.K:
                    index = Index - 1;
                    break;
                case ConsoleKey.UpArrow:
                    index = Index - 1;
                    keyHandledForNavigation = true;
                    break;
                case ConsoleKey.J:
                    index = Index + 1;
                    break;
                case ConsoleKey.DownArrow:
                    index = Index + 1;
                    keyHandledForNavigation = true;
                    break;
                case ConsoleKey.Home:
                    index = 0;
                    keyHandledForNavigation = true;
                    break;
                case ConsoleKey.End:
                    index = ItemCount - 1;
                    keyHandledForNavigation = true;
                    break;
                case ConsoleKey.PageUp:
                    index = Index - PageSize;
                    keyHandledForNavigation = true;
                    break;
                case ConsoleKey.PageDown:
                    index = Index + PageSize;
                    keyHandledForNavigation = true;
                    break;
            }
        }

        var search = SearchText; // Initialize search with current value

        if (SearchEnabled)
        {
            // IMPORTANT: Check if the key was already handled by navigation logic.
            // If it was, we prevent it from being processed as search input.
            if (keyHandledForNavigation)
            {
                // Do nothing, the 'index' has already been updated if necessary.
                // 'search' remains unchanged.
            }
            else if (keyInfo.Key == ConsoleKey.Tab && !string.IsNullOrEmpty(SearchText))
            {
                var matches = new List<int>(Items.Count);
                for (var i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];
                    var text = _converter(it.Data);
                    if ((text?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
                        && (!it.IsGroup || Mode != SelectionMode.Leaf))
                    {
                        matches.Add(i);
                    }
                }

                if (matches.Count > 0)
                {
                    var matchIndex = matches.IndexOf(Index);

                    if (matchIndex == -1)
                    {
                        index = matches[0];
                    }
                    else
                    {
                        var nextMatchIndex = (matchIndex + 1) % matches.Count;
                        index = matches[nextMatchIndex];
                    }
                }
            }
            else if (!char.IsControl(keyInfo.KeyChar))
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
