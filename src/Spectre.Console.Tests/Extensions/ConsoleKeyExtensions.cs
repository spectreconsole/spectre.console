namespace Spectre.Console.Tests;

public static class ConsoleKeyExtensions
{
    public static ConsoleKeyInfo ToConsoleKeyInfo(this ConsoleKey key)
    {
        var ch = key.GetKeyChar();

        return new ConsoleKeyInfo(ch, key, false, false, false);
    }

    private static char GetKeyChar(this ConsoleKey key)
    {
        if (key is ConsoleKey.UpArrow
            or ConsoleKey.DownArrow
            or ConsoleKey.LeftArrow
            or ConsoleKey.RightArrow
            or ConsoleKey.Home
            or ConsoleKey.End
            or ConsoleKey.PageUp
            or ConsoleKey.PageDown
            or ConsoleKey.Insert
            or ConsoleKey.Delete)
        {
            return '\0';
        }

        var ch = (char)key;
        return char.IsControl(ch) ? '\0' : ch;
    }
}