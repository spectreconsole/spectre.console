namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Rendering/Borders/Box")]
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
                var border = BoxBorder.None.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.None);
            }
        }

        [Fact]
        [Expectation("NoBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().NoBorder();

            // When
            console.Write(panel);

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
                var border = BoxBorder.Ascii.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Ascii);
            }
        }

        [Fact]
        [Expectation("AsciiBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().AsciiBorder();

            // When
            console.Write(panel);

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
                var border = BoxBorder.Double.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Double);
            }
        }

        [Fact]
        [Expectation("DoubleBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().DoubleBorder();

            // When
            console.Write(panel);

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
                var border = BoxBorder.Heavy.GetSafeBorder(safe: true);

                // Then
                border.ShouldBeSameAs(BoxBorder.Square);
            }
        }

        [Fact]
        [Expectation("HeavyBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().HeavyBorder();

            // When
            console.Write(panel);

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
            var border = BoxBorder.Rounded.GetSafeBorder(safe: true);

            // Then
            border.ShouldBeSameAs(BoxBorder.Square);
        }

        [Fact]
        [Expectation("RoundedBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().RoundedBorder();

            // When
            console.Write(panel);

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
            var border = BoxBorder.Square.GetSafeBorder(safe: true);

            // Then
            border.ShouldBeSameAs(BoxBorder.Square);
        }

        [Fact]
        [Expectation("SquareBorder")]
        public Task Should_Render_As_Expected()
        {
            // Given
            var console = new TestConsole();
            var panel = Fixture.GetPanel().SquareBorder();

            // When
            console.Write(panel);

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
