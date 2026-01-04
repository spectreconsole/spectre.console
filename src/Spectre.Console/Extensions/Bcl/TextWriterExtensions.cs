namespace Spectre.Console;

internal static class TextWriterExtensions
{
    extension(TextWriter writer)
    {
        public bool IsStandardOut()
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

        public bool IsStandardError()
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