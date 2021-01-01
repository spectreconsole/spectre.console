using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectre.Console.Rendering;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public class TreeMeasureTests
    {
        [Fact]
        public void Measure_Tree_Dominated_Width()
        {
            // Given
            var nestedChildren =
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var child3 = new TreeNode(new Text("child3"));
            child3.AddChild(new TreeNode(new Text("single leaf\n multiline")));
            var children = new List<TreeNode>
            {
                new(new Text("child1"), nestedChildren), new(new Text("child2")), child3,
            };
            var root = new TreeNode(new Text("Root node"), children);
            var tree = new Tree(root);

            // When
            var measurement = ((IRenderable)tree).Measure(new RenderContext(Encoding.Unicode, false), 80);

            // Then
            // Corresponds to "│   └── multiple"
            Assert.Equal(17, measurement.Min);

            // Corresponds to "    └── single leaf" when untrimmed
            Assert.Equal(19, measurement.Max);
        }

        [Fact]
        public void Measure_Max_Width_Bound()
        {
            // Given
            var root = new TreeNode(new Text("Root node"));
            var currentNode = root;
            foreach (var i in Enumerable.Range(0, 100))
            {
                var newNode = new TreeNode(new Text(string.Empty));
                currentNode.AddChild(newNode);
                currentNode = newNode;
            }

            var tree = new Tree(root);

            // When
            var measurement = ((IRenderable)tree).Measure(new RenderContext(Encoding.Unicode, false), 80);

            // Then
            // Each node depth contributes 4 characters, so 100 node depth -> 400 character min width
            Assert.Equal(400, measurement.Min);

            // Successfully capped at 80 terminal width
            Assert.Equal(80, measurement.Max);
        }

        [Fact]
        public void Measure_Leaf_Dominated_Width()
        {
            // Given
            var root = new TreeNode(new Text("Root node"));
            var currentNode = root;
            foreach (var i in Enumerable.Range(0, 10))
            {
                var newNode = new TreeNode(new Text(string.Empty));
                currentNode.AddChild(newNode);
                currentNode = newNode;
            }

            var tree = new Tree(root);

            // When
            var measurement = ((IRenderable)tree).Measure(new RenderContext(Encoding.Unicode, false), 80);

            // Then
            // Corresponds to "│   │   │   │   │   │   │   │   │   └── "
            Assert.Equal(40, measurement.Min);

            // Corresponds to "│   │   │   │   │   │   │   │   │   └── "
            Assert.Equal(40, measurement.Max);
        }
    }
}