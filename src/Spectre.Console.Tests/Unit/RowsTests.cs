using Shouldly;
using Spectre.Console.Rendering;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class RowsTests
    {
        [Fact]
        public void Should_Render_Rows()
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
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("Hello");
            console.Lines[1].ShouldBe("┌─────┬─────┐");
            console.Lines[2].ShouldBe("│ Foo │ Bar │");
            console.Lines[3].ShouldBe("├─────┼─────┤");
            console.Lines[4].ShouldBe("│ Baz │ Qux │");
            console.Lines[5].ShouldBe("└─────┴─────┘");
            console.Lines[6].ShouldBe("World");
        }

        [Fact]
        public void Should_Render_Rows_Correctly_Inside_Other_Widget()
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
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("┌─────────────┬─────┐");
            console.Lines[1].ShouldBe("│ Foo         │ Bar │");
            console.Lines[2].ShouldBe("├─────────────┼─────┤");
            console.Lines[3].ShouldBe("│ HELLO WORLD │     │");
            console.Lines[4].ShouldBe("│ Hello       │ Qux │");
            console.Lines[5].ShouldBe("│ World       │     │");
            console.Lines[6].ShouldBe("└─────────────┴─────┘");
        }

        [Fact]
        public void Should_Render_Rows_Correctly_Inside_Other_Widget_When_Expanded()
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
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("┌────────────────────────────────────────────────────┬─────┐");
            console.Lines[1].ShouldBe("│ Foo                                                │ Bar │");
            console.Lines[2].ShouldBe("├────────────────────────────────────────────────────┼─────┤");
            console.Lines[3].ShouldBe("│ HELLO WORLD                                        │     │");
            console.Lines[4].ShouldBe("│ Hello                                              │ Qux │");
            console.Lines[5].ShouldBe("│ World                                              │     │");
            console.Lines[6].ShouldBe("└────────────────────────────────────────────────────┴─────┘");
        }
    }
}
