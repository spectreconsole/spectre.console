using Spectre.Console;

namespace Cursor;

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Write("Hello");

        // Move the cursor 3 cells to the right
        AnsiConsole.Cursor.Move(CursorDirection.Right, 3);
        AnsiConsole.Write("World");

        // Move the cursor 5 cells to the left.
        AnsiConsole.Cursor.Move(CursorDirection.Left, 5);
        AnsiConsole.WriteLine("Universe");
    }
}
