using Wcwidth;

namespace Spectre.Console
{
    internal static class Cell
    {
        public static int GetCellLength(string text)
        {
            var sum = 0;
            foreach (var rune in text)
            {
                sum += GetCellLength(rune);
            }

            return sum;
        }

        public static int GetCellLength(char rune)
        {
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
