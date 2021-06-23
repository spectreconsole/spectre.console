using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed class ColorSystemTests
    {
        [Theory]
        [InlineData(ColorSystem.NoColors, ColorSystemSupport.NoColors)]
        [InlineData(ColorSystem.Legacy, ColorSystemSupport.Legacy)]
        [InlineData(ColorSystem.Standard, ColorSystemSupport.Standard)]
        [InlineData(ColorSystem.EightBit, ColorSystemSupport.EightBit)]
        [InlineData(ColorSystem.TrueColor, ColorSystemSupport.TrueColor)]
        public void Should_Be_Analog_To_ColorSystemSupport(ColorSystem colors, ColorSystemSupport support)
        {
            // Given, When
            var result = (int)colors;

            // Then
            result.ShouldBe((int)support);
        }
    }
}
