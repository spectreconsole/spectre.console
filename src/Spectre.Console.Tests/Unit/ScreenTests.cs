using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Screen")]
    public sealed class ScreenTests
    {
        [Fact]
        public void Should_Throw_If_Alternative_Buffer_Is_Not_Supported_By_Terminal()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.EightBit, AnsiSupport.Yes);
            console.Profile.Capabilities.AlternateBuffer = false;

            // When
            var result = Record.Exception(() =>
            {
                console.WriteLine("Foo");
                console.AlternateScreen(() =>
                {
                    console.WriteLine("Bar");
                });
            });

            // Then
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Alternate buffers are not supported by your terminal.");
        }

        [Fact]
        public void Should_Throw_If_Ansi_Is_Not_Supported_By_Terminal()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.EightBit, AnsiSupport.No);
            console.Profile.Capabilities.AlternateBuffer = true;

            // When
            var result = Record.Exception(() =>
            {
                console.WriteLine("Foo");
                console.AlternateScreen(() =>
                {
                    console.WriteLine("Bar");
                });
            });

            // Then
            result.ShouldNotBeNull();
            result.Message.ShouldBe("Alternate buffers are not supported since your terminal does not support ANSI.");
        }

        [Fact]
        [Expectation("NoBorder")]
        public async Task Should_Write_To_Alternate_Screen()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.EightBit, AnsiSupport.Yes);
            console.Profile.Capabilities.AlternateBuffer = true;

            // When
            console.WriteLine("Foo");
            console.AlternateScreen(() =>
            {
                console.WriteLine("Bar");
            });

            // Then
            await Verifier.Verify(console.Output);
        }
    }
}
