using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    ///     Representation of tree data.
    /// </summary>
    public sealed class Tree : Renderable
    {
        private readonly TreeNode _rootNode;

        /// <summary>
        /// Gets or sets the tree style.
        /// </summary>
        public Style Style { get; set; } = Style.Plain;

        /// <summary>
        ///  Gets or sets the appearance of the tree.
        /// </summary>
        public TreeAppearance Appearance { get; set; } = TreeAppearance.Ascii;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class.
        /// </summary>
        /// <param name="rootNode">Root node of the tree to be rendered.</param>
        public Tree(TreeNode rootNode)
        {
            _rootNode = rootNode ?? throw new ArgumentNullException(nameof(rootNode));
        }

        /// <inheritdoc />
        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            Measurement MeasureAtDepth(RenderContext context, int maxWidth, TreeNode node, int depth)
            {
                var rootMeasurement = node.Measure(context, maxWidth);
                var treeIndentation = depth * Appearance.PartSize;
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

            return MeasureAtDepth(context, maxWidth, _rootNode, depth: 0);
        }

        /// <inheritdoc />
        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            return _rootNode
                .Render(context, maxWidth)
                .Concat(new List<Segment> { Segment.LineBreak })
                .Concat(RenderChildren(context, maxWidth - Appearance.PartSize, _rootNode, depth: 0));
        }

        private IEnumerable<Segment> RenderChildren(RenderContext context, int maxWidth, TreeNode node, int depth,
            int? trailingStarted = null)
        {
            var result = new List<Segment>();
            foreach (var (_, _, lastChild, childNode) in node.Children.Enumerate())
            {
                var lines = Segment.SplitLines(context, childNode.Render(context, maxWidth));
                foreach (var (_, isFirstLine, _, line) in lines.Enumerate())
                {
                    var siblingConnectorSegment =
                        new Segment(Appearance.GetPart(TreePart.SiblingConnector), Style);
                    if (trailingStarted != null)
                    {
                        result.AddRange(Enumerable.Repeat(siblingConnectorSegment, trailingStarted.Value));
                        result.AddRange(Enumerable.Repeat(
                            Segment.Padding(Appearance.PartSize),
                            depth - trailingStarted.Value));
                    }
                    else
                    {
                        result.AddRange(Enumerable.Repeat(siblingConnectorSegment, depth));
                    }

                    if (isFirstLine)
                    {
                        result.Add(lastChild
                            ? new Segment(Appearance.GetPart(TreePart.BottomChildBranch), Style)
                            : new Segment(Appearance.GetPart(TreePart.ChildBranch), Style));
                    }
                    else
                    {
                        result.Add(lastChild ? Segment.Padding(Appearance.PartSize) : siblingConnectorSegment);
                    }

                    result.AddRange(line);
                    result.Add(Segment.LineBreak);
                }

                var childTrailingStarted = trailingStarted ?? (lastChild ? depth : null);

                result.AddRange(
                    RenderChildren(
                        context, maxWidth - Appearance.PartSize,
                        childNode, depth + 1,
                        childTrailingStarted));
            }

            return result;
        }
    }
}