namespace Spectre.Console.Tests.Unit;

public partial class AnsiConsoleTests
{
    public sealed class Clear
    {
        [Theory]
        [InlineData(false, "Hello\e[2J\e[3JWorld")]
        [InlineData(true, "Hello\e[2J\e[3J\e[1;1HWorld")]
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
        [Theory]
        [InlineData("{Pt.1} (TEST ~ 855D)")]
        [InlineData("{")]
        [InlineData("test {0}")]
        [InlineData("test {0} {1}")]
        public void Should_Treat_String_Value_As_Literal_Text(string value)
        {
            // Given
            var previous = AnsiConsole.Console;
            var console = new TestConsole();

            try
            {
                AnsiConsole.Console = console;

                // When
                AnsiConsole.Write(value);
            }
            finally
            {
                AnsiConsole.Console = previous;
            }

            // Then
            console.Output.ShouldBe(value);
        }

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
                .ShouldBe("\e[101mHello\e[0m\n\e[102mWorld\e[0m\n");
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
                .ShouldBe("\e[101mHello\e[0m\n\e[101mWorld\e[0m\n");
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

    [Fact]
    public void Should_Write_To_New_Output_If_Redirected()
    {
        // Given
        var console = new TestConsole();
        var redirected = new StringWriter();

        // When
        console.Write("ABC");
        console.Profile.Out = new AnsiConsoleOutput(redirected);
        console.Write("DEF");

        // Then
        console.Output.ShouldBe("ABC");
        redirected.ToString().ShouldBe("DEF");
    }
}
