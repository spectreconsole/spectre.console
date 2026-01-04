namespace Spectre.Console;

/// <summary>
/// Represents a tree node.
/// </summary>
public sealed class TreeNode : IHasTreeNodes
{
    internal IRenderable Renderable { get; }

    /// <summary>
    /// Gets the tree node's child nodes.
    /// </summary>
    public List<TreeNode> Nodes { get; } = new List<TreeNode>();

    /// <summary>
    /// Gets or sets a value indicating whether or not the tree node is expanded or not.
    /// </summary>
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="TreeNode"/> class.
    /// </summary>
    /// <param name="renderable">The tree node label.</param>
    public TreeNode(IRenderable renderable)
    {
        Renderable = renderable;
    }
}

/// <summary>
/// Contains extension methods for <see cref="TreeNode"/>.
/// </summary>
public static class TreeNodeExtensions
{
    /// <param name="node">The tree node.</param>
    extension(TreeNode node)
    {
        /// <summary>
        /// Expands the tree.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TreeNode Expand()
        {
            return Expand(node, true);
        }

        /// <summary>
        /// Collapses the tree.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TreeNode Collapse()
        {
            return Expand(node, false);
        }

        /// <summary>
        /// Sets whether or not the tree node should be expanded.
        /// </summary>
        /// <param name="expand">Whether or not the tree node should be expanded.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TreeNode Expand(bool expand)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            node.Expanded = expand;
            return node;
        }
    }
}