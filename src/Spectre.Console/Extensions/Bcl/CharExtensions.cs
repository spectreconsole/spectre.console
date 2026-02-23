namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="char"/>.
/// </summary>
public static partial class CharExtensions
{
#if WCWIDTH
    /// <summary>
    /// Gets the cell width of a character.
    /// </summary>
    /// <param name="character">The character to get the cell width of.</param>
    /// <returns>The cell width of the character.</returns>
    public static int GetCellWidth(this char character)
    {
        return Cell.GetCellLength(character);
    }
#endif

    internal static bool IsDigit(this char character, int min = 0)
    {
        return char.IsDigit(character) && character >= (char)min;
    }
}