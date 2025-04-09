namespace Spectre.Console;

internal sealed class ListPromptState<T>
    where T : notnull
{
    public IReadOnlyList<ListPromptItem<T>> Items { get; }
    public int PageSize { get; }
    public string SearchText { get; private set; }
    public List<ListPromptItem<T>> VisibleItems { get; private set; }
    public int Index => _selectableItems.Count == 0 ? 0 : _selectableItems[_selectableIndex].Index;
    public ListPromptItem<T>? Current => _selectableItems.Count == 0 ? null : _selectableItems[_selectableIndex].Item;

    private readonly Func<T, string> _converter;
    private readonly bool _skipUnselectableItems;
    private readonly bool _filterOnSearch;
    private readonly bool _wrapAround;
    private readonly SelectionMode _mode;
    private readonly bool _searchEnabled;
    private List<SelectableItem> _selectableItems;
    private int _selectableIndex;

    public ListPromptState(
        IReadOnlyList<ListPromptItem<T>> items,
        Func<T, string> converter,
        int pageSize,
        bool wrapAround,
        SelectionMode mode,
        bool skipUnselectableItems,
        bool searchEnabled,
        bool filterOnSearch)
    {
        Items = items;
        PageSize = pageSize;
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _skipUnselectableItems = skipUnselectableItems;
        _wrapAround = wrapAround;
        _mode = mode;
        _searchEnabled = searchEnabled;
        _filterOnSearch = filterOnSearch;

        SearchText = string.Empty;
        VisibleItems = Items.ToList();
        _selectableItems = GetSelectableItems();
        _selectableIndex = 0;
    }

    public bool Update(ConsoleKeyInfo keyInfo)
    {
        if (_searchEnabled)
        {
            if (!char.IsControl(keyInfo.KeyChar))
            {
                SearchText += keyInfo.KeyChar;
                if (_filterOnSearch)
                {
                    VisibleItems = FilterItemsBySearch();
                    _selectableItems = GetSelectableItems();
                    _selectableIndex = 0;
                }
                else
                {
                    var item = _selectableItems
                        .FirstOrDefault(x => MatchesSearch(x.Item));
                    if (item != null)
                    {
                        _selectableIndex = _selectableItems.IndexOf(item);
                    }
                }

                return true;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (SearchText.Length > 0)
                {
                    SearchText = SearchText[..^1];
                    if (_filterOnSearch)
                    {
                        VisibleItems = FilterItemsBySearch();
                        _selectableItems = GetSelectableItems();
                        _selectableIndex = 0;
                    }
                    else
                    {
                        var item = _selectableItems
                            .FirstOrDefault(x => MatchesSearch(x.Item));
                        if (item != null)
                        {
                            _selectableIndex = _selectableItems.IndexOf(item);
                        }
                    }

                    return true;
                }
            }
        }

        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (_selectableIndex > 0)
                {
                    _selectableIndex--;
                }
                else if (_wrapAround)
                {
                    _selectableIndex = _selectableItems.Count - 1;
                }

                return true;

            case ConsoleKey.DownArrow:
                if (_selectableIndex < _selectableItems.Count - 1)
                {
                    _selectableIndex++;
                }
                else if (_wrapAround)
                {
                    _selectableIndex = 0;
                }

                return true;

            case ConsoleKey.Home:
                _selectableIndex = 0;
                break;

            case ConsoleKey.End:
                _selectableIndex = _selectableItems.Count - 1;
                return true;

            case ConsoleKey.PageUp:
                var pageUpIndex = Index - PageSize;
                if (_wrapAround)
                {
                    pageUpIndex = (pageUpIndex + VisibleItems.Count) % VisibleItems.Count;
                }
                else
                {
                    pageUpIndex = Math.Max(pageUpIndex, 0);
                }

                _selectableIndex = _selectableItems.IndexOf(_selectableItems.First(x => x.Index >= pageUpIndex));
                return true;

            case ConsoleKey.PageDown:
                var pageDownIndex = Index + PageSize;
                if (_wrapAround)
                {
                    pageDownIndex %= VisibleItems.Count;
                }
                else
                {
                    pageDownIndex = Math.Min(pageDownIndex, VisibleItems.Count - 1);
                }

                _selectableIndex = _selectableItems.IndexOf(_selectableItems.First(x => x.Index >= pageDownIndex));
                return true;
        }

        return false;
    }

    private List<SelectableItem> GetSelectableItems()
    {
        var selectableItems = VisibleItems
            .Select((item, filteredIndex) => new SelectableItem(item, filteredIndex));

        if (_skipUnselectableItems && _mode == SelectionMode.Leaf)
        {
            selectableItems = selectableItems.Where(x => !x.Item.IsGroup);
        }

        return selectableItems.ToList();
    }

    private List<ListPromptItem<T>> FilterItemsBySearch()
    {
        return Items
            .Where(x => MatchesSearch(x) || x.Children.Any(MatchesSearch))
            .ToList();
    }

    private bool MatchesSearch(ListPromptItem<T> item) =>
        _converter.Invoke(item.Data).Contains(SearchText, StringComparison.OrdinalIgnoreCase);

    private class SelectableItem(ListPromptItem<T> item, int index)
    {
        public ListPromptItem<T> Item { get; } = item;
        public int Index { get; } = index;
    }
}