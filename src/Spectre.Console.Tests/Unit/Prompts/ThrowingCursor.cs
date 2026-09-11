namespace Spectre.Console.Tests.Unit;

internal sealed class ThrowingCursor : IAnsiConsoleCursor
{
    public List<string> Calls { get; } = [];

    public void Show(bool show)
    {
        Calls.Add(show ? "show" : "hide");

        if (show == false)
        {
            throw new InvalidOperationException("boom");
        }
    }

    public void SetPosition(int column, int line)
    {
        Calls.Add($"set:{column}:{line}");
    }

    public void Move(CursorDirection direction, int steps)
    {
        Calls.Add($"move:{direction}:{steps}");
    }
}
