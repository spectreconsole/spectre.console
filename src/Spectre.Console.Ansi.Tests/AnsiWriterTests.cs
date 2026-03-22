namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiWriterTests
{
    [Fact]
    public void Should_Write_Expected_Ansi()
    {
        // Given
        var fixture = new AnsiFixture();

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe(
            "\e]8;id=123;https://spectreconsole.net\e\\\e[1;3m\e[38;5;11mSpectre Console\e[0m\e]8;;\e\\");
    }

    [Fact]
    public void Should_Not_Write_Link_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = true;
        fixture.Capabilities.Links = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("\e[1;3m\e[38;5;11mSpectre Console\e[0m");
    }

    [Fact]
    public void Should_Not_Write_Ansi_If_Not_Supported()
    {
        // Given
        var fixture = new AnsiFixture();
        fixture.Capabilities.Ansi = false;

        // When
        fixture.Writer
            .BeginLink("https://spectreconsole.net", linkId: 123)
            .Decoration(Decoration.Bold | Decoration.Italic)
            .Foreground(Color.Yellow)
            .Write("Spectre Console")
            .ResetStyle()
            .EndLink();

        // Then
        fixture.Output.ShouldBe("Spectre Console");
    }

    public sealed class CursorLeft
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorLeft(4);

            // Then
            fixture.Output.ShouldBe("\e[4D");
        }
    }

    public sealed class CursorBackward
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackward(4);

            // Then
            fixture.Output.ShouldBe("\e[4D");
        }
    }

    public sealed class CursorRight
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorRight(4);

            // Then
            fixture.Output.ShouldBe("\e[4C");
        }
    }

    public sealed class CursorForward
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorForward(4);

            // Then
            fixture.Output.ShouldBe("\e[4C");
        }
    }

    public sealed class CursorDown
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorDown(4);

            // Then
            fixture.Output.ShouldBe("\e[4B");
        }
    }

    public sealed class CursorUp
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorUp(4);

            // Then
            fixture.Output.ShouldBe("\e[4A");
        }
    }

    public sealed class CursorPosition
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorPosition(4, 5);

            // Then
            fixture.Output.ShouldBe("\e[4;5H");
        }
    }

    public sealed class CursorHome
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHome();

            // Then
            fixture.Output.ShouldBe("\e[H");
        }
    }

    public sealed class EraseInDisplay
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseInDisplay(2);

            // Then
            fixture.Output.ShouldBe("\e[2J");
        }
    }

    public sealed class ClearScrollback
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ClearScrollback();

            // Then
            fixture.Output.ShouldBe("\e[3J");
        }
    }

    public sealed class EraseInLine
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseInLine(2);

            // Then
            fixture.Output.ShouldBe("\e[2K");
        }
    }

    public sealed class ShowCursor
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ShowCursor();

            // Then
            fixture.Output.ShouldBe("\e[?25h");
        }
    }

    public sealed class HideCursor
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.HideCursor();

            // Then
            fixture.Output.ShouldBe("\e[?25l");
        }
    }

    public sealed class SaveCursor
    {
        [Fact]
        public void Should_Write_Correct_Ansi_For_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SaveCursor(true);

            // Then
            fixture.Output.ShouldBe("\e[s");
        }

        [Fact]
        public void Should_Write_Correct_Ansi_For_Not_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SaveCursor(false);

            // Then
            fixture.Output.ShouldBe("\e7");
        }
    }

    public sealed class RestoreCursor
    {
        [Fact]
        public void Should_Write_Correct_Ansi_For_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.RestoreCursor(true);

            // Then
            fixture.Output.ShouldBe("\e[u");
        }

        [Fact]
        public void Should_Write_Correct_Ansi_For_Not_Staying_On_Page()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.RestoreCursor(false);

            // Then
            fixture.Output.ShouldBe("\e8");
        }
    }

    public sealed class CursorHorizontalAbsolute
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHorizontalAbsolute(4);

            // Then
            fixture.Output.ShouldBe("\e[4G");
        }
    }

    public sealed class EnterAltScreen
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EnterAltScreen();

            // Then
            fixture.Output.ShouldBe("\e[?1049h");
        }
    }

    public sealed class ExitAltScreen
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ExitAltScreen();

            // Then
            fixture.Output.ShouldBe("\e[?1049l");
        }
    }

    public sealed class CursorBackwardTabulation
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorBackwardTabulation(4);

            // Then
            fixture.Output.ShouldBe("\e[4Z");
        }
    }

    public sealed class CursorHorizontalTabulation
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorHorizontalTabulation(4);

            // Then
            fixture.Output.ShouldBe("\e[4I");
        }
    }

    public sealed class CursorNextLine
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorNextLine(4);

            // Then
            fixture.Output.ShouldBe("\e[4E");
        }
    }

    public sealed class CursorPreviousLine
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.CursorPreviousLine(4);

            // Then
            fixture.Output.ShouldBe("\e[4F");
        }
    }

    public sealed class Index
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.Index();

            // Then
            fixture.Output.ShouldBe("\eD");
        }
    }

    public sealed class ReverseIndex
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ReverseIndex();

            // Then
            fixture.Output.ShouldBe("\eM");
        }
    }

    public sealed class DeleteCharacter
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.DeleteCharacter(4);

            // Then
            fixture.Output.ShouldBe("\e[4P");
        }
    }

    public sealed class SetCursorStyle
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.SetCursorStyle(3);

            // Then
            fixture.Output.ShouldBe("\e[3 q");
        }
    }

    public sealed class DeleteLine
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.DeleteLine(3);

            // Then
            fixture.Output.ShouldBe("\e[3M");
        }
    }

    public sealed class EraseCharacter
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.EraseCharacter(3);

            // Then
            fixture.Output.ShouldBe("\e[3X");
        }
    }

    public sealed class InsertCharacter
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.InsertCharacter(3);

            // Then
            fixture.Output.ShouldBe("\e[3@");
        }
    }

    public sealed class InsertLine
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.InsertLine(3);

            // Then
            fixture.Output.ShouldBe("\e[3L");
        }
    }

    public sealed class ScrollDown
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ScrollDown(3);

            // Then
            fixture.Output.ShouldBe("\e[3T");
        }
    }

    public sealed class ScrollUp
    {
        [Fact]
        public void Should_Write_Correct_Ansi()
        {
            // Given
            var fixture = new AnsiFixture();

            // When
            fixture.Writer.ScrollUp(3);

            // Then
            fixture.Output.ShouldBe("\e[3S");
        }
    }
}
