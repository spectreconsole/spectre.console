using System;
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
        
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return MeasureAtDepth(context, maxWidth, _rootNode, 0);
        }

        private Measurement MeasureAtDepth(RenderContext context, int maxWidth, TreeNode node, int depth)
        {
            var rootMeasurement = node.Measure(context, maxWidth);
            var treeIndentation = depth * Rendering.PartSize;
            var currentMax = rootMeasurement.Max + treeIndentation;
            var currentMin = rootMeasurement.Min + treeIndentation;
            
            foreach (var child in node.Children)
            {
                var childMeasurement = MeasureAtDepth(context, maxWidth, child, depth + 1);
                if (childMeasurement.Min > currentMin)
                {
                    currentMin = childMeasurement.Min;
                }

                if (childMeasurement.Max > currentMax)
                {
                    currentMax = childMeasurement.Max;
                }
            }

            return new Measurement(currentMin, Math.Min(currentMax, maxWidth));
        }

        /// <inheritdoc/>
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return _rootNode
                .Render(context, maxWidth)
                .Concat(new List<Segment> { Segment.LineBreak })
                .Concat(RenderChildren(context, maxWidth - Rendering.PartSize, _rootNode, 0, false));
        }

        private IEnumerable<Segment> RenderChildren(RenderContext context, int maxWidth, TreeNode node, int depth, bool trailingTree)
        {
            var result = new List<Segment>();
            
            foreach (var (index, firstChild, lastChild, childNode) in node.Children.Enumerate())
            {
                var lines = Segment.SplitLines(context, childNode.Render(context, maxWidth));
                
                foreach (var (lineIndex, firstLine, lastLine, line) in lines.Enumerate())
                {
                    var siblingConnectorSegment =
                        new Segment(Rendering.GetPart(TreePart.SiblingConnector), Style);
                    if (trailingTree)
                    {
                        result.Add(Segment.Padding(Rendering.PartSize));
                    }
                    else
                    {
                        result.AddRange(Enumerable.Repeat(siblingConnectorSegment, depth));
                    }

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

                var lastChildOnFirstRow = depth == 0 && lastChild;
                result.AddRange(RenderChildren(context, maxWidth - Rendering.PartSize, childNode, depth + 1,
                    lastChildOnFirstRow));
            }

            return result;
        }
    }
}