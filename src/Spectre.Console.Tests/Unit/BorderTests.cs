using Shouldly;
using Xunit;
using Spectre.Console.Rendering;

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
        }
    }
}
