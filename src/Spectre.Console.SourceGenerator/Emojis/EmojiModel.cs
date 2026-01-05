using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Spectre.Console.SourceGenerator.Emojis;

/// <summary>
/// Represents an emoji from the JSON data file.
/// </summary>
internal sealed record EmojiModel(
    string Identifier,   // snake_case for dictionary lookup
    string Name,         // PascalCase for C# property
    string Code,         // Unicode escape sequence like \U0001F90E
    string Description)  // Human-readable description
    ;

/// <summary>
/// Parses emoji data from JSON.
/// </summary>
internal static class EmojiParser
{
    /// <summary>
    /// Parses the emoji.json file and returns all emoji models.
    /// Filters out emojis with multiple codepoints (combinators).
    /// </summary>
    public static EquatableArray<EmojiModel> ParseAll(string json)
    {
        var document = JsonDocument.Parse(json);
        var emojis = new List<EmojiModel>();

        foreach (var element in document.RootElement.EnumerateArray())
        {
            var label = element.GetProperty("label").GetString()
                ?? throw new InvalidOperationException("Emoji has null label");
            var hexcode = element.GetProperty("hexcode").GetString()
                ?? throw new InvalidOperationException("Emoji has null hexcode");

            // Skip multi-codepoint emojis (those with dashes in hexcode indicate combinators)
            if (hexcode.Contains("-"))
            {
                continue;
            }

            // Transform hexcode to Unicode escape sequence
            var code = TransformHexcode(hexcode);

            // Check if it has combinators (multiple escape sequences)
            var firstIndex = code.IndexOf("\\U", StringComparison.Ordinal);
            var hasCombinators = firstIndex >= 0 && code.IndexOf("\\U", firstIndex + 2, StringComparison.Ordinal) >= 0;
            if (hasCombinators)
            {
                continue;
            }

            // Transform label to identifier (snake_case)
            var identifier = TransformName(label);

            // Transform identifier to name (PascalCase)
            var name = identifier
                .Replace("1st", "first")
                .Replace("2nd", "second")
                .Replace("3rd", "third");
            name = Pascalize(name);

            // Create description
            var description = Humanize(label);

            emojis.Add(new EmojiModel(identifier, name, code, description));
        }

        // Sort by name for consistent output
        return new EquatableArray<EmojiModel>(emojis.OrderBy(e => e.Name));
    }

    private static string TransformName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var result = new StringBuilder(name.Length + 16); // Extra capacity for expansions like & -> and
        var i = 0;
        var len = name.Length;

        while (i < len)
        {
            var c = name[i];

            // Handle "'s" or "'s" -> "s" (straight or curly apostrophe followed by s)
            if ((c == '\'' || c == '\u2019') && i + 1 < len && name[i + 1] == 's')
            {
                result.Append('s');
                i += 2;
                continue;
            }

            switch (c)
            {
                // Skip these characters (remove them)
                case ':':
                case ',':
                case '.':
                case '!':
                case '(':
                case ')':
                case '\u201c': // left double quotation mark
                case '\u201d': // right double quotation mark
                case '\u229b': // circled asterisk operator
                    break;

                // Replace with underscore
                case ' ':
                case '\'':
                case '\u2019': // right single quotation mark (curly apostrophe)
                case '-':
                    result.Append('_');
                    break;

                // Multi-char expansions
                case '&':
                    result.Append("and");
                    break;
                case '#':
                    result.Append("hash");
                    break;
                case '*':
                    result.Append("star");
                    break;

                // Default: lowercase the character
                default:
                    result.Append(char.ToLowerInvariant(c));
                    break;
            }

            i++;
        }

        // Trim leading/trailing underscores (equivalent to original .Trim())
        var str = result.ToString();
        return str.Trim('_');
    }

    private static string TransformHexcode(string hexcode)
    {
        // Convert hexcode like "1F90E" to Unicode escape "\U0001F90E"
        // Pad to 8 characters for the \U escape format
        var paddedHex = hexcode.PadLeft(8, '0');
        return $"\\U{paddedHex}";
    }

    private static string Pascalize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var result = new StringBuilder();
        var capitalizeNext = true;

        foreach (var c in input)
        {
            if (c == '_')
            {
                capitalizeNext = true;
            }
            else if (char.IsLetterOrDigit(c))
            {
                result.Append(capitalizeNext ? char.ToUpperInvariant(c) : c);
                capitalizeNext = false;
            }
            else
            {
                capitalizeNext = true;
            }
        }

        return result.ToString();
    }

    private static string Humanize(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Capitalize the first letter
        var result = char.ToUpperInvariant(input[0]) + input.Substring(1);
        return result;
    }
}
