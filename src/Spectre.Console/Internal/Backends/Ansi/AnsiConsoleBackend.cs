namespace Spectre.Console;

internal sealed class AnsiConsoleBackend : IAnsiConsoleBackend
{
    private readonly IAnsiConsole _console;
    private readonly AnsiWriter _writer;

    public IAnsiConsoleCursor Cursor { get; }
    public Capabilities Capabilities => _console.Profile.Capabilities;

    public AnsiConsoleBackend(IAnsiConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _writer = new AnsiWriter(_console.Profile.Out.Writer, _console.Profile.Capabilities);

        Cursor = new AnsiConsoleCursor(this);
    }

    public void Clear(bool home)
    {
        Write(w => w.EraseInDisplay(2));
        Write(w => w.ClearScrollback());

        if (home)
        {
            Write(w => w.CursorPosition(1, 1));
        }
    }

    public void Write(IRenderable renderable)
    {
        _writer.Write(_console, renderable);
    }

    public void Write(Action<AnsiWriter> action)
    {
        action(_writer);
    }
}