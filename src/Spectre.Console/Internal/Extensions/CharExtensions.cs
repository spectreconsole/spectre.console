namespace Spectre.Console.Internal;

internal static partial class CharExtensions
{
    public static bool IsDigit(this char character, int min = 0)
    {
        return char.IsDigit(character) && character >= (char)min;
    }
}