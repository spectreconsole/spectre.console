using System.Text;

namespace Spectre.Console.Internal
{
    internal static class CharExtensions
    {
        public static int CellLength(this char token, Encoding encoding)
        {
            return Cell.GetCellLength(encoding, token);
        }
    }
}
