using System.Globalization;
using System.Text;

namespace Spectre.Console.Internal
{
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
                .Append(style.ToMarkup())
                .Append(']')
                .Append(value.EscapeMarkup())
                .Append("[/]");
            }

            return builder.Append(value);
        }
    }
}
