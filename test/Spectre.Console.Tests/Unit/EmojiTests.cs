using Shouldly;
using Spectre.Console.Testing;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class EmojiTests
    {
        [Fact]
        public void Should_Substitute_Emoji_Shortcodes_In_Markdown()
        {
            // Given
            var console = new TestConsole();

            // When
            console.Markup("Hello :globe_showing_europe_africa:!");

            // Then
            console.Output.ShouldBe("Hello 🌍!");
        }

        [Fact]
        public void Should_Contain_Predefined_Emojis()
        {
            // Given, When
            const string result = "Hello " + Emoji.Known.GlobeShowingEuropeAfrica + "!";

            // Then
            result.ShouldBe("Hello 🌍!");
        }

        public sealed class TheReplaceMethod
        {
            [Fact]
            public void Should_Replace_Emojis_In_Text()
            {
                // Given, When
                var result = Emoji.Replace("Hello :globe_showing_europe_africa:!");

                // Then
                result.ShouldBe("Hello 🌍!");
            }
        }

        public sealed class Parsing
        {
            [Theory]
            [InlineData(":", ":")]
            [InlineData("::", "::")]
            [InlineData(":::", ":::")]
            [InlineData("::::", "::::")]
            [InlineData("::i:", "::i:")]
            [InlineData(":i:i:", ":i:i:")]
            [InlineData("::globe_showing_europe_africa::", ":🌍:")]
            [InlineData(":globe_showing_europe_africa::globe_showing_europe_africa:", "🌍🌍")]
            [InlineData("::globe_showing_europe_africa:::test:::globe_showing_europe_africa:::", ":🌍::test::🌍::")]
            public void Can_Handle_Different_Combinations(string markup, string expected)
            {
                // Given
                var console = new TestConsole();

                // When
                console.Markup(markup);

                // Then
                console.Output.ShouldBe(expected);
            }

            [Fact]
            public void Should_Leave_Single_Colons()
            {
                // Given
                var console = new TestConsole();

                // When
                console.Markup("Hello :globe_showing_europe_africa:! Output: good");

                // Then
                console.Output.ShouldBe("Hello 🌍! Output: good");
            }

            [Fact]
            public void Unknown_emojis_should_remain_unchanged()
            {
                // Given
                var console = new TestConsole();

                // When
                console.Markup("Hello :globe_showing_flat_earth:!");

                // Then
                console.Output.ShouldBe("Hello :globe_showing_flat_earth:!");
            }
        }
    }
}
