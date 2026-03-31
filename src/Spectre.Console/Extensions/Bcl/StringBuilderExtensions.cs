namespace Spectre.Console;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendWithStyle(this StringBuilder builder, Style? style, int? value)
    {
        return AppendWithStyle(builder, style, value?.ToString(CultureInfo.InvariantCulture));
    }

    public static StringBuilder AppendWithStyle(this StringBuilder builder, Style? style, string? value)
    {
        value ??= string.Empty;

        if (style != null)
        {
            return builder.Append('[')
                .Append(style.Value.ToMarkup())
                .Append(']')
                .Append(value.EscapeMarkup())
                .Append("[/]");
        }

        return builder.Append(value);
    }

    public static void AppendSpan(this StringBuilder builder, ReadOnlySpan<char> span)
    {
        // NetStandard 2 lacks the override for StringBuilder to add the span. We'll need to convert the span
        // to a string for it, but for .NET 6.0 or newer we'll use the override.
#if NETSTANDARD2_0
        builder.Append(span.ToString());
#else
        builder.Append(span);
#endif
    }

    public static int CountTrailing(this StringBuilder current, char[] characters)
    {
        var trailing = 0;
        for (var index = current.Length - 1;
             index >= 0 && characters.Any(c => current[index] == c);
             index--)
        {
            trailing++;
        }

        return trailing;
    }
}