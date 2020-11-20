using System.Threading.Tasks;
using Spectre.Console.Rendering;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class RowsTests
    {
        [Fact]
        public Task Should_Render_Rows()
        {
            // Given
            var console = new PlainConsole(width: 60);
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
            console.Render(rows);

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Rows_Correctly_Inside_Other_Widget()
        {
            // Given
            var console = new PlainConsole(width: 60);
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
            console.Render(table);

            // Then
            return Verifier.Verify(console.Lines);
        }

        [Fact]
        public Task Should_Render_Rows_Correctly_Inside_Other_Widget_When_Expanded()
        {
            // Given
            var console = new PlainConsole(width: 60);
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
            console.Render(table);

            // Then
            return Verifier.Verify(console.Lines);
        }
    }
}
