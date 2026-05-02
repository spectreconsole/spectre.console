namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IAnsiConsole"/>.
/// </summary>
public static partial class AnsiConsoleExtensions
{
    internal static async Task<string> ReadLine(this IAnsiConsole console, Style? style, bool secret, char? mask, IEnumerable<string>? items = null, CancellationToken cancellationToken = default, string? initialInput = null, PromptHistory? history = null)
    {
        ArgumentNullException.ThrowIfNull(console);

        style ??= Style.Plain;
        var text = string.Empty;

        var autocomplete = new List<string>(items ?? []);
        var historyEntries = history?.Enabled == true ? history.Entries : Array.Empty<string>();
        var historyIndex = -1;
        string? pendingText = null;

        Queue<ConsoleKeyInfo>? injectedQueue = null;
        if (!string.IsNullOrEmpty(initialInput))
        {
            injectedQueue = new Queue<ConsoleKeyInfo>();
            foreach (var ch in initialInput)
            {
                var control = char.IsUpper(ch);
                injectedQueue.Enqueue(new ConsoleKeyInfo(ch, (ConsoleKey)ch, false, false, control));
            }
        }

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ConsoleKeyInfo? rawKey;
            if (injectedQueue != null && injectedQueue.Count > 0)
            {
                rawKey = injectedQueue.Dequeue();
            }
            else
            {
                rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
            }

            if (rawKey == null)
            {
                continue;
            }

            var key = rawKey.Value;
            if (key.Key == ConsoleKey.Enter)
            {
                return text;
            }

            if (key.Key == ConsoleKey.UpArrow && historyEntries.Count > 0)
            {
                if (historyIndex == -1)
                {
                    pendingText = text;
                    historyIndex = historyEntries.Count - 1;
                }
                else if (historyIndex > 0)
                {
                    historyIndex--;
                }

                var replace = historyEntries[historyIndex];
                ReplaceLine(console, style, text, replace, secret, mask);
                text = replace;
                continue;
            }

            if (key.Key == ConsoleKey.DownArrow && historyEntries.Count > 0)
            {
                if (historyIndex == -1)
                {
                    continue;
                }

                if (historyIndex < historyEntries.Count - 1)
                {
                    historyIndex++;
                    var replace = historyEntries[historyIndex];
                    ReplaceLine(console, style, text, replace, secret, mask);
                    text = replace;
                    continue;
                }

                var restore = pendingText ?? string.Empty;
                ReplaceLine(console, style, text, restore, secret, mask);
                text = restore;
                pendingText = null;
                historyIndex = -1;
                continue;
            }

            if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
            {
                var autoCompleteDirection = key.Modifiers.HasFlag(ConsoleModifiers.Shift)
                    ? AutoCompleteDirection.Backward
                    : AutoCompleteDirection.Forward;
                var replace = AutoComplete(autocomplete, text, autoCompleteDirection);
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
                historyIndex = -1;
                pendingText = null;

                if (text.Length > 0)
                {
                    var lastChar = text.Last();
                    text = text.Substring(0, text.Length - 1);

                    if (mask != null)
                    {
                        if (UnicodeCalculator.GetWidth(lastChar) == 1)
                        {
                            console.Write("\b \b");
                        }
                        else if (UnicodeCalculator.GetWidth(lastChar) == 2)
                        {
                            console.Write("\b \b\b \b");
                        }
                    }
                }

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                historyIndex = -1;
                pendingText = null;
                text += key.KeyChar.ToString();
                var output = key.KeyChar.ToString();
                console.Write(secret ? output.Mask(mask) : output, style);
            }
        }
    }

    private static string AutoComplete(List<string> autocomplete, string text, AutoCompleteDirection autoCompleteDirection)
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
            replace = GetAutocompleteValue(autoCompleteDirection, autocomplete, found);
        }

        return replace;
    }

    private static void ReplaceLine(IAnsiConsole console, Style? style, string currentText, string replace, bool secret, char? mask)
    {
        if (!string.IsNullOrEmpty(currentText) && !(secret && mask is null))
        {
            if (mask is null)
            {
                console.Write("\b \b".Repeat(currentText.Length), style);
            }
            else
            {
                foreach (var c in currentText)
                {
                    if (UnicodeCalculator.GetWidth(c) == 1)
                    {
                        console.Write("\b \b", style);
                    }
                    else if (UnicodeCalculator.GetWidth(c) == 2)
                    {
                        console.Write("\b \b\b \b", style);
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(replace) && !(secret && mask is null))
        {
            console.Write(secret ? replace.Mask(mask) : replace, style);
        }
    }

    private static string GetAutocompleteValue(AutoCompleteDirection autoCompleteDirection, IList<string> autocomplete, string found)
    {
        var foundAutocompleteIndex = autocomplete.IndexOf(found);
        var index = autoCompleteDirection switch
        {
            AutoCompleteDirection.Forward => foundAutocompleteIndex + 1,
            AutoCompleteDirection.Backward => foundAutocompleteIndex - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(autoCompleteDirection), autoCompleteDirection, null),
        };

        if (index >= autocomplete.Count)
        {
            index = 0;
        }

        if (index < 0)
        {
            index = autocomplete.Count - 1;
        }

        return autocomplete[index];
    }

    private enum AutoCompleteDirection
    {
        Forward,
        Backward,
    }
}