namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Clear
    {
        [Theory]
        [InlineData(false, "Hello[2J[3JWorld")]
        [InlineData(true, "Hello[2J[3J[1;1HWorld")]
        public void Should_Clear_Screen(bool home, string expected)
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Write("Hello");
            console.Clear(home);
            console.Write("World");

            // Then
            console.Output.ShouldBe(expected);
        }
    }

    public sealed class Write
    {
        [Fact]
        public void Should_Combine_Decoration_And_Colors()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Write(
                "Hello",
                new Style()
                    .Foreground(Color.RoyalBlue1)
                    .Background(Color.NavajoWhite1)
                    .Decoration(Decoration.Italic));

            // Then
            console.Output.ShouldBe("\u001b[3;90;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Foreground_If_Set_To_Default_Color()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Write(
                "Hello",
                new Style()
                    .Foreground(Color.Default)
                    .Background(Color.NavajoWhite1)
                    .Decoration(Decoration.Italic));

            // Then
            console.Output.ShouldBe("\u001b[3;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Background_If_Set_To_Default_Color()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Write(
                "Hello",
                new Style()
                    .Foreground(Color.RoyalBlue1)
                    .Background(Color.Default)
                    .Decoration(Decoration.Italic));

            // Then
            console.Output.ShouldBe("\u001b[3;90mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Decoration_If_Set_To_None()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.Write(
                "Hello",
                new Style()
                    .Foreground(Color.RoyalBlue1)
                    .Background(Color.NavajoWhite1)
                    .Decoration(Decoration.None));

            // Then
            console.Output.ShouldBe("\u001b[90;47mHello\u001b[0m");
        }
    }

    public sealed class WriteLine
    {
        [Fact]
        public void Should_Reset_Colors_Correctly_After_Line_Break()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.WriteLine("Hello", new Style().Background(ConsoleColor.Red));
            console.WriteLine("World", new Style().Background(ConsoleColor.Green));

            // Then
            console.Output.NormalizeLineEndings()
                .ShouldBe("[101mHello[0m\n[102mWorld[0m\n");
        }

        [Fact]
        public void Should_Reset_Colors_Correctly_After_Line_Break_In_Text()
        {
            // Given
            var console = new TestConsole()
                .Colors(ColorSystem.Standard)
                .EmitAnsiSequences();

            // When
            console.WriteLine("Hello\nWorld", new Style().Background(ConsoleColor.Red));

            // Then
            console.Output.NormalizeLineEndings()
                .ShouldBe("[101mHello[0m\n[101mWorld[0m\n");
        }
    }

    public sealed class WriteException
    {
        [Fact]
        public void Should_Not_Throw_If_Exception_Has_No_StackTrace()
        {
            // Given
            var console = new TestConsole();
            var exception = new InvalidOperationException("An exception.");

            // When
            void When() => console.WriteException(exception);

            // Then
            Should.NotThrow(When);
        }
    }
    public sealed class NoWrap
    {
        [Fact]
        public void Should_Not_Wrap_Long_Lines_When_NoWrap_Is_Enabled()
        {
            // Given
            var console = new TestConsole();
            console.Profile.Width = 20;
            console.Profile.Capabilities.NoWrap = true;

            // When
            console.Write("This is a long line that exceeds the console width");

            // Then
            console.Output.ShouldBe("This is a long line that exceeds the console width");
        }

        [Fact]
        public void Should_Wrap_Long_Lines_When_NoWrap_Is_Disabled()
        {
            // Given
            var console = new TestConsole();
            console.Profile.Width = 20;
            console.Profile.Capabilities.NoWrap = false;

            // When
            console.Write("This is a long line that exceeds the console width");

            // Then
            console.Output.ShouldContain("\n");
        }

        [Fact]
        public void Should_Report_MaxValue_Width_When_NoWrap_Is_Enabled()
        {
            // Given
            var console = new TestConsole();
            console.Profile.Width = 80;
            console.Profile.Capabilities.NoWrap = true;

            // When / Then
            console.Profile.Width.ShouldBe(int.MaxValue);
        }
    }
}