using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class ColorTests
    {
        public sealed class TheEqualsMethod
        {
            [Fact]
            public void Should_Consider_Color_And_Non_Color_Equal()
            {
                // Given
                var color1 = new Color(128, 0, 128);

                // When
                var result = color1.Equals("Foo");

                // Then
                result.ShouldBeFalse();
            }

            [Fact]
            public void Should_Consider_Same_Colors_Equal_By_Component()
            {
                // Given
                var color1 = new Color(128, 0, 128);
                var color2 = new Color(128, 0, 128);

                // When
                var result = color1.Equals(color2);

                // Then
                result.ShouldBeTrue();
            }

            [Fact]
            public void Should_Consider_Same_Known_Colors_Equal()
            {
                // Given
                var color1 = Color.Cyan1;
                var color2 = Color.Cyan1;

                // When
                var result = color1.Equals(color2);

                // Then
                result.ShouldBeTrue();
            }

            [Fact]
            public void Should_Consider_Known_Color_And_Color_With_Same_Components_Equal()
            {
                // Given
                var color1 = Color.Cyan1;
                var color2 = new Color(0, 255, 255);

                // When
                var result = color1.Equals(color2);

                // Then
                result.ShouldBeTrue();
            }

            [Fact]
            public void Should_Not_Consider_Different_Colors_Equal()
            {
                // Given
                var color1 = new Color(128, 0, 128);
                var color2 = new Color(128, 128, 128);

                // When
                var result = color1.Equals(color2);

                // Then
                result.ShouldBeFalse();
            }

            [Fact]
            public void Shourd_Not_Consider_Black_And_Default_Colors_Equal()
            {
                // Given
                var color1 = Color.Default;
                var color2 = Color.Black;

                // When
                var result = color1.Equals(color2);

                // Then
                result.ShouldBeFalse();
            }
        }

        public sealed class TheGetHashCodeMethod
        {
            [Fact]
            public void Should_Return_Same_HashCode_For_Same_Colors()
            {
                // Given
                var color1 = new Color(128, 0, 128);
                var color2 = new Color(128, 0, 128);

                // When
                var hash1 = color1.GetHashCode();
                var hash2 = color2.GetHashCode();

                // Then
                hash1.ShouldBe(hash2);
            }

            [Fact]
            public void Should_Return_Different_HashCode_For_Different_Colors()
            {
                // Given
                var color1 = new Color(128, 0, 128);
                var color2 = new Color(128, 128, 128);

                // When
                var hash1 = color1.GetHashCode();
                var hash2 = color2.GetHashCode();

                // Then
                hash1.ShouldNotBe(hash2);
            }
        }

        public sealed class ImplicitConversions
        {
            public sealed class Int32ToColor
            {
                public static IEnumerable<object[]> Data =>
                    Enumerable.Range(0, 255)
                        .Select(number => new object[] { number });

                [Theory]
                [MemberData(nameof(Data))]
                public void Should_Return_Expected_Color(int number)
                {
                    // Given, When
                    var result = (Color)number;

                    // Then
                    result.ShouldBe(Color.FromInt32(number));
                }

                [Fact]
                public void Should_Throw_If_Integer_Is_Lower_Than_Zero()
                {
                    // Given, When
                    var result = Record.Exception(() => (Color)(-1));

                    // Then
                    result.ShouldBeOfType<InvalidOperationException>();
                    result.Message.ShouldBe("Color number must be between 0 and 255");
                }

                [Fact]
                public void Should_Throw_If_Integer_Is_Higher_Than_255()
                {
                    // Given, When
                    var result = Record.Exception(() => (Color)256);

                    // Then
                    result.ShouldBeOfType<InvalidOperationException>();
                    result.Message.ShouldBe("Color number must be between 0 and 255");
                }
            }

            public sealed class ConsoleColorToColor
            {
                [Theory]
                [InlineData(ConsoleColor.Black, 0)]
                [InlineData(ConsoleColor.DarkRed, 1)]
                [InlineData(ConsoleColor.DarkGreen, 2)]
                [InlineData(ConsoleColor.DarkYellow, 3)]
                [InlineData(ConsoleColor.DarkBlue, 4)]
                [InlineData(ConsoleColor.DarkMagenta, 5)]
                [InlineData(ConsoleColor.DarkCyan, 6)]
                [InlineData(ConsoleColor.Gray, 7)]
                [InlineData(ConsoleColor.DarkGray, 8)]
                [InlineData(ConsoleColor.Red, 9)]
                [InlineData(ConsoleColor.Green, 10)]
                [InlineData(ConsoleColor.Yellow, 11)]
                [InlineData(ConsoleColor.Blue, 12)]
                [InlineData(ConsoleColor.Magenta, 13)]
                [InlineData(ConsoleColor.Cyan, 14)]
                [InlineData(ConsoleColor.White, 15)]
                public void Should_Return_Expected_Color(ConsoleColor color, int expected)
                {
                    // Given, When
                    var result = (Color)color;

                    // Then
                    result.ShouldBe(Color.FromInt32(expected));
                }
            }

            public sealed class ColorToConsoleColor
            {
                [Theory]
                [InlineData(0, ConsoleColor.Black)]
                [InlineData(1, ConsoleColor.DarkRed)]
                [InlineData(2, ConsoleColor.DarkGreen)]
                [InlineData(3, ConsoleColor.DarkYellow)]
                [InlineData(4, ConsoleColor.DarkBlue)]
                [InlineData(5, ConsoleColor.DarkMagenta)]
                [InlineData(6, ConsoleColor.DarkCyan)]
                [InlineData(7, ConsoleColor.Gray)]
                [InlineData(8, ConsoleColor.DarkGray)]
                [InlineData(9, ConsoleColor.Red)]
                [InlineData(10, ConsoleColor.Green)]
                [InlineData(11, ConsoleColor.Yellow)]
                [InlineData(12, ConsoleColor.Blue)]
                [InlineData(13, ConsoleColor.Magenta)]
                [InlineData(14, ConsoleColor.Cyan)]
                [InlineData(15, ConsoleColor.White)]
                public void Should_Return_Expected_ConsoleColor_For_Known_Color(int color, ConsoleColor expected)
                {
                    // Given, When
                    var result = (ConsoleColor)Color.FromInt32(color);

                    // Then
                    result.ShouldBe(expected);
                }
            }
        }

        public sealed class TheToMarkupMethod
        {
            [Fact]
            public void Should_Return_Expected_Markup_For_Default_Color()
            {
                // Given, When
                var result = Color.Default.ToMarkup();

                // Then
                result.ShouldBe("default");
            }

            [Fact]
            public void Should_Return_Expected_Markup_For_Known_Color()
            {
                // Given, When
                var result = Color.Red.ToMarkup();

                // Then
                result.ShouldBe("red");
            }

            [Fact]
            public void Should_Return_Expected_Markup_For_Custom_Color()
            {
                // Given, When
                var result = new Color(255, 1, 12).ToMarkup();

                // Then
                result.ShouldBe("#FF010C");
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Return_Color_Name_For_Known_Colors()
            {
                // Given, When
                var name = Color.Fuchsia.ToString();

                // Then
                name.ShouldBe("fuchsia");
            }

            [Fact]
            public void Should_Return_Hex_String_For_Unknown_Colors()
            {
                // Given, When
                var name = new Color(128, 0, 128).ToString();

                // Then
                name.ShouldBe("#800080 (RGB=128,0,128)");
            }
        }
    }
}
