using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        internal static string ReadLine(this IAnsiConsole console, Style? style, bool secret, IEnumerable<string>? items = null)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            style ??= Style.Plain;
            var text = string.Empty;

            var autocomplete = new List<string>(items ?? Enumerable.Empty<string>());

            while (true)
            {
                var key = console.Input.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    return text;
                }

                if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
                {
                    var replace = AutoComplete(autocomplete, text);
                    if (!string.IsNullOrEmpty(replace))
                    {
                        // Render the suggestion
                        console.Write("\b \b".Repeat(text.Length), style);
                        console.Write(replace);
                        text = replace;
                        continue;
                    }
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (text.Length > 0)
                    {
                        text = text.Substring(0, text.Length - 1);
                        console.Write("\b \b");
                    }

                    continue;
                }

                if (!char.IsControl(key.KeyChar))
                {
                    text += key.KeyChar.ToString();
                    console.Write(secret ? "*" : key.KeyChar.ToString(), style);
                }
            }
        }

        private static string AutoComplete(List<string> autocomplete, string text)
        {
            var found = autocomplete.Find(i => i == text);
            var replace = string.Empty;

            if (found == null)
            {
                // Get the closest match
                var next = autocomplete.Find(i => i.StartsWith(text, true, CultureInfo.InvariantCulture));
                if (next != null)
                {
                    replace = next;
                }
                else if (string.IsNullOrEmpty(text))
                {
                    // Use the first item
                    replace = autocomplete[0];
                }
            }
            else
            {
                // Get the next match
                var index = autocomplete.IndexOf(found) + 1;
                if (index >= autocomplete.Count)
                {
                    index = 0;
                }

                replace = autocomplete[index];
            }

            return replace;
        }
    }
}
