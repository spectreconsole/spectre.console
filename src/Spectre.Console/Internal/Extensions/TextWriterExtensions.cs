using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Spectre.Console.Internal
{
    internal static class TextWriterExtensions
    {
        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        public static bool IsStandardOut(this TextWriter writer)
        {
            try
            {
                return writer == System.Console.Out;
            }
            catch
            {
                return false;
            }
        }
    }
}
