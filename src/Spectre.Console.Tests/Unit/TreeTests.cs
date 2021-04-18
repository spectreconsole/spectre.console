using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Tree")]
    public class TreeTests
    {
        [Fact]
        [Expectation("Render")]
        public Task Should_Render_Tree_Correctly()
        {
            // Given
            var console = new TestConsole();

            var tree = new Tree(new Text("Root node")).Guide(TreeGuide.DoubleLine);

            var nestedChildren = Enumerable.Range(0, 10).Select(x => new Text($"multiple\nline {x}"));
            var child2 = new TreeNode(new Text("child2"));
            var child2Child = new TreeNode(new Text("child2-1"));
            child2.AddNode(child2Child);
            child2Child.AddNode(new TreeNode(new Text("Child2-1-1\nchild")));
            var child3 = new TreeNode(new Text("child3"));
            var child3Child = new TreeNode(new Text("single leaf\nmultiline"));
            child3Child.AddNode(new TreeNode(new Calendar(2021, 01)));
            child3.AddNode(child3Child);

            tree.AddNode("child1").AddNodes(nestedChildren);
            tree.AddNode(child2);
            tree.AddNode(child3);
            tree.AddNode("child4");

            // When
            console.Write(tree);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_NoChildren")]
        public Task Should_Render_Tree_With_No_Child_Nodes_Correctly()
        {
            // Given
            var console = new TestConsole();
            var tree = new Tree(new Text("Root node"));

            // When
            console.Write(tree);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public void Should_Throw_If_Tree_Contains_Cycles()
        {
            // Given
            var console = new TestConsole();

            var child2 = new TreeNode(new Text("child 2"));
            var child3 = new TreeNode(new Text("child 3"));
            var child1 = new TreeNode(new Text("child 1"));
            child1.AddNodes(child2, child3);
            var root = new TreeNode(new Text("Branch Node"));
            root.AddNodes(child1);
            child2.AddNode(root);

            var tree = new Tree("root node");
            tree.AddNodes(root);

            // When
            var result = Record.Exception(() => console.Write(tree));

            // Then
            result.ShouldBeOfType<CircularTreeException>();
        }
    }
}
