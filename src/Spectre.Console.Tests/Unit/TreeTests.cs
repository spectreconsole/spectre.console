using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class TreeTests
    {
        [Fact]
        public Task Simple()
        {
            Environment.SetEnvironmentVariable("Verify_DisableClipboard", "true");
            
            var console = new FakeConsole(width: 80);
            var nestedChildren = 
                Enumerable.Range(0, 10)
                    .Select(x => new TreeNode(new Text($"multiple \n line {x}")));
            var children = new List<TreeNode> {new(new Text("child1"), nestedChildren), new(new Text("child2"))};
            var root = new TreeNode(new Text("Root node"), children);
            var tree = new Tree(root);

            // When
            console.Render(tree);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}