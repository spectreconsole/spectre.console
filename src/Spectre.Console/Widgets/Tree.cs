using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Representation of tree data.
    /// </summary>
    public class Tree : Renderable
    {
        private readonly TreeNode _rootNode;
        
        public Style Style { get; set; } = Style.Plain;

        public ITreeRendering Rendering { get; set; } = TreeRendering.Ascii;
        
        public Tree(TreeNode rootNode)
        {
            _rootNode = rootNode;
        }
        
        // TODO MC: Implement Measure
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return base.Measure(context, maxWidth);
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return _rootNode
                .Render(context, maxWidth)
                .Concat(new List<Segment> { Segment.LineBreak })
                .Concat(RenderChildren(context, maxWidth - 3, _rootNode, 0));
        }

        private IEnumerable<Segment> RenderChildren(RenderContext context, int maxWidth, TreeNode node, int depth)
        {
            var result = new List<Segment>();
            
            foreach (var (index, firstChild, lastChild, childNode) in node.Children.Enumerate())
            {
                var lines = Segment.SplitLines(context, childNode.Render(context, maxWidth));
                
                foreach (var (lineIndex, firstLine, lastLine, line) in lines.Enumerate())
                {
                    var siblingConnectorSegment =
                        new Segment(Rendering.GetPart(TreePart.SiblingConnector), Style); 
                    result.AddRange(Enumerable.Repeat(siblingConnectorSegment, depth));
                    
                    if (firstLine)
                    {
                        result.Add(lastChild
                            ? new Segment(Rendering.GetPart(TreePart.BottomChildBranch), Style)
                            : new Segment(Rendering.GetPart(TreePart.ChildBranch), Style));
                    }
                    else
                    {
                        result.Add(lastChild ? Segment.Padding(Rendering.PartSize) : siblingConnectorSegment);
                    }

                    result.AddRange(line);
                    result.Add(Segment.LineBreak);
                }
                
                result.AddRange(RenderChildren(context, maxWidth, childNode, depth + 1));
            }

            return result;
        }
    }
}