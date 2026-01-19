namespace Spectre.Console;

internal sealed class AnsiConsoleCursor : IAnsiConsoleCursor
{
    private readonly AnsiConsoleBackend _backend;

    public AnsiConsoleCursor(AnsiConsoleBackend backend)
    {
        _backend = backend ?? throw new ArgumentNullException(nameof(backend));
    }

    public void Show(bool show)
    {
        if (show)
        {
            _backend.Write(w => w.ShowCursor());
        }
        else
        {
            _backend.Write(w => w.HideCursor());
        }
    }

    public void Move(CursorDirection direction, int steps)
    {
        if (steps == 0)
        {
            return;
        }

        switch (direction)
        {
            case CursorDirection.Up:
                _backend.Write(w => w.CursorUp(steps));
                break;
            case CursorDirection.Down:
                _backend.Write(w => w.CursorDown(steps));
                break;
            case CursorDirection.Right:
                _backend.Write(w => w.CursorRight(steps));
                break;
            case CursorDirection.Left:
                _backend.Write(w => w.CursorLeft(steps));
                break;
        }
    }

    public void SetPosition(int column, int line)
    {
        _backend.Write(w => w.CursorPosition(line, column));
    }
}