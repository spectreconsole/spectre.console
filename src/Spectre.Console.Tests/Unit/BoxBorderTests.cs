using Shouldly;
using Spectre.Console.Rendering;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class BoxBorderTests
    {
        public sealed class NoBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxBorder.None.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.None);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().NoBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("  Greeting     ");
                console.Lines[1].ShouldBe("  Hello World  ");
                console.Lines[2].ShouldBe("               ");
            }
        }

        public sealed class AsciiBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxBorder.Ascii.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Ascii);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().AsciiBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("+-Greeting----+");
                console.Lines[1].ShouldBe("| Hello World |");
                console.Lines[2].ShouldBe("+-------------+");
            }
        }

        public sealed class DoubleBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxBorder.Double.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Double);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().DoubleBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("╔═Greeting════╗");
                console.Lines[1].ShouldBe("║ Hello World ║");
                console.Lines[2].ShouldBe("╚═════════════╝");
            }
        }

        public sealed class HeavyBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxBorder.Heavy.GetSafeBorder(safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Square);
                }
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().HeavyBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("┏━Greeting━━━━┓");
                console.Lines[1].ShouldBe("┃ Hello World ┃");
                console.Lines[2].ShouldBe("┗━━━━━━━━━━━━━┛");
            }
        }

        public sealed class RoundedBorder
        {
            [Fact]
            public void Should_Return_Safe_Border()
            {
                // Given, When
                var border = BoxBorder.Rounded.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Square);
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().RoundedBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("╭─Greeting────╮");
                console.Lines[1].ShouldBe("│ Hello World │");
                console.Lines[2].ShouldBe("╰─────────────╯");
            }
        }

        public sealed class SquareBorder
        {
            [Fact]
            public void Should_Return_Safe_Border()
            {
                // Given, When
                var border = BoxBorder.Square.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Square);
            }

            [Fact]
            public void Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().SquareBorder();

                // When
                console.Render(panel);

                // Then
                console.Lines.Count.ShouldBe(3);
                console.Lines[0].ShouldBe("┌─Greeting────┐");
                console.Lines[1].ShouldBe("│ Hello World │");
                console.Lines[2].ShouldBe("└─────────────┘");
            }
        }

        private static class Fixture
        {
            public static Panel GetPanel()
            {
                return new Panel("Hello World")
                    .SetHeader("Greeting");
            }
        }
    }
}
