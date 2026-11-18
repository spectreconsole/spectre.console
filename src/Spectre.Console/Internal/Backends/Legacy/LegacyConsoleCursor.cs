namespace Spectre.Console;

internal sealed class LegacyConsoleCursor : IAnsiConsoleCursor
{
    public void Show(bool show)
    {
        if (System.Console.IsOutputRedirected)
        {
            return;
        }

        System.Console.CursorVisible = show;
    }

    public void Move(CursorDirection direction, int steps)
    {
        if (System.Console.IsOutputRedirected)
        {
            return;
        }

        if (steps == 0)
        {
            return;
        }

        switch (direction)
        {
            case CursorDirection.Up:
                System.Console.CursorTop -= steps;
                break;
            case CursorDirection.Down:
                System.Console.CursorTop += steps;
                break;
            case CursorDirection.Left:
                System.Console.CursorLeft -= steps;
                break;
            case CursorDirection.Right:
                System.Console.CursorLeft += steps;
                break;
        }
    }

    public void SetPosition(int x, int y)
    {
        if (System.Console.IsOutputRedirected)
        {
            return;
        }

        System.Console.CursorLeft = x;
        System.Console.CursorTop = y;
    }
}