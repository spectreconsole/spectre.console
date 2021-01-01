namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Defines the different rendering parts of a <see cref="Tree"/>.
    /// </summary>
    public enum TreePart
    {
        /// <summary>
        /// Connection between siblings.
        /// </summary>
        SiblingConnector,

        /// <summary>
        /// Branch from parent to child.
        /// </summary>
        ChildBranch,

        /// <summary>
        /// Branch from parent to child for the last child in a set.
        /// </summary>
        BottomChildBranch,
    }
}