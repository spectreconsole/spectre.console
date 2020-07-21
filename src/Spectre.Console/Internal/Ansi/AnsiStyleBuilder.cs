using System.Collections.Generic;

namespace Spectre.Console.Internal
{
    internal static class AnsiStyleBuilder
    {
        // TODO: Rewrite this to not yield
        public static IEnumerable<byte> GetAnsiCodes(Styles style)
        {
            if ((style & Styles.Bold) != 0)
            {
                yield return 1;
            }

            if ((style & Styles.Dim) != 0)
            {
                yield return 2;
            }

            if ((style & Styles.Italic) != 0)
            {
                yield return 3;
            }

            if ((style & Styles.Underline) != 0)
            {
                yield return 4;
            }

            if ((style & Styles.SlowBlink) != 0)
            {
                yield return 5;
            }

            if ((style & Styles.RapidBlink) != 0)
            {
                yield return 6;
            }

            if ((style & Styles.Invert) != 0)
            {
                yield return 7;
            }

            if ((style & Styles.Conceal) != 0)
            {
                yield return 8;
            }

            if ((style & Styles.Strikethrough) != 0)
            {
                yield return 9;
            }
        }
    }
}
