namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents the characters used to render a tree.
    /// </summary>
    public interface ITreeRendering
    {
        /// <summary>
        /// Get the set of characters used to render the corresponding <see cref="TreePart"/>.
        /// </summary>
        /// <param name="part">The part of the tree to get rendering string for.</param>
        /// <returns>Rendering string for the tree part.</returns>
        string GetPart(TreePart part);

        /// <summary>
        /// Gets the length of all tree part strings.
        /// </summary>
        int PartSize { get; }
    }
}