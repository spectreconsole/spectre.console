namespace Spectre.Console;

internal static class StringBuilderExtensions
{
    extension(StringBuilder builder)
    {
        public StringBuilder AppendWithStyle(Style? style, int? value)
        {
            return AppendWithStyle(builder, style, value?.ToString(CultureInfo.InvariantCulture));
        }
    }

    extension(StringBuilder builder)
    {
        public StringBuilder AppendWithStyle(Style? style, string? value)
        {
            value ??= string.Empty;

            if (style != null)
            {
                return builder.Append('[')
                    .Append(style.ToMarkup())
                    .Append(']')
                    .Append(value.EscapeMarkup())
                    .Append("[/]");
            }

            return builder.Append(value);
        }

        public void AppendSpan(ReadOnlySpan<char> span)
        {
            // NetStandard 2 lacks the override for StringBuilder to add the span. We'll need to convert the span
            // to a string for it, but for .NET 6.0 or newer we'll use the override.
#if NETSTANDARD2_0
            builder.Append(span.ToString());
#else
            builder.Append(span);
#endif
        }
    }
}