using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class SegmentTests
    {
        [UsesVerify]
        public sealed class TheSplitMethod
        {
            [Theory]
            [InlineData("Foo Bar", 0, "", "Foo Bar")]
            [InlineData("Foo Bar", 1, "F", "oo Bar")]
            [InlineData("Foo Bar", 2, "Fo", "o Bar")]
            [InlineData("Foo Bar", 3, "Foo", " Bar")]
            [InlineData("Foo Bar", 4, "Foo ", "Bar")]
            [InlineData("Foo Bar", 5, "Foo B", "ar")]
            [InlineData("Foo Bar", 6, "Foo Ba", "r")]
            [InlineData("Foo Bar", 7, "Foo Bar", null)]
            [InlineData("Foo 测试 Bar", 0, "", "Foo 测试 Bar")]
            [InlineData("Foo 测试 Bar", 1, "F", "oo 测试 Bar")]
            [InlineData("Foo 测试 Bar", 2, "Fo", "o 测试 Bar")]
            [InlineData("Foo 测试 Bar", 3, "Foo", " 测试 Bar")]
            [InlineData("Foo 测试 Bar", 4, "Foo ", "测试 Bar")]
            [InlineData("Foo 测试 Bar", 5, "Foo 测", "试 Bar")]
            [InlineData("Foo 测试 Bar", 6, "Foo 测", "试 Bar")]
            [InlineData("Foo 测试 Bar", 7, "Foo 测试", " Bar")]
            [InlineData("Foo 测试 Bar", 8, "Foo 测试", " Bar")]
            [InlineData("Foo 测试 Bar", 9, "Foo 测试 ", "Bar")]
            [InlineData("Foo 测试 Bar", 10, "Foo 测试 B", "ar")]
            [InlineData("Foo 测试 Bar", 11, "Foo 测试 Ba", "r")]
            [InlineData("Foo 测试 Bar", 12, "Foo 测试 Bar", null)]
            public void Should_Split_Segment_Correctly(string text, int offset, string expectedFirst, string expectedSecond)
            {
                // Given
                var style = new Style(Color.Red, Color.Green, Decoration.Bold);
                var segment = new Segment(text, style);

                // When
                var (first, second) = segment.Split(offset);

                // Then
                first.Text.ShouldBe(expectedFirst);
                first.Style.ShouldBe(style);
                second?.Text?.ShouldBe(expectedSecond);
                second?.Style?.ShouldBe(style);
            }
        }

        [UsesVerify]
        public sealed class TheSplitLinesMethod
        {
            [Fact]
            [Expectation("Segment", "Split")]
            public Task Should_Split_Segment()
            {
                // Given, When
                var lines = Segment.SplitLines(
                    new[]
                    {
                        new Segment("Foo"),
                        new Segment("Bar"),
                        new Segment("\n"),
                        new Segment("Baz"),
                        new Segment("Qux"),
                        new Segment("\n"),
                        new Segment("Corgi"),
                    });

                // Then
                lines.Count.ShouldBe(3);

                lines[0].Count.ShouldBe(2);
                lines[0][0].Text.ShouldBe("Foo");
                lines[0][1].Text.ShouldBe("Bar");

                lines[1].Count.ShouldBe(2);
                lines[1][0].Text.ShouldBe("Baz");
                lines[1][1].Text.ShouldBe("Qux");

                lines[2].Count.ShouldBe(1);
                lines[2][0].Text.ShouldBe("Corgi");
                return Verifier.Verify(lines);
            }

            [Fact]
            [Expectation("Segment", "Split_Linebreak")]
            public Task Should_Split_Segments_With_Linebreak_In_Text()
            {
                // Given, Given
                var lines = Segment.SplitLines(
                    new[]
                    {
                        new Segment("Foo\n"),
                        new Segment("Bar\n"),
                        new Segment("Baz"),
                        new Segment("Qux\n"),
                        new Segment("Corgi"),
                    });

                // Then
                return Verifier.Verify(lines);
            }
        }
    }
}
