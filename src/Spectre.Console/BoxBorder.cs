namespace Spectre.Console;

/// <summary>
/// Represents a border.
/// </summary>
public abstract partial class BoxBorder
{
    /// <summary>
    /// Gets the safe border for this border or <c>null</c> if none exist.
    /// </summary>
    public virtual BoxBorder? SafeBorder { get; }

    /// <summary>
    /// Gets the string representation of the specified border part.
    /// </summary>
    /// <param name="part">The part to get the character representation for.</param>
    /// <returns>A character representation of the specified border part.</returns>
    public abstract string GetPart(BoxBorderPart part);
}

/// <summary>
/// Contains extension methods for <see cref="BoxBorder"/>.
/// </summary>
public static class BoxExtensions
{
    /// <param name="border">The border to get the safe border for.</param>
    extension(BoxBorder border)
    {
        /// <summary>
        /// Gets the safe border for a border.
        /// </summary>
        /// <param name="safe">Whether or not to return the safe border.</param>
        /// <returns>The safe border if one exist, otherwise the original border.</returns>
        public BoxBorder GetSafeBorder(bool safe)
        {
            ArgumentNullException.ThrowIfNull(border);

            if (safe && border.SafeBorder != null)
            {
                border = border.SafeBorder;
            }

            return border;
        }
    }
}