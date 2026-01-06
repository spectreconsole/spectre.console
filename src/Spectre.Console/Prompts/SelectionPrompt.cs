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
        var converter = Converter ?? TypeConverterHelper.ConvertToString;
        var result = await prompt.Show(_tree, converter, Mode, true, SearchEnabled, PageSize, WrapAround, cancellationToken).ConfigureAwait(false);

        // Return the selected item
        return result.Items[result.Index].Data;
    }

    /// <inheritdoc/>
    ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
    {
        if (key.Key == ConsoleKey.Enter
         || key.Key == ConsoleKey.Packet
         || (!state.SearchEnabled && key.Key == ConsoleKey.Spacebar))
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
}

/// <summary>
/// Contains extension methods for <see cref="SelectionPrompt{T}"/>.
/// </summary>
public static class SelectionPromptExtensions
{
    /// <summary>
    /// Adds multiple choices.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoices<T>(
        this SelectionPrompt<T> obj,
        params T[] choices) where T : notnull
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        ArgumentNullException.ThrowIfNull(obj);

        foreach (var choice in choices)
        {
            obj.AddChoice(choice);
        }

        return obj;
    }

    /// <summary>
    /// Adds multiple grouped choices.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    /// <param name="obj">The prompt.</param>
    /// <param name="group">The group.</param>
    /// <param name="choices">The choices to add.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static SelectionPrompt<T> AddChoiceGroup<T>(
        this SelectionPrompt<T> obj,
        T group, params T[] choices) where T : notnull
    {
        // TODO: This is here temporary due to a bug in the .NET SDK
        // See issue: https://github.com/dotnet/roslyn/issues/80024

        ArgumentNullException.ThrowIfNull(obj);

        var root = obj.AddChoice(group);
        foreach (var choice in choices)
        {
            root.AddChild(choice);
        }

        return obj;
    }

    /// <param name="obj">The prompt.</param>
    /// <typeparam name="T">The prompt result type.</typeparam>
    extension<T>(SelectionPrompt<T> obj) where T : notnull
    {
        /// <summary>
        /// Sets the selection mode.
        /// </summary>
        /// <param name="mode">The selection mode.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> Mode(SelectionMode mode)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Mode = mode;
            return obj;
        }

        /// <summary>
        /// Adds multiple choices.
        /// </summary>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> AddChoices(IEnumerable<T> choices)
        {
            ArgumentNullException.ThrowIfNull(obj);

            foreach (var choice in choices)
            {
                obj.AddChoice(choice);
            }

            return obj;
        }

        /// <summary>
        /// Adds multiple grouped choices.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> AddChoiceGroup(T group, IEnumerable<T> choices)
        {
            ArgumentNullException.ThrowIfNull(obj);

            var root = obj.AddChoice(group);
            foreach (var choice in choices)
            {
                root.AddChild(choice);
            }

            return obj;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <param name="title">The title markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> Title(string? title)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Title = title;
            return obj;
        }

        /// <summary>
        /// Sets how many choices that are displayed to the user.
        /// </summary>
        /// <param name="pageSize">The number of choices that are displayed to the user.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> PageSize(int pageSize)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (pageSize <= 2)
            {
                throw new ArgumentException("Page size must be greater or equal to 3.", nameof(pageSize));
            }

            obj.PageSize = pageSize;
            return obj;
        }

        /// <summary>
        /// Sets whether the selection should wrap around when reaching its edges.
        /// </summary>
        /// <param name="shouldWrap">Whether the selection should wrap around.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> WrapAround(bool shouldWrap = true)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.WrapAround = shouldWrap;
            return obj;
        }

        /// <summary>
        /// Enables search for the prompt.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> EnableSearch()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.SearchEnabled = true;
            return obj;
        }

        /// <summary>
        /// Disables search for the prompt.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> DisableSearch()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.SearchEnabled = false;
            return obj;
        }

        /// <summary>
        /// Sets the text that will be displayed when no search text has been entered.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> SearchPlaceholderText(string? text)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.SearchPlaceholderText = text;
            return obj;
        }

        /// <summary>
        /// Sets the highlight style of the selected choice.
        /// </summary>
        /// <param name="highlightStyle">The highlight style of the selected choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> HighlightStyle(Style highlightStyle)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.HighlightStyle = highlightStyle;
            return obj;
        }

        /// <summary>
        /// Sets the text that will be displayed if there are more choices to show.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> MoreChoicesText(string? text)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.MoreChoicesText = text;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice.
        /// </summary>
        /// <param name="displaySelector">The function to get a display string for a given choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public SelectionPrompt<T> UseConverter(Func<T, string>? displaySelector)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Converter = displaySelector;
            return obj;
        }
    }
}