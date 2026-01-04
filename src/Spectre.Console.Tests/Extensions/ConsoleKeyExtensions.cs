namespace Spectre.Console.Tests;

public static class ConsoleKeyExtensions
{
    extension(ConsoleKey key)
    {
        public ConsoleKeyInfo ToConsoleKeyInfo()
        {
            var ch = (char)key;
            if (char.IsControl(ch))
            {
                ch = '\0';
            }

            return new ConsoleKeyInfo(ch, key, false, false, false);
        }
    }
}