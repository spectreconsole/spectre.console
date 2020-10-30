using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        internal static string ReadLine(this IAnsiConsole console, Style? style, bool secret)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            style ??= Style.Plain;

            var result = string.Empty;
            while (true)
            {
                var key = console.Input.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    return result;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (result.Length > 0)
                    {
                        result = result.Substring(0, result.Length - 1);
                        console.Write("\b \b");
                    }

                    continue;
                }

                result += key.KeyChar.ToString();

                if (!char.IsControl(key.KeyChar))
                {
                    console.Write(secret ? "*" : key.KeyChar.ToString(), style);
                }
            }
        }
    }
}
