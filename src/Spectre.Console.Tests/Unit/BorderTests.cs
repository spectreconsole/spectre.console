using Shouldly;
using Spectre.Console.Rendering;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class BorderTests
    {
        public sealed class NoBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.None.Visible;

                // Then
                visibility.ShouldBeFalse();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.None.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.None);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().NoBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("Header 1 Header 2");
                console.Lines[1].ShouldBe("Cell     Cell    ");
                console.Lines[2].ShouldBe("Cell     Cell    ");
            }
        }

        public sealed class AsciiBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Ascii.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Ascii.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Ascii);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().AsciiBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("+---------------------+");
                console.Lines[1].ShouldBe("| Header 1 | Header 2 |");
                console.Lines[2].ShouldBe("|----------+----------|");
                console.Lines[3].ShouldBe("| Cell     | Cell     |");
                console.Lines[4].ShouldBe("| Cell     | Cell     |");
                console.Lines[5].ShouldBe("+---------------------+");
            }
        }

        public sealed class Ascii2Border
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Ascii2.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Ascii2.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Ascii2);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().Ascii2Border();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("+----------+----------+");
                console.Lines[1].ShouldBe("| Header 1 | Header 2 |");
                console.Lines[2].ShouldBe("|----------+----------|");
                console.Lines[3].ShouldBe("| Cell     | Cell     |");
                console.Lines[4].ShouldBe("| Cell     | Cell     |");
                console.Lines[5].ShouldBe("+----------+----------+");
            }
        }

        public sealed class AsciiDoubleHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.AsciiDoubleHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.AsciiDoubleHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.AsciiDoubleHead);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().AsciiDoubleHeadBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("+----------+----------+");
                console.Lines[1].ShouldBe("| Header 1 | Header 2 |");
                console.Lines[2].ShouldBe("|==========+==========|");
                console.Lines[3].ShouldBe("| Cell     | Cell     |");
                console.Lines[4].ShouldBe("| Cell     | Cell     |");
                console.Lines[5].ShouldBe("+----------+----------+");
            }
        }

        public sealed class SquareBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Square.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Square.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().SquareBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("┌──────────┬──────────┐");
                console.Lines[1].ShouldBe("│ Header 1 │ Header 2 │");
                console.Lines[2].ShouldBe("├──────────┼──────────┤");
                console.Lines[3].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[4].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[5].ShouldBe("└──────────┴──────────┘");
            }
        }

        public sealed class RoundedBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Rounded.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Rounded.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().RoundedBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("╭──────────┬──────────╮");
                console.Lines[1].ShouldBe("│ Header 1 │ Header 2 │");
                console.Lines[2].ShouldBe("├──────────┼──────────┤");
                console.Lines[3].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[4].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[5].ShouldBe("╰──────────┴──────────╯");
            }
        }

        public sealed class MinimalBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Minimal.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Minimal.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Minimal);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().MinimalBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("                       ");
                console.Lines[1].ShouldBe("  Header 1 │ Header 2  ");
                console.Lines[2].ShouldBe(" ──────────┼────────── ");
                console.Lines[3].ShouldBe("  Cell     │ Cell      ");
                console.Lines[4].ShouldBe("  Cell     │ Cell      ");
                console.Lines[5].ShouldBe("                       ");
            }
        }

        public sealed class MinimalHeavyHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.MinimalHeavyHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.MinimalHeavyHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Minimal);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().MinimalHeavyHeadBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("                       ");
                console.Lines[1].ShouldBe("  Header 1 │ Header 2  ");
                console.Lines[2].ShouldBe(" ━━━━━━━━━━┿━━━━━━━━━━ ");
                console.Lines[3].ShouldBe("  Cell     │ Cell      ");
                console.Lines[4].ShouldBe("  Cell     │ Cell      ");
                console.Lines[5].ShouldBe("                       ");
            }
        }

        public sealed class MinimalDoubleHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.MinimalDoubleHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.MinimalDoubleHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.MinimalDoubleHead);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().MinimalDoubleHeadBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("                       ");
                console.Lines[1].ShouldBe("  Header 1 │ Header 2  ");
                console.Lines[2].ShouldBe(" ══════════╪══════════ ");
                console.Lines[3].ShouldBe("  Cell     │ Cell      ");
                console.Lines[4].ShouldBe("  Cell     │ Cell      ");
                console.Lines[5].ShouldBe("                       ");
            }
        }

        public sealed class SimpleBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Simple.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Simple.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Simple);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().SimpleBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("                       ");
                console.Lines[1].ShouldBe("  Header 1   Header 2  ");
                console.Lines[2].ShouldBe("───────────────────────");
                console.Lines[3].ShouldBe("  Cell       Cell      ");
                console.Lines[4].ShouldBe("  Cell       Cell      ");
                console.Lines[5].ShouldBe("                       ");
            }
        }

        public sealed class HorizontalBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Horizontal.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Horizontal.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Horizontal);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().HorizontalBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("───────────────────────");
                console.Lines[1].ShouldBe("  Header 1   Header 2  ");
                console.Lines[2].ShouldBe("───────────────────────");
                console.Lines[3].ShouldBe("  Cell       Cell      ");
                console.Lines[4].ShouldBe("  Cell       Cell      ");
                console.Lines[5].ShouldBe("───────────────────────");
            }
        }

        public sealed class SimpleHeavyBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.SimpleHeavy.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.SimpleHeavy.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Simple);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().SimpleHeavyBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("                       ");
                console.Lines[1].ShouldBe("  Header 1   Header 2  ");
                console.Lines[2].ShouldBe("━━━━━━━━━━━━━━━━━━━━━━━");
                console.Lines[3].ShouldBe("  Cell       Cell      ");
                console.Lines[4].ShouldBe("  Cell       Cell      ");
                console.Lines[5].ShouldBe("                       ");
            }
        }

        public sealed class HeavyBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Heavy.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Heavy.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().HeavyBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("┏━━━━━━━━━━┳━━━━━━━━━━┓");
                console.Lines[1].ShouldBe("┃ Header 1 ┃ Header 2 ┃");
                console.Lines[2].ShouldBe("┣━━━━━━━━━━╋━━━━━━━━━━┫");
                console.Lines[3].ShouldBe("┃ Cell     ┃ Cell     ┃");
                console.Lines[4].ShouldBe("┃ Cell     ┃ Cell     ┃");
                console.Lines[5].ShouldBe("┗━━━━━━━━━━┻━━━━━━━━━━┛");
            }
        }

        public sealed class HeavyEdgeBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.HeavyEdge.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.HeavyEdge.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().HeavyEdgeBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("┏━━━━━━━━━━┯━━━━━━━━━━┓");
                console.Lines[1].ShouldBe("┃ Header 1 │ Header 2 ┃");
                console.Lines[2].ShouldBe("┠──────────┼──────────┨");
                console.Lines[3].ShouldBe("┃ Cell     │ Cell     ┃");
                console.Lines[4].ShouldBe("┃ Cell     │ Cell     ┃");
                console.Lines[5].ShouldBe("┗━━━━━━━━━━┷━━━━━━━━━━┛");
            }
        }

        public sealed class HeavyHeadBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.HeavyHead.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.HeavyHead.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().HeavyHeadBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("┏━━━━━━━━━━┳━━━━━━━━━━┓");
                console.Lines[1].ShouldBe("┃ Header 1 ┃ Header 2 ┃");
                console.Lines[2].ShouldBe("┡━━━━━━━━━━╇━━━━━━━━━━┩");
                console.Lines[3].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[4].ShouldBe("│ Cell     │ Cell     │");
                console.Lines[5].ShouldBe("└──────────┴──────────┘");
            }
        }

        public sealed class DoubleBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.Double.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.Double.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.Double);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().DoubleBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("╔══════════╦══════════╗");
                console.Lines[1].ShouldBe("║ Header 1 ║ Header 2 ║");
                console.Lines[2].ShouldBe("╠══════════╬══════════╣");
                console.Lines[3].ShouldBe("║ Cell     ║ Cell     ║");
                console.Lines[4].ShouldBe("║ Cell     ║ Cell     ║");
                console.Lines[5].ShouldBe("╚══════════╩══════════╝");
            }
        }

        public sealed class DoubleEdgeBorder
        {
            [Fact]
            public void Should_Return_Correct_Visibility()
            {
                // Given, When
                var visibility = Border.DoubleEdge.Visible;

                // Then
                visibility.ShouldBeTrue();
            }

            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = Border.DoubleEdge.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(Border.DoubleEdge);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var table = Fixture.GetTable().DoubleEdgeBorder();

                // When
                console.Render(table);

                // Then
                console.Lines.Count.ShouldBe(6);
                console.Lines[0].ShouldBe("╔══════════╤══════════╗");
                console.Lines[1].ShouldBe("║ Header 1 │ Header 2 ║");
                console.Lines[2].ShouldBe("╟──────────┼──────────╢");
                console.Lines[3].ShouldBe("║ Cell     │ Cell     ║");
                console.Lines[4].ShouldBe("║ Cell     │ Cell     ║");
                console.Lines[5].ShouldBe("╚══════════╧══════════╝");
            }
        }

        private static class Fixture
        {
            public static Table GetTable()
            {
                var table = new Table();
                table.AddColumns("Header 1", "Header 2");
                table.AddRow("Cell", "Cell");
                table.AddRow("Cell", "Cell");
                return table;
            }
        }
    }
}
