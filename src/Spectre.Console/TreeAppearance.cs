using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a tree appearance.
    /// </summary>
    public abstract partial class TreeAppearance
    {
        /// <summary>
        /// Gets the length of all tree part strings.
        /// </summary>
        public abstract int PartSize { get; }

        /// <summary>
        /// Get the set of characters used to render the corresponding <see cref="TreePart"/>.
        /// </summary>
        /// <param name="part">The part of the tree to get rendering string for.</param>
        /// <returns>Rendering string for the tree part.</returns>
        public abstract string GetPart(TreePart part);
    }
}