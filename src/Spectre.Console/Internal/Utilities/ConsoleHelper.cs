using System.IO;
using System.Runtime.InteropServices;

namespace Spectre.Console.Internal
{
    internal static class ConsoleHelper
    {
        public static int GetSafeBufferWidth(int defaultValue = Constants.DefaultBufferWidth)
        {
            try
            {
                var width = System.Console.BufferWidth;
                if (width == 0)
                {
                    width = defaultValue;
                }

                return width;
            }
            catch (IOException)
            {
                return defaultValue;
            }
        }

        public static int GetSafeBufferHeight(int defaultValue = Constants.DefaultBufferWidth)
        {
            try
            {
                var height = System.Console.BufferHeight;
                if (height == 0)
                {
                    height = defaultValue;
                }

                return height;
            }
            catch (IOException)
            {
                return defaultValue;
            }
        }
    }
}
