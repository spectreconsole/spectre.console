namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    // Cache whether or not internally normalized line endings
    // already are normalized. No reason to do yet another replace if it is.
    private static readonly bool _alreadyNormalized
        = Environment.NewLine.Equals("\n", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the cell width of the specified text.
    /// </summary>
    /// <param name="text">The text to get the cell width of.</param>
    /// <returns>The cell width of the text.</returns>
    public static int GetCellWidth(this string text)
    {
        return Cell.GetCellLength(text);
    }

    internal static string CapitalizeFirstLetter(this string? text, CultureInfo? culture = null)
    {
        if (text == null)
        {
            return string.Empty;
        }

        culture ??= CultureInfo.InvariantCulture;

        if (text.Length > 0 && char.IsLower(text[0]))
        {
            text = string.Format(culture, "{0}{1}", char.ToUpper(text[0], culture), text.Substring(1));
        }

        return text;
    }

    internal static string? RemoveNewLines(this string? text)
    {
        return text?.ReplaceExact("\r\n", string.Empty)
            ?.ReplaceExact("\n", string.Empty);
    }

    internal static string NormalizeNewLines(this string? text, bool native = false)
    {
        text = text?.ReplaceExact("\r\n", "\n");
        text ??= string.Empty;

        if (native && !_alreadyNormalized)
        {
            text = text.ReplaceExact("\n", Environment.NewLine);
        }

        return text;
    }

    internal static string[] SplitLines(this string text)
    {
        var result = text?.NormalizeNewLines()?.Split(['\n'], StringSplitOptions.None);
        return result ?? [];
    }

    internal static string[] SplitWords(this string word, StringSplitOptions options = StringSplitOptions.None)
    {
        var result = new List<string>();

        static string Read(StringBuffer reader, Func<char, bool> criteria)
        {
            var buffer = new StringBuilder();
            while (!reader.Eof)
            {
                var current = reader.Peek();
                if (!criteria(current))
                {
                    break;
                }

                buffer.Append(reader.Read());
            }

            return buffer.ToString();
        }

        using (var reader = new StringBuffer(word))
        {
            while (!reader.Eof)
            {
                var current = reader.Peek();
                if (char.IsWhiteSpace(current))
                {
                    var x = Read(reader, c => char.IsWhiteSpace(c));
                    if (options != StringSplitOptions.RemoveEmptyEntries)
                    {
                        result.Add(x);
                    }
                }
                else
                {
                    result.Add(Read(reader, c => !char.IsWhiteSpace(c)));
                }
            }
        }

        return result.ToArray();
    }

    internal static string Repeat(this string text, int count)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (count <= 0)
        {
            return string.Empty;
        }

        if (count == 1)
        {
            return text;
        }

        return string.Concat(Enumerable.Repeat(text, count));
    }

    internal static string ReplaceExact(this string text, string oldValue, string? newValue)
    {
#if NETSTANDARD2_0
        return text.Replace(oldValue, newValue);
#else
        return text.Replace(oldValue, newValue, StringComparison.Ordinal);
#endif
    }

    internal static bool ContainsExact(this string text, string value)
    {
#if NETSTANDARD2_0
        return text.Contains(value);
#else
        return text.Contains(value, StringComparison.Ordinal);
#endif
    }

#if NETSTANDARD2_0
    internal static bool Contains(this string target, string value, System.StringComparison comparisonType)
    {
        return target.IndexOf(value, comparisonType) != -1;
    }
#endif

    /// <summary>
    /// "Masks" every character in a string.
    /// </summary>
    /// <param name="value">String value to mask.</param>
    /// <param name="mask">Character to use for masking.</param>
    /// <returns>Masked string.</returns>
    public static string Mask(this string value, char? mask)
    {
        if (mask is null)
        {
            return string.Empty;
        }

        return new string(mask.Value, value.Length);
    }
}