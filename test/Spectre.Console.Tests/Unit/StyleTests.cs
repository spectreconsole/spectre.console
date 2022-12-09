namespace Spectre.Console.Tests.Unit;

public sealed class StyleTests
{
    [Fact]
    public void Should_Combine_Two_Styles_As_Expected()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic);
        var other = new Style(Color.Green, Color.Silver, Decoration.Underline, "https://example.com");

        // When
        var result = first.Combine(other);

        // Then
        result.Foreground.ShouldBe(Color.Green);
        result.Background.ShouldBe(Color.Silver);
        result.Decoration.ShouldBe(Decoration.Bold | Decoration.Italic | Decoration.Underline);
        result.Link.ShouldBe("https://example.com");
    }

    [Fact]
    public void Should_Consider_Two_Identical_Styles_Equal()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");
        var second = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");

        // When
        var result = first.Equals(second);

        // Then
        result.ShouldBeTrue();
    }

    [Fact]
    public void Should_Not_Consider_Two_Styles_With_Different_Foreground_Colors_Equal()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");
        var second = new Style(Color.Blue, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");

        // When
        var result = first.Equals(second);

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Consider_Two_Styles_With_Different_Background_Colors_Equal()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");
        var second = new Style(Color.White, Color.Blue, Decoration.Bold | Decoration.Italic, "http://example.com");

        // When
        var result = first.Equals(second);

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Consider_Two_Styles_With_Different_Decorations_Equal()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");
        var second = new Style(Color.White, Color.Yellow, Decoration.Bold, "http://example.com");

        // When
        var result = first.Equals(second);

        // Then
        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Consider_Two_Styles_With_Different_Links_Equal()
    {
        // Given
        var first = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://example.com");
        var second = new Style(Color.White, Color.Yellow, Decoration.Bold | Decoration.Italic, "http://foo.com");

        // When
        var result = first.Equals(second);

        // Then
        result.ShouldBeFalse();
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
        [InlineData("b", Decoration.Bold)]
        [InlineData("dim", Decoration.Dim)]
        [InlineData("i", Decoration.Italic)]
        [InlineData("italic", Decoration.Italic)]
        [InlineData("underline", Decoration.Underline)]
        [InlineData("u", Decoration.Underline)]
        [InlineData("invert", Decoration.Invert)]
        [InlineData("conceal", Decoration.Conceal)]
        [InlineData("slowblink", Decoration.SlowBlink)]
        [InlineData("rapidblink", Decoration.RapidBlink)]
        [InlineData("strikethrough", Decoration.Strikethrough)]
        [InlineData("s", Decoration.Strikethrough)]
        public void Should_Parse_Decoration(string text, Decoration decoration)
        {
            // Given, When
            var result = Style.Parse(text);

            // Then
            result.ShouldNotBeNull();
            result.Decoration.ShouldBe(decoration);
        }

        [Fact]
        public void Should_Parse_Link_Without_Address()
        {
            // Given, When
            var result = Style.Parse("link");

            // Then
            result.ShouldNotBeNull();
            result.Link.ShouldBe("https://emptylink");
        }

        [Fact]
        public void Should_Parse_Link()
        {
            // Given, When
            var result = Style.Parse("link=https://example.com");

            // Then
            result.ShouldNotBeNull();
            result.Link.ShouldBe("https://example.com");
        }

        [Fact]
        public void Should_Throw_If_Link_Is_Set_Twice()
        {
            // Given, When
            var result = Record.Exception(() => Style.Parse("link=https://example.com link=https://example.com"));

            // Then
            result.ShouldBeOfType<InvalidOperationException>();
            result.Message.ShouldBe("A link has already been set.");
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

        [Fact]
        public void Should_Parse_Colors_And_Decoration_And_Link()
        {
            // Given, When
            var result = Style.Parse("link=https://example.com bold underline blue on green");

            // Then
            result.ShouldNotBeNull();
            result.Decoration.ShouldBe(Decoration.Bold | Decoration.Underline);
            result.Foreground.ShouldBe(Color.Blue);
            result.Background.ShouldBe(Color.Green);
            result.Link.ShouldBe("https://example.com");
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
        [InlineData("12 on 24")]
        public void Should_Parse_Colors_Numbers_Correctly(string style)
        {
            // Given, When
            var result = Style.Parse(style);

            // Then
            result.Foreground.ShouldBe(Color.Blue);
            result.Background.ShouldBe(Color.DeepSkyBlue4_1);
        }

        [Theory]
        [InlineData("-12", "Color number must be greater than or equal to 0 (was -12)")]
        [InlineData("256", "Color number must be less than or equal to 255 (was 256)")]
        public void Should_Return_Error_If_Color_Number_Is_Invalid(string style, string expected)
        {
            // Given, When
            var result = Record.Exception(() => Style.Parse(style));

            // Then
            result.ShouldNotBeNull();
            result.Message.ShouldBe(expected);
        }

        [Theory]
        [InlineData("rgb()", "Invalid RGB color 'rgb()'.")]
        [InlineData("rgb(", "Invalid RGB color 'rgb('.")]
        [InlineData("rgb(255)", "Invalid RGB color 'rgb(255)'.")]
        [InlineData("rgb(255,255)", "Invalid RGB color 'rgb(255,255)'.")]
        [InlineData("rgb(255,255,255", "Invalid RGB color 'rgb(255,255,255'.")]
        [InlineData("rgb(A,B,C)", "Invalid RGB color 'rgb(A,B,C)'.")]
        public void Should_Return_Error_If_Rgb_Color_Is_Invalid(string style, string expected)
        {
            // Given, When
            var result = Record.Exception(() => Style.Parse(style));

            // Then
            result.ShouldNotBeNull();
            result.Message.ShouldStartWith(expected);
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

    public sealed class TheToMarkupMethod
    {
        [Fact]
        public void Should_Return_Expected_Markup_For_Style_With_Foreground_Color()
        {
            // Given
            var style = new Style(Color.Red);

            // When
            var result = style.ToMarkup();

            // Then
            result.ShouldBe("red");
        }

        [Fact]
        public void Should_Return_Expected_Markup_For_Style_With_Foreground_And_Background_Color()
        {
            // Given
            var style = new Style(Color.Red, Color.Green);

            // When
            var result = style.ToMarkup();

            // Then
            result.ShouldBe("red on green");
        }

        [Fact]
        public void Should_Return_Expected_Markup_For_Style_With_Foreground_And_Background_Color_And_Decoration()
        {
            // Given
            var style = new Style(Color.Red, Color.Green, Decoration.Bold | Decoration.Underline);

            // When
            var result = style.ToMarkup();

            // Then
            result.ShouldBe("bold underline red on green");
        }

        [Fact]
        public void Should_Return_Expected_Markup_For_Style_With_Only_Background_Color()
        {
            // Given
            var style = new Style(background: Color.Green);

            // When
            var result = style.ToMarkup();

            // Then
            result.ShouldBe("default on green");
        }
    }
}
