namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="char"/>.
/// </summary>
public static partial class CharExtensions
{
    /// <param name="character">The character to get the cell width of.</param>
    extension(char character)
    {
        /// <summary>
        /// Gets the cell width of a character.
        /// </summary>
        /// <returns>The cell width of the character.</returns>
        public int GetCellWidth()
        {
            return Cell.GetCellLength(character);
        }
    }
}