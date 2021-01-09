using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
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
}