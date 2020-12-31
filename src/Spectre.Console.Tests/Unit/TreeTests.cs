using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class TreeTests
    {
        public TreeTests()
        {
            Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");
        }
        
        [Fact]
        public Task Representative_Tree()
        {
            // Given
            var console = new FakeConsole(width: 80);
            var nestedChildren = 
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var children = new List<TreeNode>
            {
                new(new Text("child1"), nestedChildren), 
                new(new Text("child2")),
                new(new Text("child3"), new List<TreeNode> {new TreeNode(new Text("single leaf\n multiline"))}),
            };
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