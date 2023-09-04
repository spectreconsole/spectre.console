namespace Spectre.Console;

/// <summary>
/// Represents a single list prompt.
/// </summary>
/// <typeparam name="T">The prompt result type.</typeparam>
public sealed class SelectionPrompt<T> : IPrompt<T>, IListPromptStrategy<T>
    where T : notnull
{
    private readonly ListPromptTree<T> _tree;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// Defaults to <c>10</c>.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets a value indicating whether the selection should wrap around when reaching the edge.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool WrapAround { get; set; } = false;

    /// <summary>
    /// Gets or sets the highlight style of the selected choice.
    /// </summary>
    public Style? HighlightStyle { get; set; }

    /// <summary>
    /// Gets or sets the style of a disabled choice.
    /// </summary>
    public Style? DisabledStyle { get; set; }

    /// <summary>
    /// Gets or sets the style of highlighted search matches.
    /// </summary>
    public Style? SearchHighlightStyle { get; set; }

    /// <summary>
    /// Gets or sets the converter to get the display string for a choice. By default
    /// the corresponding <see cref="TypeConverter"/> is used.
    /// </summary>
    public Func<T, string>? Converter { get; set; }

    /// <summary>
    /// Gets or sets the text that will be displayed if there are more choices to show.
    /// </summary>
    public string? MoreChoicesText { get; set; }

    /// <summary>
    /// Gets or sets the selection mode.
    /// Defaults to <see cref="SelectionMode.Leaf"/>.
    /// </summary>
    public SelectionMode Mode { get; set; } = SelectionMode.Leaf;

    /// <summary>
    /// Gets or sets a value indicating whether or not the search filter is enabled.
    /// </summary>
    public bool SearchFilterEnabled { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionPrompt{T}"/> class.
    /// </summary>
    public SelectionPrompt()
    {
        _tree = new ListPromptTree<T>(EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Adds a choice.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A <see cref="ISelectionItem{T}"/> so that multiple calls can be chained.</returns>
    public ISelectionItem<T> AddChoice(T item)
    {
        var node = new ListPromptItem<T>(item);
        _tree.Add(node);
        return node;
    }

    /// <inheritdoc/>
    public T Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        // Create the list prompt
        var prompt = new ListPrompt<T>(console, this);
        var result = await prompt.Show(_tree, Mode, SearchFilterEnabled, PageSize, WrapAround, cancellationToken).ConfigureAwait(false);

        // Return the selected item
        return result.Items[result.Index].Data;
    }

    /// <inheritdoc/>
    ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
    {
        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Packet)
        {
            // Selecting a non leaf in "leaf mode" is not allowed
            if (state.Current.IsGroup && Mode == SelectionMode.Leaf)
            {
                return ListPromptInputResult.None;
            }

            return ListPromptInputResult.Submit;
        }

        return ListPromptInputResult.None;
    }

    /// <inheritdoc/>
    int IListPromptStrategy<T>.CalculatePageSize(IAnsiConsole console, int totalItemCount, int requestedPageSize)
    {
        var extra = 0;

        if (Title != null)
        {
            // Title takes up two rows including a blank line
            extra += 2;
        }

        // Scrolling?
        if (totalItemCount > requestedPageSize)
        {
            // The scrolling instructions takes up two rows
            extra += 2;
        }

        if (SearchFilterEnabled)
        {
            // The search instructions takes up two rows
            extra += 2;
        }

        if (requestedPageSize > console.Profile.Height - extra)
        {
            return console.Profile.Height - extra;
        }

        return requestedPageSize;
    }

    /// <inheritdoc/>
    IRenderable IListPromptStrategy<T>.Render(IAnsiConsole console, bool scrollable, int cursorIndex,
        IEnumerable<(int Index, ListPromptItem<T> Node)> items, string stateSearchFilter)
    {
        var list = new List<IRenderable>();
        var disabledStyle = DisabledStyle ?? Color.Grey;
        var highlightStyle = HighlightStyle ?? Color.Blue;
        var searchHighlightStyle = SearchHighlightStyle ?? new Style(foreground: Color.Default, background: Color.Yellow, Decoration.Bold);

        if (Title != null)
        {
            list.Add(new Markup(Title));
        }

        var grid = new Grid();
        grid.AddColumn(new GridColumn().Padding(0, 0, 1, 0).NoWrap());

        if (Title != null)
        {
            grid.AddEmptyRow();
        }

        foreach (var item in items)
        {
            var current = item.Index == cursorIndex;
            var prompt = item.Index == cursorIndex ? ListPromptConstants.Arrow : new string(' ', ListPromptConstants.Arrow.Length);
            var style = item.Node.IsGroup && Mode == SelectionMode.Leaf
                ? disabledStyle
                : current ? highlightStyle : Style.Plain;

            var indent = new string(' ', item.Node.Depth * 2);

            var text = (Converter ?? TypeConverterHelper.ConvertToString)?.Invoke(item.Node.Data) ?? item.Node.Data.ToString() ?? "?";
            if (current)
            {
                text = text.RemoveMarkup().EscapeMarkup();
            }

            if (stateSearchFilter.Length > 0 && !(item.Node.IsGroup && Mode == SelectionMode.Leaf))
            {
                var index = text.IndexOf(stateSearchFilter, StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                {
                    var before = text.Substring(0, index);
                    var match = text.Substring(index, stateSearchFilter.Length);
                    var after = text.Substring(index + stateSearchFilter.Length);

                    text = new StringBuilder()
                        .Append(before)
                        .AppendWithStyle(searchHighlightStyle, match)
                        .Append(after)
                        .ToString();
                }
            }

            grid.AddRow(new Markup(indent + prompt + " " + text, style));
        }

        list.Add(grid);

        if (scrollable)
        {
            // (Move up and down to reveal more choices)
            list.Add(Text.Empty);
            list.Add(new Markup(MoreChoicesText ?? ListPromptConstants.MoreChoicesMarkup));
        }

        if (SearchFilterEnabled)
        {
            list.Add(Text.Empty);
            list.Add(new Markup(
                $"search: {(stateSearchFilter.Length > 0 ? stateSearchFilter.EscapeMarkup() : ListPromptConstants.SearchPlaceholderMarkup)}"));
        }

        return new Rows(list);
    }
}