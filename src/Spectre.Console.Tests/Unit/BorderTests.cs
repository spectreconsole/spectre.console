using System;
using Shouldly;
using Spectre.Console.Rendering;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class BorderTests
    {
        public sealed class TheGetBorderMethod
        {
            [Theory]
            [InlineData(BorderKind.None, false, typeof(NoBorder))]
            [InlineData(BorderKind.Ascii, false, typeof(AsciiBorder))]
            [InlineData(BorderKind.Square, false, typeof(SquareBorder))]
            [InlineData(BorderKind.Rounded, false, typeof(RoundedBorder))]
            [InlineData(BorderKind.None, true, typeof(NoBorder))]
            [InlineData(BorderKind.Ascii, true, typeof(AsciiBorder))]
            [InlineData(BorderKind.Square, true, typeof(SquareBorder))]
            [InlineData(BorderKind.Rounded, true, typeof(SquareBorder))]
            public void Should_Return_Correct_Border_For_Specified_Kind(BorderKind kind, bool safe, Type expected)
            {
                // Given, When
                var result = Border.GetBorder(kind, safe);

                // Then
                result.ShouldBeOfType(expected);
            }

            [Fact]
            public void Should_Throw_If_Unknown_Border_Kind_Is_Specified()
            {
                // Given, When
                var result = Record.Exception(() => Border.GetBorder((BorderKind)int.MaxValue, false));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Unknown border kind");
            }
        }
    }
}
