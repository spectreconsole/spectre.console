using System.Threading.Tasks;
using Spectre.Console.Testing;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class PadderTests
    {
        [Fact]
        public Task Should_Render_Padded_Object_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 60);
            var table = new Table();
            table.AddColumn("Foo");
            table.AddColumn("Bar");
            table.AddRow("Baz", "Qux");
            table.AddRow("Corgi", "Waldo");

            // When
            console.Render(new Padder(table).Padding(1, 2, 3, 4));

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Expanded_Padded_Object_Correctly()
        {
            // Given
            var console = new FakeConsole(width: 60);
            var table = new Table();
            table.AddColumn("Foo");
            table.AddColumn("Bar");
            table.AddRow("Baz", "Qux");
            table.AddRow("Corgi", "Waldo");

            // When
            console.Render(new Padder(table)
                .Padding(1, 2, 3, 4)
                .Expand());

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        public Task Should_Render_Padded_Object_Correctly_When_Nested_Within_Other_Object()
        {
            // Given
            var console = new FakeConsole(width: 60);
            var table = new Table();
            table.AddColumn("Foo");
            table.AddColumn("Bar", c => c.PadLeft(0).PadRight(0));
            table.AddRow("Baz", "Qux");
            table.AddRow(new Text("Corgi"), new Padder(new Panel("Waldo"))
                .Padding(2, 1));

            // When
            console.Render(new Padder(table)
                .Padding(1, 2, 3, 4)
                .Expand());

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
