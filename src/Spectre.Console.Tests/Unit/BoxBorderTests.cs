using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Rendering;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class BoxBorderTests
    {
        [UsesVerify]
        public sealed class NoBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxExtensions.GetSafeBorder(BoxBorder.None, safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.None);
                }
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().NoBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class AsciiBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxExtensions.GetSafeBorder(BoxBorder.Ascii, safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Ascii);
                }
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().AsciiBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class DoubleBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxExtensions.GetSafeBorder(BoxBorder.Double, safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Double);
                }
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().DoubleBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class HeavyBorder
        {
            public sealed class TheSafeGetBorderMethod
            {
                [Fact]
                public void Should_Return_Safe_Border()
                {
                    // Given, When
                    var border = BoxExtensions.GetSafeBorder(BoxBorder.Heavy, safe: true);

                    // Then
                    border.ShouldBeSameAs(BoxBorder.Square);
                }
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().HeavyBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class RoundedBorder
        {
            [Fact]
            public void Should_Return_Safe_Border()
            {
                // Given, When
                var border = BoxExtensions.GetSafeBorder(BoxBorder.Rounded, safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Square);
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().RoundedBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        [UsesVerify]
        public sealed class SquareBorder
        {
            [Fact]
            public void Should_Return_Safe_Border()
            {
                // Given, When
                var border = BoxExtensions.GetSafeBorder(BoxBorder.Square, safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Square);
            }

            [Fact]
            public Task Should_Render_As_Expected()
            {
                // Given
                var console = new PlainConsole();
                var panel = Fixture.GetPanel().SquareBorder();

                // When
                console.Render(panel);

                // Then
                return Verifier.Verify(console.Output);
            }
        }

        private static class Fixture
        {
            public static Panel GetPanel()
            {
                return new Panel("Hello World")
                    .Header("Greeting");
            }
        }
    }
}
