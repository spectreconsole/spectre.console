namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("AlternateScreen")]
public sealed class AlternateScreenTests
{
    [Fact]
    public void Should_Throw_If_Alternative_Buffer_Is_Not_Supported_By_Terminal()
    {
        // Given
        var console = new TestConsole();
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
        var console = new TestConsole();
        console.Profile.Capabilities.Ansi = false;
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
    [Expectation("Show")]
    public async Task Should_Write_To_Alternate_Screen()
    {
        // Given
        var console = new TestConsole();
        console.EmitAnsiSequences = true;
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
