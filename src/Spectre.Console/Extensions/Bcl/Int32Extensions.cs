namespace Spectre.Console;

internal static class Int32Extensions
{
    extension(int value)
    {
        public int Clamp(int min, int max)
        {
            if (value <= min)
            {
                return min;
            }

            if (value >= max)
            {
                return max;
            }

            return value;
        }
    }
}