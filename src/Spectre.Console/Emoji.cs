using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spectre.Console
{
    /// <summary>
    /// Utility for working with emojis.
    /// </summary>
    public static partial class Emoji
    {
        private static readonly Regex _emojiCode = new Regex(@"(:(\S*?):)", RegexOptions.Compiled);
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

        /// <summary>
        /// Replaces emoji markup with corresponding unicode characters.
        /// </summary>
        /// <param name="value">A string with emojis codes, e.g. "Hello :smiley:!".</param>
        /// <returns>A string with emoji codes replaced with actual emoji.</returns>
        public static string Replace(string value)
        {
            static string ReplaceEmoji(Match match)
            {
                var key = match.Groups[2].Value;

                if (_remappings.Count > 0 && _remappings.TryGetValue(key, out var remappedEmoji))
                {
                    return remappedEmoji;
                }

                if (_emojis.TryGetValue(key, out var emoji))
                {
                    return emoji;
                }

                return match.Value;
            }

            return _emojiCode.Replace(value, ReplaceEmoji);
        }
    }
}
