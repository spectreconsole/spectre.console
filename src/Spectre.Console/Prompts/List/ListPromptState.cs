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

    public bool Update(ConsoleKey key, Func<T, string>? converter = null)
    {
        var index = Index;

        // if user presses a letter key
        if (key >= ConsoleKey.A && key <= ConsoleKey.Z)
        {
            var keyStruck = new string((char)key, 1);

            // find indexes of items that start with the letter
            int[] matchingIndexes = Items.Select((item, idx) => (ItemToString(item, converter), idx))
                                   .Where(k => k.Item1?.StartsWith(keyStruck, StringComparison.InvariantCultureIgnoreCase) ?? false)
                                   .Select(k => k.idx)
                                   .ToArray();

            // if there are items beginning with this letter
            if (matchingIndexes.Length > 0)
            {
                // is one of them currently selected?
                var currentlySelected = Array.IndexOf(matchingIndexes, index);

                if (currentlySelected == -1)
                {
                    // we are not currently selecting any item beginning with the struck key
                    // so jump to first item in list that begins with the letter
                    index = matchingIndexes[0];
                }
                else
                {
                    // cycle to next (circular)
                    index = matchingIndexes[(currentlySelected + 1) % matchingIndexes.Length];
                }
            }
        }

        index = key switch
        {
            ConsoleKey.UpArrow => index - 1,
            ConsoleKey.DownArrow => index + 1,
            ConsoleKey.Home => 0,
            ConsoleKey.End => ItemCount - 1,
            ConsoleKey.PageUp => index - PageSize,
            ConsoleKey.PageDown => index + PageSize,
            _ => index,
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

    private string ItemToString(ListPromptItem<T> item, Func<T, string>? converter)
    {
        return (converter ?? TypeConverterHelper.ConvertToString)?.Invoke(item.Data) ?? item.Data.ToString() ?? "?";
    }
}