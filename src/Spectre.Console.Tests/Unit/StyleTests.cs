using System;
using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class StyleTests
    {
        [Fact]
        public void Should_Combine_Two_Styles_As_Expected()
        {
            // Given
            var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic);
            var other = new Style(Color.Green, Color.Silver, Decoration.Underline);

            // When
            var result = first.Combine(other);

            // Then
            result.Foreground.ShouldBe(Color.Green);
            result.Background.ShouldBe(Color.Silver);
            result.Decoration.ShouldBe(Decoration.Bold | Decoration.Italic | Decoration.Underline);
        }

        public sealed class TheParseMethod
        {
            [Fact]
            public void Default_Keyword_Should_Return_Default_Style()
            {
                // Given, When
                var result = Style.Parse("default");

                // Then
                result.ShouldNotBeNull();
                result.Foreground.ShouldBe(Color.Default);
                result.Background.ShouldBe(Color.Default);
                result.Decoration.ShouldBe(Decoration.None);
            }

            [Theory]
            [InlineData("bold", Decoration.Bold)]
            [InlineData("dim", Decoration.Dim)]
            [InlineData("italic", Decoration.Italic)]
            [InlineData("underline", Decoration.Underline)]
            [InlineData("invert", Decoration.Invert)]
            [InlineData("conceal", Decoration.Conceal)]
            [InlineData("slowblink", Decoration.SlowBlink)]
            [InlineData("rapidblink", Decoration.RapidBlink)]
            [InlineData("strikethrough", Decoration.Strikethrough)]
            public void Should_Parse_Decoration(string text, Decoration decoration)
            {
                // Given, When
                var result = Style.Parse(text);

                // Then
                result.ShouldNotBeNull();
                result.Decoration.ShouldBe(decoration);
            }

            [Fact]
            public void Should_Parse_Text_And_Decoration()
            {
                // Given, When
                var result = Style.Parse("bold underline blue on green");

                // Then
                result.ShouldNotBeNull();
                result.Decoration.ShouldBe(Decoration.Bold | Decoration.Underline);
                result.Foreground.ShouldBe(Color.Blue);
                result.Background.ShouldBe(Color.Green);
            }

            [Fact]
            public void Should_Parse_Background_If_Foreground_Is_Set_To_Default()
            {
                // Given, When
                var result = Style.Parse("default on green");

                // Then
                result.ShouldNotBeNull();
                result.Decoration.ShouldBe(Decoration.None);
                result.Foreground.ShouldBe(Color.Default);
                result.Background.ShouldBe(Color.Green);
            }

            [Fact]
            public void Should_Throw_If_Foreground_Is_Set_Twice()
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse("green yellow"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("A foreground color has already been set.");
            }

            [Fact]
            public void Should_Throw_If_Background_Is_Set_Twice()
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse("green on blue yellow"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("A background color has already been set.");
            }

            [Fact]
            public void Should_Throw_If_Color_Name_Could_Not_Be_Found()
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse("bold lol"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Could not find color or style 'lol'.");
            }

            [Fact]
            public void Should_Throw_If_Background_Color_Name_Could_Not_Be_Found()
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse("blue on lol"));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Could not find color 'lol'.");
            }

            [Theory]
            [InlineData("#FF0000 on #0000FF")]
            [InlineData("#F00 on #00F")]
            public void Should_Parse_Hex_Colors_Correctly(string style)
            {
                // Given, When
                var result = Style.Parse(style);

                // Then
                result.Foreground.ShouldBe(Color.Red);
                result.Background.ShouldBe(Color.Blue);
            }

            [Theory]
            [InlineData("#", "Invalid hex color '#'.")]
            [InlineData("#FF00FF00FF", "Invalid hex color '#FF00FF00FF'.")]
            [InlineData("#FOO", "Invalid hex color '#FOO'. Could not find any recognizable digits.")]
            public void Should_Return_Error_If_Hex_Color_Is_Invalid(string style, string expected)
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse(style));

                // Then
                result.ShouldNotBeNull();
                result.Message.ShouldBe(expected);
            }

            [Theory]
            [InlineData("rgb(255,0,0) on rgb(0,0,255)")]
            public void Should_Parse_Rgb_Colors_Correctly(string style)
            {
                // Given, When
                var result = Style.Parse(style);

                // Then
                result.Foreground.ShouldBe(Color.Red);
                result.Background.ShouldBe(Color.Blue);
            }

            [Theory]
            [InlineData("rgb()", "Invalid RGB color 'rgb()'.")]
            [InlineData("rgb(", "Invalid RGB color 'rgb('.")]
            [InlineData("rgb(255)", "Invalid RGB color 'rgb(255)'.")]
            [InlineData("rgb(255,255)", "Invalid RGB color 'rgb(255,255)'.")]
            [InlineData("rgb(255,255,255", "Invalid RGB color 'rgb(255,255,255'.")]
            [InlineData("rgb(A,B,C)", "Invalid RGB color 'rgb(A,B,C)'. Input string was not in a correct format.")]
            public void Should_Return_Error_If_Rgb_Color_Is_Invalid(string style, string expected)
            {
                // Given, When
                var result = Record.Exception(() => Style.Parse(style));

                // Then
                result.ShouldNotBeNull();
                result.Message.ShouldBe(expected);
            }
        }

        public sealed class TheTryParseMethod
        {
            [Fact]
            public void Should_Return_True_If_Parsing_Succeeded()
            {
                // Given, When
                var result = Style.TryParse("bold", out var style);

                // Then
                result.ShouldBeTrue();
                style.ShouldNotBeNull();
                style.Decoration.ShouldBe(Decoration.Bold);
            }

            [Fact]
            public void Should_Return_False_If_Parsing_Failed()
            {
                // Given, When
                var result = Style.TryParse("lol", out _);

                // Then
                result.ShouldBeFalse();
            }
        }
    }
}
