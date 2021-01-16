using System;

namespace Spectre.Console.Cli
{
    internal static class CaseSensitivityExtensions
    {
        public static StringComparison GetStringComparison(this CaseSensitivity caseSensitivity, CommandPart part)
        {
            if (part == CommandPart.CommandName && (caseSensitivity & CaseSensitivity.Commands) == 0)
            {
                return StringComparison.OrdinalIgnoreCase;
            }
            else if (part == CommandPart.LongOption && (caseSensitivity & CaseSensitivity.LongOptions) == 0)
            {
                return StringComparison.OrdinalIgnoreCase;
            }

            return StringComparison.Ordinal;
        }

        public static StringComparer GetStringComparer(this CaseSensitivity caseSensitivity, CommandPart part)
        {
            if (part == CommandPart.CommandName && (caseSensitivity & CaseSensitivity.Commands) == 0)
            {
                return StringComparer.OrdinalIgnoreCase;
            }
            else if (part == CommandPart.LongOption && (caseSensitivity & CaseSensitivity.LongOptions) == 0)
            {
                return StringComparer.OrdinalIgnoreCase;
            }

            return StringComparer.Ordinal;
        }
    }
}
