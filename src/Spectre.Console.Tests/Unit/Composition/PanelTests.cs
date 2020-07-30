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
            console.Render(new Panel(Text.New("Hello World")));

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌─────────────┐");
            console.Lines[1].ShouldBe("│ Hello World │");
            console.Lines[2].ShouldBe("└─────────────┘");
        }

        [Fact]
        public void Should_Render_Panel_With_Unicode_Correctly()
        {
            // Given
            var console = new PlainConsole(width: 80);

            // When
            console.Render(new Panel(Text.New(" \n💩\n ")));

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
            console.Render(new Panel(Text.New("Hello World\nFoo Bar")));

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
                Text.New("I heard [underline on blue]you[/] like 📦\n\n\n\nSo I put a 📦 in a 📦"),
                content: Justify.Center);

            // When
            console.Render(text);

            // Then
            console.Lines.Count.ShouldBe(7);
            console.Lines[0].ShouldBe("┌───────────────────────┐");
            console.Lines[1].ShouldBe("│  I heard you like 📦  │");
            console.Lines[2].ShouldBe("│                       │");
            console.Lines[3].ShouldBe("│                       │");
            console.Lines[4].ShouldBe("│                       │");
            console.Lines[5].ShouldBe("│ So I put a 📦 in a 📦 │");
            console.Lines[6].ShouldBe("└───────────────────────┘");
        }

        [Fact]
        public void Should_Fit_Panel_To_Parent_If_Enabled()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(new Panel(Text.New("Hello World"), fit: true));

            // Then
            console.Lines.Count.ShouldBe(3);
            console.Lines[0].ShouldBe("┌───────────────────────┐");
            console.Lines[1].ShouldBe("│ Hello World           │");
            console.Lines[2].ShouldBe("└───────────────────────┘");
        }

        [Fact]
        public void Should_Justify_Child_To_Right()
        {
            // Given
            var console = new PlainConsole(width: 25);

            // When
            console.Render(new Panel(Text.New("Hello World"), fit: true, content: Justify.Right));

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
            console.Render(new Panel(Text.New("Hello World"), fit: true, content: Justify.Center));

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
            console.Render(new Panel(new Panel(Text.New("Hello World"))));

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
