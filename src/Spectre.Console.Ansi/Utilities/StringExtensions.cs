namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Escapes text so that it won’t be interpreted as markup.
    /// </summary>
    /// <param name="text">The text to escape.</param>
    /// <returns>A string that is safe to use in markup.</returns>
    public static string EscapeMarkup(this string? text)
    {
        return AnsiMarkup.Escape(text);
    }

    /// <summary>
    /// Removes markup from the specified string.
    /// </summary>
    /// <param name="text">The text to remove markup from.</param>
    /// <returns>A string that does not have any markup.</returns>
    public static string RemoveMarkup(this string? text)
    {
        return AnsiMarkup.Remove(text);
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
}