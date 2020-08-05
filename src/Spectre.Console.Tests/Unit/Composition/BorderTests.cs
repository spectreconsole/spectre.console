using System;
using Shouldly;
using Spectre.Console.Composition;
using Xunit;

namespace Spectre.Console.Tests.Unit.Composition
{
    public sealed class BorderTests
    {
        public sealed class TheGetBorderMethod
        {
            [Theory]
            [InlineData(BorderKind.Ascii, typeof(AsciiBorder))]
            [InlineData(BorderKind.Square, typeof(SquareBorder))]
            [InlineData(BorderKind.Rounded, typeof(RoundedBorder))]
            public void Should_Return_Correct_Border_For_Specified_Kind(BorderKind kind, Type expected)
            {
                // Given, When
                var result = Border.GetBorder(kind);

                // Then
                result.ShouldBeOfType(expected);
            }

            [Fact]
            public void Should_Throw_If_Unknown_Border_Kind_Is_Specified()
            {
                // Given, When
                var result = Record.Exception(() => Border.GetBorder((BorderKind)int.MaxValue));

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Unknown border kind");
            }
        }
    }
}
