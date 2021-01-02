using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Node of a tree.
    /// </summary>
    public sealed class TreeNode : IRenderable
    {
        private readonly IRenderable _renderable;
        private List<TreeNode> _children;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="renderable">The <see cref="IRenderable"/> which this node wraps.</param>
        /// <param name="children">Any children that the node is declared with.</param>
        public TreeNode(IRenderable renderable, IEnumerable<TreeNode>? children = null)
        {
            _renderable = renderable;
            _children = new List<TreeNode>(children ?? Enumerable.Empty<TreeNode>());
        }

        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        public List<TreeNode> Children
        {
            get => _children;
        }

        /// <summary>
        /// Adds a child to the end of the node's list of children.
        /// </summary>
        /// <param name="child">Child to be added.</param>
        public void AddChild(TreeNode child)
        {
            _children.Add(child);
        }

        /// <inheritdoc/>
        public Measurement Measure(RenderContext context, int maxWidth)
        {
            return _renderable.Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return _renderable.Render(context, maxWidth);
        }
    }
}