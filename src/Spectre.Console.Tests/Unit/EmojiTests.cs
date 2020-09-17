using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class EmojiTests
    {
        [Fact]
        public void Should_Substitute_Emoji_Shortcodes_In_Markdown()
        {
            // Given
            var console = new TestableAnsiConsole(ColorSystem.Standard, AnsiSupport.Yes);

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
    }
}
