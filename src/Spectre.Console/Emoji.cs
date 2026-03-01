namespace Spectre.Console;

/// <summary>
/// Utility for working with emojis.
/// </summary>
public static partial class Emoji
{
    private static readonly Dictionary<string, string> _remappings;

    static Emoji()
    {
        _remappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Remaps a specific emoji tag with a new emoji.
    /// </summary>
    /// <param name="tag">The emoji tag.</param>
    /// <param name="emoji">The emoji.</param>
    public static void Remap(string tag, string emoji)
    {
        ArgumentNullException.ThrowIfNull(tag);
        ArgumentNullException.ThrowIfNull(emoji);

        tag = tag.TrimStart(':').TrimEnd(':');
        emoji = emoji.TrimStart(':').TrimEnd(':');

        _remappings[tag] = emoji;
    }

    /// <summary>
    /// Replaces emoji markup with corresponding unicode characters.
    /// </summary>
    /// <param name="value">A string with emojis codes, e.g. "Hello :smiley:!".</param>
    /// <returns>A string with emoji codes replaced with actual emoji.</returns>
    public static string Replace(string value)
    {
        var colonPos = value.IndexOf(':');
        if (colonPos == -1)
        {
            // No colons, no emoji. return what was passed in with no changes.
            return value;
        }

        var span = value.AsSpan();
        StringBuilder? output = null;

        var index = colonPos + 1;
        int nextColonPos;
        while ((nextColonPos = span.IndexOf(':', index)) != -1)
        {
            var emojiKey = span.Slice(index, nextColonPos - index).ToString();
            if (TryGetEmoji(emojiKey, out var emojiValue))
            {
                output ??= new StringBuilder();
                output.AppendSpan(span[..(index - 1)]);
                output.Append(emojiValue);

                span = span.Slice(nextColonPos + 1);
                index = 0;
            }
            else
            {
                index = nextColonPos + 1;
            }
        }

        if (output == null)
        {
            return value;
        }

        output.AppendSpan(span);
        return output.ToString();
    }

    private static bool TryGetEmoji(string emoji, out string value)
    {
        if (_remappings.TryGetValue(emoji, out var remappedEmojiValue))
        {
            value = remappedEmojiValue;
            return true;
        }

        if (_emojis.TryGetValue(emoji, out var emojiValue))
        {
            value = emojiValue;
            return true;
        }

        value = string.Empty;
        return false;
    }

    private static int IndexOf(this ReadOnlySpan<char> span, char value, int startIndex)
    {
        var indexInSlice = span.Slice(startIndex).IndexOf(value);

        if (indexInSlice == -1)
        {
            return -1;
        }

        return startIndex + indexInSlice;
    }
}