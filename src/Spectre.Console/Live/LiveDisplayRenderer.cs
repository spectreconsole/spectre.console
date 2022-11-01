namespace Spectre.Console;

internal sealed class LiveDisplayRenderer : IRenderHook
{
    private readonly IAnsiConsole _console;
    private readonly LiveDisplayContext _context;

    public LiveDisplayRenderer(IAnsiConsole console, LiveDisplayContext context)
    {
        _console = console;
        _context = context;
    }

    public void Started()
    {
        _console.Cursor.Hide();
    }

    public void Completed(bool autoclear)
    {
        lock (_context.Lock)
        {
            if (autoclear)
            {
                _console.Write(_context.Live.RestoreCursor());
            }
            else
            {
                if (_context.Live.HasRenderable && _context.Live.DidOverflow)
                {
                    // Redraw the whole live renderable
                    _console.Write(_context.Live.RestoreCursor());
                    _context.Live.Overflow = VerticalOverflow.Visible;
                    _console.Write(_context.Live.Target);
                }

                _console.WriteLine();
            }

            _console.Cursor.Show();
        }
    }

    public IEnumerable<IRenderable> Process(RenderOptions options, IEnumerable<IRenderable> renderables)
    {
        lock (_context.Lock)
        {
            yield return _context.Live.PositionCursor();

            foreach (var renderable in renderables)
            {
                yield return renderable;
            }

            yield return _context.Live;
        }
    }
}