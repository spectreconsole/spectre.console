namespace Spectre.Console;

/// <summary>
/// Represents a strategy for a list prompt.
/// </summary>
/// <typeparam name="T">The list data type.</typeparam>
internal interface IListPromptStrategy<T>
    where T : notnull
{
    /// <summary>
    /// Handles any input received from the user.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    /// <param name="state">The current state.</param>
    /// <returns>A result representing an action.</returns>
    ListPromptInputResult HandleInput(ConsoleKeyInfo key, ListPromptState<T> state);

    /// <summary>
    /// Calculates the state's initial index.
    /// </summary>
    /// <param name="nodes">The nodes that will be shown in the list.</param>
    /// <returns>The initial index that should be used.</returns>
    public int CalculateInitialIndex(IReadOnlyList<ListPromptItem<T>> nodes);

    /// <summary>
    /// Calculates the page size.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="totalItemCount">The total number of items.</param>
    /// <param name="requestedPageSize">The requested number of items to show.</param>
    /// <returns>The page size that should be used.</returns>
    public int CalculatePageSize(IAnsiConsole console, int totalItemCount, int requestedPageSize);

    /// <summary>
    /// Builds a <see cref="IRenderable"/> from the current state.
    /// </summary>
    /// <param name="console">The console.</param>
    /// <param name="scrollable">Whether or not the list is scrollable.</param>
    /// <param name="cursorIndex">The cursor index.</param>
    /// <param name="items">The visible items.</param>
    /// <param name="skipUnselectableItems">A value indicating whether or not the prompt should skip unselectable items.</param>
    /// <param name="searchText">The search text.</param>
    /// <returns>A <see cref="IRenderable"/> representing the items.</returns>
    public IRenderable Render(IAnsiConsole console, bool scrollable, int cursorIndex,
        IEnumerable<(int Index, ListPromptItem<T> Node)> items, bool skipUnselectableItems, string searchText);
}