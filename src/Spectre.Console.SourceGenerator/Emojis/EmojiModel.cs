using System;
using System.Collections.Generic;
using System.Globalization;
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
    string Emoji,        // The emoji text
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
            var emoji = element.GetProperty("emoji").GetString()
                ?? throw new InvalidOperationException("Emoji has null emoji");

            // Skip anything that is not a single code point for now
            if (hexcode.Contains("-"))
            {
                continue;
            }

            // Transform hexcode to Unicode escape sequence
            var code = TransformToUnicode(hexcode);

            // If the emoji does not have the Emoji_Presentation=Yes property,
            // then it doesn't have a Variation Selector-16 suffix, and we should add it.
            var codepoint = int.Parse(hexcode, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            if (!HasEmojiPresentation(codepoint))
            {
                code += "\\uFE0F";
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

            emojis.Add(new EmojiModel(identifier, name, code, emoji, description));
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

    private static string TransformToUnicode(string hexcode)
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

    /// <summary>
    /// Returns <see langword="true"/> if the Unicode code point has the
    /// <c>Emoji_Presentation=Yes</c> property, meaning it defaults to wide emoji
    /// rendering without a Variation Selector-16 suffix.
    /// </summary>
    /// <remarks>
    /// Based on https://www.unicode.org/Public/16.0.0/ucd/emoji/emoji-data.txt.
    /// Code points not listed here that are still emoji have <c>Emoji_Presentation=No</c>
    /// and render as narrow text characters (1 cell) when FE0F is absent.
    /// </remarks>
    private static bool HasEmojiPresentation(int codePoint)
    {
        return codePoint switch
        {
            // BMP: Miscellaneous Technical
            0x231A or 0x231B => true,
            >= 0x23E9 and <= 0x23EC => true,
            0x23F0 or 0x23F3 => true,
            // BMP: Geometric Shapes
            0x25FD or 0x25FE => true,
            // BMP: Miscellaneous Symbols
            0x2614 or 0x2615 => true,
            >= 0x2648 and <= 0x2653 => true,
            0x267F or 0x2693 or 0x26A1 => true,
            0x26AA or 0x26AB => true,
            0x26BD or 0x26BE => true,
            0x26C4 or 0x26C5 or 0x26CE or 0x26D4 or 0x26EA => true,
            0x26F2 or 0x26F3 or 0x26F5 or 0x26FA or 0x26FD => true,
            // BMP: Dingbats
            0x2702 or 0x2705 => true,
            >= 0x2708 and <= 0x270D => true,
            0x270F or 0x2712 or 0x2714 or 0x2716 => true,
            0x271D or 0x2721 or 0x2728 => true,
            0x2733 or 0x2734 or 0x2744 or 0x2747 => true,
            0x274C or 0x274E => true,
            >= 0x2753 and <= 0x2755 => true,
            0x2757 => true,
            0x2763 or 0x2764 => true,
            >= 0x2795 and <= 0x2797 => true,
            0x27A1 or 0x27B0 or 0x27BF => true,
            // BMP: Supplemental Arrows / Misc Symbols
            0x2934 or 0x2935 => true,
            >= 0x2B05 and <= 0x2B07 => true,
            0x2B1B or 0x2B1C or 0x2B50 or 0x2B55 => true,
            // BMP: CJK Symbols
            0x3030 or 0x303D or 0x3297 or 0x3299 => true,
            // SMP: Mahjong, Playing Cards, Enclosed Ideographic
            0x1F004 or 0x1F0CF or 0x1F18E => true,
            >= 0x1F191 and <= 0x1F19A => true,
            >= 0x1F1E6 and <= 0x1F1FF => true,
            0x1F201 or 0x1F21A or 0x1F22F => true,
            >= 0x1F232 and <= 0x1F236 => true,
            >= 0x1F238 and <= 0x1F23A => true,
            0x1F250 or 0x1F251 => true,
            // SMP: Miscellaneous Symbols and Pictographs
            >= 0x1F300 and <= 0x1F320 => true,
            >= 0x1F32D and <= 0x1F335 => true,
            >= 0x1F337 and <= 0x1F37C => true,
            >= 0x1F37E and <= 0x1F393 => true,
            >= 0x1F3A0 and <= 0x1F3CA => true,
            >= 0x1F3CF and <= 0x1F3D3 => true,
            >= 0x1F3E0 and <= 0x1F3F0 => true,
            0x1F3F4 => true,
            >= 0x1F3F8 and <= 0x1F43E => true,
            0x1F440 => true,
            >= 0x1F442 and <= 0x1F4FC => true,
            >= 0x1F4FF and <= 0x1F53D => true,
            >= 0x1F54B and <= 0x1F54E => true,
            >= 0x1F550 and <= 0x1F567 => true,
            0x1F57A => true,
            0x1F595 or 0x1F596 => true,
            0x1F5A4 => true,
            >= 0x1F5FB and <= 0x1F64F => true,
            // SMP: Transport and Map Symbols
            >= 0x1F680 and <= 0x1F6C5 => true,
            0x1F6CC => true,
            >= 0x1F6D0 and <= 0x1F6D2 => true,
            >= 0x1F6D5 and <= 0x1F6D7 => true,
            >= 0x1F6DC and <= 0x1F6DF => true,
            0x1F6EB or 0x1F6EC => true,
            >= 0x1F6F4 and <= 0x1F6FC => true,
            // SMP: Geometric Shapes Extended, Chess Symbols
            >= 0x1F7E0 and <= 0x1F7EB => true,
            0x1F7F0 => true,
            // SMP: Supplemental Symbols and Pictographs
            >= 0x1F90C and <= 0x1F93A => true,
            >= 0x1F93C and <= 0x1F945 => true,
            >= 0x1F947 and <= 0x1F9FF => true,
            // SMP: Symbols and Pictographs Extended-A
            >= 0x1FA00 and <= 0x1FA53 => true,
            >= 0x1FA60 and <= 0x1FA6D => true,
            >= 0x1FA70 and <= 0x1FA7C => true,
            >= 0x1FA80 and <= 0x1FA89 => true,
            >= 0x1FA8F and <= 0x1FAC6 => true,
            >= 0x1FACE and <= 0x1FADC => true,
            >= 0x1FADF and <= 0x1FAE9 => true,
            >= 0x1FAF0 and <= 0x1FAF8 => true,
            _ => false,
        };
    }
}
