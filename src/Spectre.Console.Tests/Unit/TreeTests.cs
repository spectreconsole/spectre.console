using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public class TreeTests
    {
        [Fact]
        public void Should_Measure_Tree_Correctly()
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
            measurement.Min.ShouldBe(17);
            measurement.Max.ShouldBe(19);
        }

        [Fact]
        public void Should_Measure_Tree_Correctly_With_Regard_To_Max_Width()
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
            measurement.Min.ShouldBe(400);
            measurement.Max.ShouldBe(80);
        }

        [Fact]
        public void Measure_Leaf_Dominated_Width()
        {
            // Given
            var root = new TreeNode(new Text("Root node"));
            var currentNode = root;
            foreach (var i in Enumerable.Range(0, 10))
            {
                var newNode = new TreeNode(new Text(i.ToString()));
                currentNode.AddChild(newNode);
                currentNode = newNode;
            }

            var tree = new Tree(root);

            // When
            var measurement = ((IRenderable)tree).Measure(new RenderContext(Encoding.Unicode, false), 80);

            // Then
            measurement.Min.ShouldBe(41);
            measurement.Max.ShouldBe(41);
        }

        [Fact]
        public Task Should_Render_Tree_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var nestedChildren =
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var child2 = new TreeNode(new Text("child2"));
            var child2Child = new TreeNode(new Text("child2Child"));
            child2.AddChild(child2Child);
            child2Child.AddChild(new TreeNode(new Text("Child 2 child\n child")));
            var child3 = new TreeNode(new Text("child3"));
            var child3Child = new TreeNode(new Text("single leaf\n multiline"));
            child3Child.AddChild(new TreeNode(new Calendar(2020, 01)));
            child3.AddChild(child3Child);
            var children = new List<TreeNode> { new(new Text("child1"), nestedChildren), child2, child3 };
            var root = new TreeNode(new Text("Root node"), children);
            var tree = new Tree(root);

            // When
            console.Render(tree);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Tree_With_Only_Root_Node_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var root = new TreeNode(new Text("Root node"), Enumerable.Empty<TreeNode>());
            var tree = new Tree(root);

            // When
            console.Render(tree);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}