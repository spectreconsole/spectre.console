using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class PanelTests
    {
        [Fact]
        public void Should_Render_Panel()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World")));

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌─────────────┐");
            console.Lines[1].ShouldBe("│ Hello World │");
            console.Lines[2].ShouldBe("└─────────────┘");
        }

        [Fact]
        public void Should_Render_Panel_With_Padding()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World"))
            {
                Padding = new Padding(3, 5),
            });

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌───────────────────┐");
            console.Lines[1].ShouldBe("│   Hello World     │");
            console.Lines[2].ShouldBe("└───────────────────┘");
        }

        [Fact]
        public void Should_Render_Panel_With_Unicode_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text(" \n💩\n ")));

            // Then
            console.Lines.Count.ShouldBe(5);
            console.Lines[0].ShouldBe("┌────┐");
            console.Lines[1].ShouldBe("│    │");
            console.Lines[2].ShouldBe("│ 💩 │");
            console.Lines[3].ShouldBe("│    │");
            console.Lines[4].ShouldBe("└────┘");
        }

        [Fact]
        public void Should_Render_Panel_With_Multiple_Lines()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World\nFoo Bar")));

            // Then
            console.Lines.Count.ShouldBe(4);
            console.Lines[0].ShouldBe("┌─────────────┐");
            console.Lines[1].ShouldBe("│ Hello World │");
            console.Lines[2].ShouldBe("│ Foo Bar     │");
            console.Lines[3].ShouldBe("└─────────────┘");
        }

        [Fact]
        public void Should_Preserve_Explicit_Line_Ending()
        {
            // Given
            var console = new PlainConsole(width: 80);
            var text = new Panel(
                Text.Markup("I heard [underline on blue]you[/] like 📦\n\n\n\nSo I put a 📦 in a 📦"));

            // When
            console.Render(text);

            // Then
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("┌───────────────────────┐");
            console.Lines[1].ShouldBe("│ I heard you like 📦   │");
            console.Lines[2].ShouldBe("│                       │");
            console.Lines[3].ShouldBe("│                       │");
            console.Lines[4].ShouldBe("│                       │");
            console.Lines[5].ShouldBe("│ So I put a 📦 in a 📦 │");
            console.Lines[6].ShouldBe("└───────────────────────┘");
        }

        [Fact]
        public void Should_Expand_Panel_If_Enabled()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Text("Hello World"))
            {
                Expand = true,
            });

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].Length.ShouldBe(80);
            console.Lines[0].ShouldBe("┌──────────────────────────────────────────────────────────────────────────────┐");
            console.Lines[1].ShouldBe("│ Hello World                                                                  │");
            console.Lines[2].ShouldBe("└──────────────────────────────────────────────────────────────────────────────┘");
        }

        [Fact]
        public void Should_Justify_Child_To_Right()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(
                new Panel(
                    new Text("Hello World").WithAlignment(Justify.Right))
                {
                    Expand = true,
                });

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌───────────────────────┐");
            console.Lines[1].ShouldBe("│           Hello World │");
            console.Lines[2].ShouldBe("└───────────────────────┘");
        }

        [Fact]
        public void Should_Justify_Child_To_Center()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(
                new Panel(
                    new Text("Hello World").WithAlignment(Justify.Center))
                {
                    Expand = true,
                });

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌───────────────────────┐");
            console.Lines[1].ShouldBe("│      Hello World      │");
            console.Lines[2].ShouldBe("└───────────────────────┘");
        }

        [Fact]
        public void Should_Render_Panel_Inside_Panel_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(new Panel(new Text("Hello World"))));

            // Then
            console.Lines.Count.ShouldBe(5);
            console.Lines[0].ShouldBe("┌─────────────────┐");
            console.Lines[1].ShouldBe("│ ┌─────────────┐ │");
            console.Lines[2].ShouldBe("│ │ Hello World │ │");
            console.Lines[3].ShouldBe("│ └─────────────┘ │");
            console.Lines[4].ShouldBe("└─────────────────┘");
        }
    }
}
