using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Rendering;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class SegmentTests
    {
        [Fact]
        public void Lol()
        {
            var context = new RenderContext(Encoding.UTF8, false);

            var result = new Segment("    ").CellCount(context);

            result.ShouldBe(4);
        }

        public sealed class TheSplitMethod
        {
            [Fact]
            public void Should_Split_Segment_Correctly()
            {
                // Given
                var style = new Style(Color.Red, Color.Green, Decoration.Bold);
                var segment = new Segment("Foo Bar", style);

                // When
                var (first, second) = segment.Split(3);

                // Then
                first.Text.ShouldBe("Foo");
                first.Style.ShouldBe(style);
                second.Text.ShouldBe(" Bar");
                second.Style.ShouldBe(style);
            }
        }

        [UsesVerify]
        public sealed class TheSplitLinesMethod
        {
            [Fact]
            public void Should_Split_Segment()
            {
                var context = new RenderContext(Encoding.UTF8, false);

                var lines = Segment.SplitLines(
                    context,
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
            }

            [Fact]
            public Task Should_Split_Segments_With_Linebreak_In_Text()
            {
                var context = new RenderContext(Encoding.UTF8, false);
                var lines = Segment.SplitLines(
                    context,
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
