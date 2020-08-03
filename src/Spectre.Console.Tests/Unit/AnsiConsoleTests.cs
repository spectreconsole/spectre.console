using System;
using System.Globalization;
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
            fixture.Console.Foreground = Color.RoyalBlue1;
            fixture.Console.Background = Color.NavajoWhite1;
            fixture.Console.Decoration = Decoration.Italic;

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
            fixture.Console.Decoration = Decoration.Italic;

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
            fixture.Console.Decoration = Decoration.Italic;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[3;90mHello\u001b[0m");
        }

        [Fact]
        public void Should_Not_Include_Decoration_If_Set_To_None()
        {
            // Given
            var fixture = new AnsiConsoleFixture(ColorSystem.Standard);
            fixture.Console.Foreground = Color.RoyalBlue1;
            fixture.Console.Background = Color.NavajoWhite1;
            fixture.Console.Decoration = Decoration.None;

            // When
            fixture.Console.Write("Hello");

            // Then
            fixture.Output.ShouldBe("\u001b[90;47mHello\u001b[0m");
        }

        public sealed class Write
        {
            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Int32_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32);

                // Then
                fixture.Output.ShouldBe("32");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_UInt32_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32U);

                // Then
                fixture.Output.ShouldBe("32");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Int64_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32L);

                // Then
                fixture.Output.ShouldBe("32");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_UInt64_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32UL);

                // Then
                fixture.Output.ShouldBe("32");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Single_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32.432F);

                // Then
                fixture.Output.ShouldBe("32.432");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Double_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, (double)32.432);

                // Then
                fixture.Output.ShouldBe("32.432");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Decimal_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 32.432M);

                // Then
                fixture.Output.ShouldBe("32.432");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Boolean_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, true);

                // Then
                fixture.Output.ShouldBe("True");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Char_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(CultureInfo.InvariantCulture, 'P');

                // Then
                fixture.Output.ShouldBe("P");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Char_Array_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(
                    CultureInfo.InvariantCulture,
                    new[] { 'P', 'a', 't', 'r', 'i', 'k' });

                // Then
                fixture.Output.ShouldBe("Patrik");
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Formatted_String_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.Write(
                    CultureInfo.InvariantCulture,
                    "Hello {0}! {1}",
                    "World", 32);

                // Then
                fixture.Output.ShouldBe("Hello World! 32");
            }
        }

        public sealed class WriteLine
        {
            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Int32_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32);

                // Then
                fixture.Output.ShouldBe("32" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_UInt32_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32U);

                // Then
                fixture.Output.ShouldBe("32" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Int64_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32L);

                // Then
                fixture.Output.ShouldBe("32" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_UInt64_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32UL);

                // Then
                fixture.Output.ShouldBe("32" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Single_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32.432F);

                // Then
                fixture.Output.ShouldBe("32.432" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Double_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, (double)32.432);

                // Then
                fixture.Output.ShouldBe("32.432" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Decimal_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 32.432M);

                // Then
                fixture.Output.ShouldBe("32.432" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Boolean_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, true);

                // Then
                fixture.Output.ShouldBe("True" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Char_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(CultureInfo.InvariantCulture, 'P');

                // Then
                fixture.Output.ShouldBe("P" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Char_Array_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(
                    CultureInfo.InvariantCulture,
                    new[] { 'P', 'a', 't', 'r', 'i', 'k' });

                // Then
                fixture.Output.ShouldBe("Patrik" + Environment.NewLine);
            }

            [Theory]
            [InlineData(AnsiSupport.Yes)]
            [InlineData(AnsiSupport.No)]
            public void Should_Write_Formatted_String_With_Format_Provider(AnsiSupport ansi)
            {
                // Given
                var fixture = new AnsiConsoleFixture(ColorSystem.Standard, ansi);

                // When
                fixture.Console.WriteLine(
                    CultureInfo.InvariantCulture,
                    "Hello {0}! {1}",
                    "World", 32);

                // Then
                fixture.Output.ShouldBe("Hello World! 32" + Environment.NewLine);
            }
        }
    }
}
