namespace Spectre.Console;

/// <summary>
/// Represents a multi selection list prompt.
/// </summary>
/// <typeparam name="T">The prompt result type.</typeparam>
public sealed class MultiSelectionPrompt<T> : IPrompt<List<T>>, IListPromptStrategy<T>
    where T : notnull
{
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
    /// Gets or sets the converter to get the display string for a choice. By default
    /// the corresponding <see cref="TypeConverter"/> is used.
    /// </summary>
    public Func<T, string>? Converter { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// at least one selection is required.
    /// </summary>
    public bool Required { get; set; } = true;

    /// <summary>
    /// Gets or sets the text that will be displayed if there are more choices to show.
    /// </summary>
    public string? MoreChoicesText { get; set; }

    /// <summary>
    /// Gets or sets the text that instructs the user of how to select items.
    /// </summary>
    public string? InstructionsText { get; set; }

    /// <summary>
    /// Gets or sets the selection mode.
    /// Defaults to <see cref="SelectionMode.Leaf"/>.
    /// </summary>
    public SelectionMode Mode { get; set; } = SelectionMode.Leaf;

    internal ListPromptTree<T> Tree { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiSelectionPrompt{T}"/> class.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing items,
    /// or <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the item.
    /// </param>
    public MultiSelectionPrompt(IEqualityComparer<T>? comparer = null)
    {
        Tree = new ListPromptTree<T>(comparer ?? EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Adds a choice.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>A <see cref="IMultiSelectionItem{T}"/> so that multiple calls can be chained.</returns>
    public IMultiSelectionItem<T> AddChoice(T item)
    {
        var node = new ListPromptItem<T>(item);
        Tree.Add(node);
        return node;
    }

    /// <inheritdoc/>
    public List<T> Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<List<T>> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        // Create the list prompt
        var prompt = new ListPrompt<T>(console, this);
        var converter = Converter ?? TypeConverterHelper.ConvertToString;
        var result = await prompt.Show(Tree, converter, Mode, false, false, PageSize, WrapAround, cancellationToken).ConfigureAwait(false);

        if (Mode == SelectionMode.Leaf)
        {
            return result.Items
                .Where(x => x.IsSelected && x.Children.Count == 0)
                .Select(x => x.Data)
                .ToList();
        }

        return result.Items
            .Where(x => x.IsSelected)
            .Select(x => x.Data)
            .ToList();
    }

    /// <summary>
    /// Returns all parent items of the given <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item for which to find the parents.</param>
    /// <returns>The parent items, or an empty list, if the given item has no parents.</returns>
    public IEnumerable<T> GetParents(T item)
    {
        var promptItem = Tree.Find(item);
        if (promptItem == null)
        {
            throw new ArgumentOutOfRangeException(nameof(item), "Item not found in tree.");
        }

        var parents = new List<ListPromptItem<T>>();
        while (promptItem.Parent != null)
        {
            promptItem = promptItem.Parent;
            parents.Add(promptItem);
        }

        return parents
            .ReverseEnumerable()
            .Select(x => x.Data);
    }

    /// <summary>
    /// Returns the parent item of the given <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item for which to find the parent.</param>
    /// <returns>The parent item, or <c>null</c> if the given item has no parent.</returns>
    public T? GetParent(T item)
    {
        return GetParents(item).LastOrDefault();
    }

    /// <inheritdoc/>
    ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            if (Required && state.Items.None(x => x.IsSelected))
            {
                // Selection not permitted
                return ListPromptInputResult.None;
            }

            // Submit
            return ListPromptInputResult.Submit;
        }

        if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.Packet)
        {
            var current = state.Items[state.Index];
            var select = !current.IsSelected;

            if (Mode == SelectionMode.Leaf)
            {
                // Select the node and all its children
                foreach (var item in current.Traverse(includeSelf: true))
                {
                    item.IsSelected = select;
                }

                // Visit every parent and evaluate if its selection
                // status need to be updated
                var parent = current.Parent;
                while (parent != null)
                {
                    parent.IsSelected = parent.Traverse(includeSelf: false).All(x => x.IsSelected);
                    parent = parent.Parent;
                }
            }
            else
            {
                current.IsSelected = !current.IsSelected;
            }

            // Refresh the list
            return ListPromptInputResult.Refresh;
        }

        return ListPromptInputResult.None;
    }

    /// <inheritdoc/>
    int IListPromptStrategy<T>.CalculatePageSize(IAnsiConsole console, int totalItemCount, int requestedPageSize)
    {
        // The instructions take up two rows including a blank line
        var extra = 2;
        if (Title != null)
        {
            // Title takes up two rows including a blank line
            extra += 2;
        }

        // Scrolling?
        if (totalItemCount > requestedPageSize)
        {
            // The scrolling instructions takes up one row
            extra++;
        }

        var pageSize = requestedPageSize;
        if (pageSize > console.Profile.Height - extra)
        {
            pageSize = console.Profile.Height - extra;
        }

        return pageSize;
    }

    /// <inheritdoc/>
    IRenderable IListPromptStrategy<T>.Render(IAnsiConsole console, bool scrollable, int cursorIndex,
        IEnumerable<(int Index, ListPromptItem<T> Node)> items, bool skipUnselectableItems, string searchText)
    {
        var list = new List<IRenderable>();
        var highlightStyle = HighlightStyle ?? Color.Blue;

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
            var style = current ? highlightStyle : Style.Plain;

            var indent = new string(' ', item.Node.Depth * 2);
            var prompt = item.Index == cursorIndex ? ListPromptConstants.Arrow : new string(' ', ListPromptConstants.Arrow.Length);

            var text = (Converter ?? TypeConverterHelper.ConvertToString)?.Invoke(item.Node.Data) ?? item.Node.Data.ToString() ?? "?";
            if (current)
            {
                text = text.RemoveMarkup().EscapeMarkup();
            }

            var checkbox = item.Node.IsSelected
                ? ListPromptConstants.GetSelectedCheckbox(item.Node.IsGroup, Mode, HighlightStyle)
                : ListPromptConstants.Checkbox;

            grid.AddRow(new Markup(indent + prompt + " " + checkbox + " " + text, style));
        }

        list.Add(grid);
        list.Add(Text.Empty);

        if (scrollable)
        {
            // There are more choices
            list.Add(new Markup(MoreChoicesText ?? ListPromptConstants.MoreChoicesMarkup));
        }

        // Instructions
        list.Add(new Markup(InstructionsText ?? ListPromptConstants.InstructionsMarkup));

        // Combine all items
        return new Rows(list);
    }

    /// <inheritdoc/>
    int IListPromptStrategy<T>.CalculateInitialIndex(IReadOnlyList<ListPromptItem<T>> nodes)
    {
        return 0;
    }
}