namespace Spectre.Console;

internal sealed class ListPromptRenderHook<T> : IRenderHook
    where T : notnull
{
    private readonly IAnsiConsole _console;
    private readonly Func<IRenderable> _builder;
    private readonly LiveRenderable _live;
    private readonly object _lock;
    private bool _dirty;

    public ListPromptRenderHook(
        IAnsiConsole console,
        Func<IRenderable> builder)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));

        _live = new LiveRenderable(console);
        _lock = new object();
        _dirty = true;
    }

    public void Clear()
    {
        _console.Write(_live.RestoreCursor());
    }

    public void Refresh()
    {
        _dirty = true;
        _console.Write(new ControlCode(string.Empty));
    }

    public IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_lock)
        {
            if (!_live.HasRenderable || _dirty)
            {
                _live.SetRenderable(_builder());
                _dirty = false;
            }

            yield return _live.PositionCursor();

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _live;
        }
    }
}