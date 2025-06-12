namespace Spectre.Console;

internal sealed class ListPromptRenderHook<T> : IRenderHook
    where T : notnull
{
    private readonly IAnsiConsole _console;
    private readonly Func<IRenderable> _builder;
    private readonly LiveRenderable _live;
    private readonly object _lock;
    private bool _dirty;
    private Size _size;

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

            // check if the size of the renderable decreased
            var size = options.ConsoleSize;
            if (size.Width >= _size.Width && size.Height >= _size.Height)
            {
                yield return _live.PositionCursor();
            }
            else
            {
                // clear shape to ensure new size calculations
                _live.ClearShape();

                // render a clear screen
                yield return new ControlCode(AnsiSequences.ED(2));
                yield return new ControlCode(AnsiSequences.ED(3));
                yield return new ControlCode(AnsiSequences.CUP(1, 1));
            }

            // store new size
            _size = size;

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _live;
        }
    }
}