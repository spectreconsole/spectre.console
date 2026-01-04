namespace Spectre.Console;

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