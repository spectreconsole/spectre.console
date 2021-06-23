using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Borders/Table")]
    public sealed class TableBorderTests
    {
        [UsesVerify]
        public sealed class NoBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.None.Visible;

                // Then
                visibility.ShouldBeFalse();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.None.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.None);
                }
            }

            [Fact]
            [Expectation("NoBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().NoBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class AsciiBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Ascii.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Ascii.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Ascii);
                }
            }

            [Fact]
            [Expectation("AsciiBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().AsciiBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class Ascii2Border
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Ascii2.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Ascii2.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Ascii2);
                }
            }

            [Fact]
            [Expectation("Ascii2Border")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().Ascii2Border();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class AsciiDoubleHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.AsciiDoubleHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.AsciiDoubleHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.AsciiDoubleHead);
                }
            }

            [Fact]
            [Expectation("AsciiDoubleHeadBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().AsciiDoubleHeadBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class SquareBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Square.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Square.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Square);
                }
            }

            [Fact]
            [Expectation("SquareBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().SquareBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class RoundedBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Rounded.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Rounded.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Square);
                }
            }

            [Fact]
            [Expectation("RoundedBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().RoundedBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class MinimalBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Minimal.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Minimal.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Minimal);
                }
            }

            [Fact]
            [Expectation("MinimalBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().MinimalBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class MinimalHeavyHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.MinimalHeavyHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.MinimalHeavyHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Minimal);
                }
            }

            [Fact]
            [Expectation("MinimalHeavyHeadBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().MinimalHeavyHeadBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class MinimalDoubleHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.MinimalDoubleHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.MinimalDoubleHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.MinimalDoubleHead);
                }
            }

            [Fact]
            [Expectation("MinimalDoubleHeadBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().MinimalDoubleHeadBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class SimpleBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Simple.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Simple.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Simple);
                }
            }

            [Fact]
            [Expectation("SimpleBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().SimpleBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class HorizontalBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Horizontal.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Horizontal.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Horizontal);
                }
            }

            [Fact]
            [Expectation("HorizontalBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().HorizontalBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class SimpleHeavyBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.SimpleHeavy.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.SimpleHeavy.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Simple);
                }
            }

            [Fact]
            [Expectation("SimpleHeavyBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().SimpleHeavyBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class HeavyBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Heavy.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Heavy.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Square);
                }
            }

            [Fact]
            [Expectation("HeavyBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().HeavyBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class HeavyEdgeBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.HeavyEdge.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.HeavyEdge.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Square);
                }
            }

            [Fact]
            [Expectation("HeavyEdgeBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().HeavyEdgeBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class HeavyHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.HeavyHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.HeavyHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Square);
                }
            }

            [Fact]
            [Expectation("HeavyHeadBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().HeavyHeadBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class DoubleBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Double.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Double.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Double);
                }
            }

            [Fact]
            [Expectation("DoubleBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().DoubleBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class DoubleEdgeBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.DoubleEdge.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.DoubleEdge.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.DoubleEdge);
                }
            }

            [Fact]
            [Expectation("DoubleEdgeBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().DoubleEdgeBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class MarkdownBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = TableBorder.Markdown.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = TableBorder.Markdown.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(TableBorder.Markdown);
                }
            }

            [Fact]
            [Expectation("MarkdownBorder")]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable().MarkdownBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }

            [Fact]
            [Expectation("MarkdownBorder_LeftAligned")]
            public Task Should_Render_Left_Aligned_Table_Columns_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable(header2: Justify.Left).MarkdownBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }

            [Fact]
            [Expectation("MarkdownBorder_Centered")]
            public Task Should_Render_Center_Aligned_Table_Columns_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable(header2: Justify.Center).MarkdownBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }

            [Fact]
            [Expectation("MarkdownBorder_RightAligned")]
            public Task Should_Render_Right_Aligned_Table_Columns_As_Expected()
            {
                // Given
                var console = new TestConsole();
                var table = Fixture.GetTable(header2: Justify.Right).MarkdownBorder();

                // When
                console.Write(table);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        private static class Fixture
        {
            public static Table GetTable(Justify? header1 = null, Justify? header2 = null)
            {
                var table = new Table();
                table.AddColumn("Header 1", c => c.Alignment(header1).Footer("Footer 1"));
                table.AddColumn("Header 2", c => c.Alignment(header2).Footer("Footer 2"));
                table.AddRow("Cell", "Cell");
                table.AddRow("Cell", "Cell");
                return table;
            }
        }
    }
}
