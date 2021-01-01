using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class TreeRenderingTests
    {
        [Fact]
        public Task Representative_Tree()
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
        public Task Root_Node_Only()
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