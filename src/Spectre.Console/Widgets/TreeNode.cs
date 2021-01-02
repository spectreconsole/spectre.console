using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Node of a tree.
    /// </summary>
    public sealed class TreeNode : IHasTreeNodes, IRenderable
    {
        private readonly IRenderable _renderable;

        /// <inheritdoc/>
        public List<TreeNode> Children { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        /// <param name="renderable">The <see cref="IRenderable"/> which this node wraps.</param>
        /// <param name="children">Any children that the node is declared with.</param>
        public TreeNode(IRenderable renderable, IEnumerable<TreeNode>? children = null)
        {
            _renderable = renderable ?? throw new ArgumentNullException(nameof(renderable));
            Children = new List<TreeNode>(children ?? Enumerable.Empty<TreeNode>());
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