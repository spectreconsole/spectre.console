using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    public class TreeNode : IRenderable
    {
        private readonly IRenderable _renderable;
        private List<TreeNode> _children;

        public TreeNode(IRenderable renderable, IEnumerable<TreeNode>? children = null)
        {
            _renderable = renderable;
            _children = new List<TreeNode>(children ?? Enumerable.Empty<TreeNode>());
        }

        public List<TreeNode> Children
        {
            get => _children;
        }

        public void AddChild(TreeNode child)
        {
            _children.Add(child);
        }

        public Measurement Measure(RenderContext context, int maxWidth)
        {
            return _renderable.Measure(context, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return _renderable.Render(context, maxWidth);
        }
    }
}