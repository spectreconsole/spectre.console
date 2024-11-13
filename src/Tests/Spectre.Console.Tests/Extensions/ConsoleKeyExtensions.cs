namespace Spectre.Console.Tests;

public static class ConsoleKeyExtensions
{
    public static ConsoleKeyInfo ToConsoleKeyInfo(this ConsoleKey key)
    {
        var ch = (char)key;
        if (char.IsControl(ch))
        {
            ch = '\0';
        }

        return new ConsoleKeyInfo(ch, key, false, false, false);
    }
}