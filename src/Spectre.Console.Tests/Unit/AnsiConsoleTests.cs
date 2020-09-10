using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public partial class AnsiConsoleTests
    {
        [Fact]
        public void Should_Combine_Decoration_And_Colors()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);

            // When
            fixture.Console.Write(
                "Hello",
                Style.WithForeground(Color.RoyalBlue1)
                     .WithBackground(Color.NavajoWhite1)
                     .WithDecoration(Decoration.Italic));

            // Then
            fixture.Output.ShouldBe("\u001b[3;90;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Foreground_If_Set_To_Default_Color()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);

            // When
            fixture.Console.Write(
                "Hello",
                Style.WithForeground(Color.Default)
                     .WithBackground(Color.NavajoWhite1)
                     .WithDecoration(Decoration.Italic));

            // Then
            fixture.Output.ShouldBe("\u001b[3;47mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Background_If_Set_To_Default_Color()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);

            // When
            fixture.Console.Write(
                "Hello",
                Style.WithForeground(Color.RoyalBlue1)
                     .WithBackground(Color.Default)
                     .WithDecoration(Decoration.Italic));

            // Then
            fixture.Output.ShouldBe("\u001b[3;90mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Decoration_If_Set_To_None()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);

            // When
            fixture.Console.Write(
                "Hello",
                Style.WithForeground(Color.RoyalBlue1)
                     .WithBackground(Color.NavajoWhite1)
                     .WithDecoration(Decoration.None));

            // Then
            fixture.Output.ShouldBe("\u001b[90;47mHello\u001b[0m");
        }

        public sealed class WriteLine
        {
            [Fact]
            public void Should_Reset_Colors_Correctly_After_Line_Break()
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                fixture.Console.WriteLine("Hello", Style.WithBackground(ConsoleColor.Red));
                fixture.Console.WriteLine("World", Style.WithBackground(ConsoleColor.Green));

                // Then
                fixture.Output.NormalizeLineEndings()
                    .ShouldBe("[101mHello[0m\n[102mWorld[0m\n");
            }

            [Fact]
            public void Should_Reset_Colors_Correctly_After_Line_Break_In_Text()
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, AnsiSupport.Yes);

                // When
                fixture.Console.WriteLine("Hello\nWorld", Style.WithBackground(ConsoleColor.Red));

                // Then
                fixture.Output.NormalizeLineEndings()
                    .ShouldBe("[101mHello[0m\n[101mWorld[0m\n");
            }
        }
    }
}
