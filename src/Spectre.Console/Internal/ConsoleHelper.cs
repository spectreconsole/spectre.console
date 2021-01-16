using System.IO;

namespace Spectre.Console
{
    internal static class ConsoleHelper
    {
        public static int GetSafeWidth(int defaultValue = Constants.DefaultTerminalWidth)
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

        public static int GetSafeHeight(int defaultValue = Constants.DefaultTerminalHeight)
        {
            try
            {
                var height = System.Console.WindowHeight;
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
