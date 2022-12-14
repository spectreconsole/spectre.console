namespace Spectre.Console;

internal sealed class ListPrompt<T>
    where T : notnull
{
    private readonly IAnsiConsole _console;
    private readonly IListPromptStrategy<T> _strategy;

    public ListPrompt(IAnsiConsole console, IListPromptStrategy<T> strategy)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public async Task<ListPromptState<T>> Show(
        ListPromptTree<T> tree,
        CancellationToken cancellationToken,
        int requestedPageSize = 15,
        bool wrapAround = false)
    {
        if (tree is null)
        {
            throw new ArgumentNullException(nameof(tree));
        }

        if (!_console.Profile.Capabilities.Interactive)
        {
            throw new NotSupportedException(
                "Cannot show selection prompt since the current " +
                "terminal isn't interactive.");
        }

        if (!_console.Profile.Capabilities.Ansi)
        {
            throw new NotSupportedException(
                "Cannot show selection prompt since the current " +
                "terminal does not support ANSI escape sequences.");
        }

        var nodes = tree.Traverse().ToList();
        var state = new ListPromptState<T>(nodes, _strategy.CalculatePageSize(_console, nodes.Count, requestedPageSize), wrapAround);
        var hook = new ListPromptRenderHook<T>(_console, () => BuildRenderable(state));

        using (new RenderHookScope(_console, hook))
        {
            _console.Cursor.Hide();
            hook.Refresh();

            while (true)
            {
                var rawKey = await _console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
                if (rawKey == null)
                {
                    continue;
                }

                var key = rawKey.Value;
                var result = _strategy.HandleInput(key, state);
                if (result == ListPromptInputResult.Submit)
                {
                    break;
                }

                if (state.Update(key.Key) || result == ListPromptInputResult.Refresh)
                {
                    hook.Refresh();
                }
            }
        }

        hook.Clear();
        _console.Cursor.Show();

        return state;
    }

    private IRenderable BuildRenderable(ListPromptState<T> state)
    {
        var pageSize = state.PageSize;
        var middleOfList = pageSize / 2;

        var skip = 0;
        var take = state.ItemCount;
        var cursorIndex = state.Index;

        var scrollable = state.ItemCount > pageSize;
        if (scrollable)
        {
            skip = Math.Max(0, state.Index - middleOfList);
            take = Math.Min(pageSize, state.ItemCount - skip);

            if (state.ItemCount - state.Index < middleOfList)
            {
                // Pointer should be below the end of the list
                var diff = middleOfList - (state.ItemCount - state.Index);
                skip -= diff;
                take += diff;
                cursorIndex = middleOfList + diff;
            }
            else
            {
                // Take skip into account
                cursorIndex -= skip;
            }
        }

        // Build the renderable
        return _strategy.Render(
            _console,
            scrollable, cursorIndex,
            state.Items.Skip(skip).Take(take)
                .Select((node, index) => (index, node)));
    }
}