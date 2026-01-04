namespace Spectre.Console.Testing;

/// <summary>
/// Contains extensions for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <param name="value">The string to trim.</param>
    extension(string value)
    {
        /// <summary>
        /// Returns a new string with all lines trimmed of trailing whitespace.
        /// </summary>
        /// <returns>A new string with all lines trimmed of trailing whitespace.</returns>
        public string TrimLines()
        {
            if (value is null)
            {
                return string.Empty;
            }

            var result = new List<string>();
            foreach (var line in value.NormalizeLineEndings().Split(new[] { '\n' }))
            {
                result.Add(line.TrimEnd());
            }

            return string.Join("\n", result);
        }

        /// <summary>
        /// Returns a new string with normalized line endings.
        /// </summary>
        /// <returns>A new string with normalized line endings.</returns>
        public string NormalizeLineEndings()
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