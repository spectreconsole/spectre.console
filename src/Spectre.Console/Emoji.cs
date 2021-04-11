using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.Console
{
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
            if (tag is null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            if (emoji is null)
            {
                throw new ArgumentNullException(nameof(emoji));
            }

            tag = tag.TrimStart(':').TrimEnd(':');
            emoji = emoji.TrimStart(':').TrimEnd(':');

            _remappings[tag] = emoji;
        }

#if NETSTANDARD2_0
        /// <summary>
        /// Replaces emoji markup with corresponding unicode characters.
        /// </summary>
        /// <param name="value">A string with emojis codes, e.g. "Hello :smiley:!".</param>
        /// <returns>A string with emoji codes replaced with actual emoji.</returns>
        public static string Replace(string value)
        {
            return Replace(value.AsSpan());
        }
#endif

        /// <summary>
        /// Replaces emoji markup with corresponding unicode characters.
        /// </summary>
        /// <param name="value">A string with emojis codes, e.g. "Hello :smiley:!".</param>
        /// <returns>A string with emoji codes replaced with actual emoji.</returns>
        public static string Replace(ReadOnlySpan<char> value)
        {
            var output = new StringBuilder();
            var colonPos = value.IndexOf(':');
            if (colonPos == -1)
            {
                // No colons, no emoji. return what was passed in with no changes.
                return value.ToString();
            }

            while ((colonPos = value.IndexOf(':')) != -1)
            {
                // Append text up to colon
                output.AppendSpan(value.Slice(0, colonPos));

                // Set value equal to that colon and the rest of the string
                value = value.Slice(colonPos);

                // Find colon after that. if no colon, break out
                var nextColonPos = value.IndexOf(':', 1);
                if (nextColonPos == -1)
                {
                    break;
                }

                // Get the emoji text minus the colons
                var emojiKey = value.Slice(1, nextColonPos - 1).ToString();
                if (TryGetEmoji(emojiKey, out var emojiValue))
                {
                    output.Append(emojiValue);
                    value = value.Slice(nextColonPos + 1);
                }
                else
                {
                    output.Append(':');
                    value = value.Slice(1);
                }
            }

            output.AppendSpan(value);
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
}