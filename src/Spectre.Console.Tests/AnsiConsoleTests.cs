using Shouldly;
using Xunit;

namespace Spectre.Console.Tests
{
    public partial class AnsiConsoleTests
    {
        [Fact]
        public void Should_Combine_Style_And_Colors()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
            fixture.Console.Foreground = Color.RoyalBlue1;
            fixture.Console.Background = Color.NavajoWhite1;
            fixture.Console.Style = Styles.Italic;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[3;90;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Foreground_If_Set_To_Default_Color()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
            fixture.Console.Foreground = Color.Default;
            fixture.Console.Background = Color.NavajoWhite1;
            fixture.Console.Style = Styles.Italic;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[3;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Background_If_Set_To_Default_Color()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
            fixture.Console.Foreground = Color.RoyalBlue1;
            fixture.Console.Background = Color.Default;
            fixture.Console.Style = Styles.Italic;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[3;90mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Style_If_Set_To_None()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
            fixture.Console.Foreground = Color.RoyalBlue1;
            fixture.Console.Background = Color.NavajoWhite1;
            fixture.Console.Style = Styles.None;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[90;47mHello\u001b[0m");
        }
    }
}
