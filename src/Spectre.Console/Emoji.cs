using System.Text.RegularExpressions;

namespace Spectre.Console
{
    /// <summary>
    /// Utility class for working with emojis.
    /// </summary>
    internal static partial class Emoji
    {
        private static readonly Regex _emojiCode = new Regex(@"(:(\S*?):)", RegexOptions.Compiled);

        /// <summary>
        /// Replaces emoji markup with corresponding unicode characters.
        /// </summary>
        /// <param name="value">A string with emojis codes, e.g. "Hello :smiley:!".</param>
        /// <returns>A string with emoji codes replaced with actual emoji.</returns>
        public static string Replace(string value)
        {
            static string ReplaceEmoji(Match match) => _emojis[match.Groups[2].Value];
            return _emojiCode.Replace(value, ReplaceEmoji);
        }
    }
}
