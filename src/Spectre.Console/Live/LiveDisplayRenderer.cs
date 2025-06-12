namespace Spectre.Console;

internal sealed class LiveDisplayRenderer : IRenderHook
{
    private readonly IAnsiConsole _console;
    private readonly LiveDisplayContext _context;
    private Size _size;
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
            // check if the size of the renderable decreased
            var size = options.ConsoleSize;
            if (size.Width >= _size.Width && size.Height >= _size.Height)
            {
                yield return _context.Live.PositionCursor();
            }
            else
            {
                // clear shape to ensure new size calculations
                _context.Live.ClearShape();

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

            yield return _context.Live;
        }
    }
}