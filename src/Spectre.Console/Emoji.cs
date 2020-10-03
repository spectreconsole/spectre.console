using System.Text.RegularExpressions;

namespace Spectre.Console
{
    /// <summary>
    /// Utility for working with emojis.
    /// </summary>
    public static partial class Emoji
    {
        private static readonly Regex _emojiCode = new Regex(@"(:(\S*?):)", RegexOptions.Compiled);

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
