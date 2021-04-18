using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Spectre.Console.Testing
{
    /// <summary>
    /// Contains extensions for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a new string with all lines trimmed of trailing whitespace.
        /// </summary>
        /// <param name="value">The string to trim.</param>
        /// <returns>A new string with all lines trimmed of trailing whitespace.</returns>
        public static string TrimLines(this string value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var result = new List<string>();
            foreach (var line in value.Split(new[] { '\n' }))
            {
                result.Add(line.TrimEnd());
            }

            return string.Join("\n", result);
        }

        /// <summary>
        /// Returns a new string with normalized line endings.
        /// </summary>
        /// <param name="value">The string to normalize line endings for.</param>
        /// <returns>A new string with normalized line endings.</returns>
        public static string NormalizeLineEndings(this string value)
        {
            if (value != null)
            {
                value = value.Replace("\r\n", "\n");
                return value.Replace("\r", string.Empty);
            }

            return string.Empty;
        }
    }
}
