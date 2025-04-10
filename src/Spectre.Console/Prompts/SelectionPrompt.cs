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
    /// Gets or sets the text that will be displayed when no search text has been entered.
    /// </summary>
    public string? SearchPlaceholderText { get; set; }

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
    /// Gets or sets a value indicating whether or not search is enabled.
    /// </summary>
    public bool SearchEnabled { get; set; }

    /// <summary>
    /// Gets or sets the choice to show as selected when the prompt is first displayed.
    /// By default the first choice is selected.
    /// </summary>
    public T? DefaultValue { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectionPrompt{T}"/> class.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing items,
    /// or <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the item.
    /// </param>
    public SelectionPrompt(IEqualityComparer<T>? comparer = null)
    {
        _tree = new ListPromptTree<T>(comparer ?? EqualityComparer<T>.Default);
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
        var converter = Converter ?? TypeConverterHelper.ConvertToString;
        var result = await prompt.Show(_tree, converter, Mode, true, SearchEnabled, PageSize, WrapAround, cancellationToken).ConfigureAwait(false);

        // Return the selected item
        return result.Items[result.Index].Data;
    }

    /// <inheritdoc/>
    ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
    {
        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.Packet)
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

        var scrollable = totalItemCount > requestedPageSize;
        if (SearchEnabled || scrollable)
        {
            extra += 1;
        }

        if (SearchEnabled)
        {
            extra += 1;
        }

        if (scrollable)
        {
            extra += 1;
        }

        if (requestedPageSize > console.Profile.Height - extra)
        {
            return console.Profile.Height - extra;
        }

        return requestedPageSize;
    }

    /// <inheritdoc/>
    IRenderable IListPromptStrategy<T>.Render(IAnsiConsole console, bool scrollable, int cursorIndex,
        IEnumerable<(int Index, ListPromptItem<T> Node)> items, bool skipUnselectableItems, string searchText)
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

            if (searchText.Length > 0 && !(item.Node.IsGroup && Mode == SelectionMode.Leaf))
            {
                text = text.Highlight(searchText, searchHighlightStyle);
            }

            grid.AddRow(new Markup(indent + prompt + " " + text, style));
        }

        list.Add(grid);

        if (SearchEnabled || scrollable)
        {
            // Add padding
            list.Add(Text.Empty);
        }

        if (SearchEnabled)
        {
            list.Add(new Markup(
                searchText.Length > 0 ? searchText.EscapeMarkup() : SearchPlaceholderText ?? ListPromptConstants.SearchPlaceholderMarkup));
        }

        if (scrollable)
        {
            // (Move up and down to reveal more choices)
            list.Add(new Markup(MoreChoicesText ?? ListPromptConstants.MoreChoicesMarkup));
        }

        return new Rows(list);
    }

    /// <inheritdoc/>
    int IListPromptStrategy<T>.CalculateInitialIndex(IReadOnlyList<ListPromptItem<T>> nodes)
    {
        if (DefaultValue is not null)
        {
            return _tree.IndexOf(DefaultValue) ?? 0;
        }

        return 0;
    }
}