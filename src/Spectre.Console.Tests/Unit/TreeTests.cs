using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public class TreeTests
    {
        [Fact]
        public Task Should_Render_Tree_With_Single_Root_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var nestedChildren =
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var child2 = new TreeNode(new Text("child2"));
            var child2Child = new TreeNode(new Text("child2Child"));
            child2.AddNode(child2Child);
            child2Child.AddNode(new TreeNode(new Text("Child 2 child\n child")));
            var child3 = new TreeNode(new Text("child3"));
            var child3Child = new TreeNode(new Text("single leaf\n multiline"));
            child3Child.AddNode(new TreeNode(new Calendar(2020, 01)));
            child3.AddNode(child3Child);
            var children = new List<TreeNode> { new(new Text("child1"), nestedChildren), child2, child3 };
            var root = new TreeNode(new Text("Root node"), children);
            var tree = new Tree().AddNode(root);

            // When
            console.Render(tree);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Tree_With_Multiple_Roots_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var nestedChildren =
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var child2 = new TreeNode(new Text("child2"));
            var child2Child = new TreeNode(new Text("child2Child"));
            child2.AddNode(child2Child);
            child2Child.AddNode(new TreeNode(new Text("Child 2 child\n child")));
            var secondRoot = new TreeNode(new Text("secondRoot"));
            secondRoot.AddNode(new TreeNode(new Text("secondRoot child")));

            var child3 = new TreeNode(new Text("child3"));
            var child3Child = new TreeNode(new Text("single leaf\n multiline"));
            child3Child.AddNode(new TreeNode(new Calendar(2020, 01)));
            child3.AddNode(child3Child);
            var children = new List<TreeNode> { new(new Text("child1"), nestedChildren), child2, child3 };
            var root = new TreeNode(new Text("Root node"), children);
            var tree = new Tree().AddNode(root).AddNode(secondRoot);

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
            var tree = new Tree().AddNode(root);

            // When
            console.Render(tree);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public void Tree_With_Cycle_Throws()
        {
            // Given
            var console = new FakeConsole(width: 80);

            var child2 = new TreeNode(new Text("child 2"));
            var child3 = new TreeNode(new Text("child 3"));
            var child1 = new TreeNode(new Text("child 1"), new[] {child2, child3});
            var root = new TreeNode(new Text("Root node"), new[] {child1});
            child2.Children.Add(root);
            var tree = new Tree().AddNode(root);

            // When
            Should.Throw<CircularTreeException>(() => console.Render(tree));
        }
    }
}