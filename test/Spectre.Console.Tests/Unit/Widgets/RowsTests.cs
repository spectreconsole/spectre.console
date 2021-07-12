using System.Threading.Tasks;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Rows")]
    public sealed class RowsTests
    {
        [Fact]
        [Expectation("Render")]
        public Task Should_Render_Rows()
        {
            // Given
            var console = new TestConsole().Width(60);
            var rows = new Rows(
                new IRenderable[]
                {
                    new Markup("Hello"),
                    new Table()
                        .AddColumns("Foo", "Bar")
                        .AddRow("Baz", "Qux"),
                    new Markup("World"),
                });

            // When
            console.Write(rows);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_Nested")]
        public Task Should_Render_Rows_Correctly_Inside_Other_Widget()
        {
            // Given
            var console = new TestConsole().Width(60);
            var table = new Table()
                .AddColumns("Foo", "Bar")
                .AddRow("HELLO WORLD")
                .AddRow(
                new Rows(new IRenderable[]
                {
                    new Markup("Hello"),
                    new Markup("World"),
                }), new Text("Qux"));

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }

        [Fact]
        [Expectation("Render_Expanded_And_Nested")]
        public Task Should_Render_Rows_Correctly_Inside_Other_Widget_When_Expanded()
        {
            // Given
            var console = new TestConsole().Width(60);
            var table = new Table()
                .AddColumns("Foo", "Bar")
                .AddRow("HELLO WORLD")
                .AddRow(
                new Rows(new IRenderable[]
                {
                    new Markup("Hello"),
                    new Markup("World"),
                }).Expand(), new Text("Qux"));

            // When
            console.Write(table);

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
