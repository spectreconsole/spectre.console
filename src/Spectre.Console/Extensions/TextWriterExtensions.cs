using System.IO;

namespace Spectre.Console
{
    internal static class TextWriterExtensions
    {
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

        public static bool IsStandardError(this TextWriter writer)
        {
            try
            {
                return writer == System.Console.Error;
            }
            catch
            {
                return false;
            }
        }
    }
}
