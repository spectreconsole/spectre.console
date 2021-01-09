using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="TreeNode"/>.
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Expands the tree.
        /// </summary>
        /// <param name="node">The tree node.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TreeNode Expand(this TreeNode node)
        {
            return Expand(node, true);
        }

        /// <summary>
        /// Collapses the tree.
        /// </summary>
        /// <param name="node">The tree node.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TreeNode Collapse(this TreeNode node)
        {
            return Expand(node, false);
        }

        /// <summary>
        /// Sets whether or not the tree node should be expanded.
        /// </summary>
        /// <param name="node">The tree node.</param>
        /// <param name="expand">Whether or not the tree node should be expanded.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TreeNode Expand(this TreeNode node, bool expand)
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
