namespace Spectre.Console;

/// <summary>
/// Represents a console backend.
/// </summary>
internal interface IAnsiConsoleBackend
{
    IAnsiConsoleCursor Cursor { get; }

    void Clear(bool home);
    void Write(IRenderable renderable);
    void Write(Action<AnsiWriter> action);
}