using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class AppearanceTests
    {
        [Fact]
        public void Should_Combine_Two_Appearances_As_Expected()
        {
            // Given
            var first = new Appearance(Color.White, Color.Yellow, Styles.Bold | Styles.Italic);
            var other = new Appearance(Color.Green, Color.Silver, Styles.Underline);

            // When
            var result = first.Combine(other);

            // Then
            result.Foreground.ShouldBe(Color.Green);
            result.Background.ShouldBe(Color.Silver);
            result.Style.ShouldBe(Styles.Bold | Styles.Italic | Styles.Underline);
        }
    }
}
