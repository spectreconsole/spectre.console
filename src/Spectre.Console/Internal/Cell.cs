using Spectre.Console.Rendering;
using Wcwidth;

namespace Spectre.Console
{
    internal static class Cell
    {
        public static int GetCellLength(RenderContext context, string text)
        {
            var sum = 0;
            foreach (var rune in text)
            {
                sum += GetCellLength(context, rune);
            }

            return sum;
        }

        public static int GetCellLength(RenderContext context, char rune)
        {
            if (context.LegacyConsole)
            {
                // Is it represented by a single byte?
                // In that case we don't have to calculate the
                // actual cell width.
                if (context.Encoding.GetByteCount(new[] { rune }) == 1)
                {
                    return 1;
                }
            }

            // TODO: We need to figure out why Segment.SplitLines fails
            // if we let wcwidth (which returns -1 instead of 1)
            // calculate the size for new line characters.
            // That is correct from a Unicode perspective, but the
            // algorithm was written before wcwidth was added and used
            // to work with string length and not cell length.
            if (rune == '\n')
            {
                return 1;
            }

            return UnicodeCalculator.GetWidth(rune);
        }
    }
}
