namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Defines the different rendering parts of a <see cref="Tree"/>.
    /// </summary>
    public enum TreeGuidePart
    {
        /// <summary>
        /// Represents a space.
        /// </summary>
        Space,

        /// <summary>
        /// Connection between siblings.
        /// </summary>
        Continue,

        /// <summary>
        /// Branch from parent to child.
        /// </summary>
        Fork,

        /// <summary>
        /// Branch from parent to child for the last child in a set.
        /// </summary>
        End,
    }
}