namespace Spectre.Console;

/// <summary>
/// Represents tree guide lines.
/// </summary>
public abstract partial class TreeGuide
{
    /// <summary>
    /// Gets the safe guide lines or <c>null</c> if none exist.
    /// </summary>
    public virtual TreeGuide? SafeTreeGuide { get; }

    /// <summary>
    /// Get the set of characters used to render the corresponding <see cref="TreeGuidePart"/>.
    /// </summary>
    /// <param name="part">The part of the tree to get rendering string for.</param>
    /// <returns>Rendering string for the tree part.</returns>
    public abstract string GetPart(TreeGuidePart part);
}

/// <summary>
/// Contains extension methods for <see cref="TreeGuide"/>.
/// </summary>
public static class TreeGuideExtensions
{
    /// <param name="guide">The tree guide to get the safe version for.</param>
    extension(TreeGuide guide)
    {
        /// <summary>
        /// Gets the safe border for a border.
        /// </summary>
        /// <param name="safe">Whether or not to return the safe border.</param>
        /// <returns>The safe border if one exist, otherwise the original border.</returns>
        public TreeGuide GetSafeTreeGuide(bool safe)
        {
            ArgumentNullException.ThrowIfNull(guide);

            if (safe && guide.SafeTreeGuide != null)
            {
                return guide.SafeTreeGuide;
            }

            return guide;
        }
    }
}