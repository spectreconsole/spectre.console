using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class PadderTests
    {
        [Fact]
        public void Should_Render_Padded_Object_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 60);
            var table = new Table();
            table.AddColumn("Foo");
            table.AddColumn("Bar");
            table.AddRow("Baz", "Qux");
            table.AddRow("Corgi", "Waldo");

            // When
            console.Render(new Padder(table).Padding(1, 2, 3, 4));

            // Then
            console.Lines.Count.ShouldBe(12);
            console.Lines[00].ShouldBe("                     ");
            console.Lines[01].ShouldBe("                     ");
            console.Lines[02].ShouldBe(" ┌───────┬───────┐   ");
            console.Lines[03].ShouldBe(" │ Foo   │ Bar   │   ");
            console.Lines[04].ShouldBe(" ├───────┼───────┤   ");
            console.Lines[05].ShouldBe(" │ Baz   │ Qux   │   ");
            console.Lines[06].ShouldBe(" │ Corgi │ Waldo │   ");
            console.Lines[07].ShouldBe(" └───────┴───────┘   ");
            console.Lines[08].ShouldBe("                     ");
            console.Lines[09].ShouldBe("                     ");
            console.Lines[10].ShouldBe("                     ");
            console.Lines[11].ShouldBe("                     ");
        }

        [Fact]
        public void Should_Render_Expanded_Padded_Object_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 60);
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
            console.Lines.Count.ShouldBe(12);
            console.Lines[00].ShouldBe("                                                            ");
            console.Lines[01].ShouldBe("                                                            ");
            console.Lines[02].ShouldBe(" ┌───────┬───────┐                                          ");
            console.Lines[03].ShouldBe(" │ Foo   │ Bar   │                                          ");
            console.Lines[04].ShouldBe(" ├───────┼───────┤                                          ");
            console.Lines[05].ShouldBe(" │ Baz   │ Qux   │                                          ");
            console.Lines[06].ShouldBe(" │ Corgi │ Waldo │                                          ");
            console.Lines[07].ShouldBe(" └───────┴───────┘                                          ");
            console.Lines[08].ShouldBe("                                                            ");
            console.Lines[09].ShouldBe("                                                            ");
            console.Lines[10].ShouldBe("                                                            ");
            console.Lines[11].ShouldBe("                                                            ");
        }

        [Fact]
        public void Should_Render_Padded_Object_Correctly_When_Nested_Within_Other_Object()
        {
            // Given
            var console = new PlainConsole(width: 60);
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
            console.Lines.Count.ShouldBe(16);
            console.Lines[00].ShouldBe("                                                            ");
            console.Lines[01].ShouldBe("                                                            ");
            console.Lines[02].ShouldBe(" ┌───────┬─────────────┐                                    ");
            console.Lines[03].ShouldBe(" │ Foo   │Bar          │                                    ");
            console.Lines[04].ShouldBe(" ├───────┼─────────────┤                                    ");
            console.Lines[05].ShouldBe(" │ Baz   │Qux          │                                    ");
            console.Lines[06].ShouldBe(" │ Corgi │             │                                    ");
            console.Lines[07].ShouldBe(" │       │  ┌───────┐  │                                    ");
            console.Lines[08].ShouldBe(" │       │  │ Waldo │  │                                    ");
            console.Lines[09].ShouldBe(" │       │  └───────┘  │                                    ");
            console.Lines[10].ShouldBe(" │       │             │                                    ");
            console.Lines[11].ShouldBe(" └───────┴─────────────┘                                    ");
            console.Lines[12].ShouldBe("                                                            ");
            console.Lines[13].ShouldBe("                                                            ");
            console.Lines[14].ShouldBe("                                                            ");
            console.Lines[15].ShouldBe("                                                            ");
        }
    }
}
