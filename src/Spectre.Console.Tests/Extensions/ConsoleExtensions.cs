using System;

namespace Spectre.Console.Tests
{
    public static class ConsoleExtensions
    {
        public static void SetColor(this IAnsiConsole console, Color color, bool foreground)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            if (foreground)
            {
                console.Foreground = color;
            }
            else
            {
                console.Background = color;
            }
        }
    }
}
