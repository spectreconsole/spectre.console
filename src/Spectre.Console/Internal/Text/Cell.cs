using System.Linq;
using Spectre.Console.Rendering;
using Wcwidth;

namespace Spectre.Console.Internal
{
    internal static class Cell
    {
        public static int GetCellLength(RenderContext context, string text)
        {
            return text.Sum(rune =>
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

                return UnicodeCalculator.GetWidth(rune);
            });
        }
    }
}
