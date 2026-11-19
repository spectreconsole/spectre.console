namespace Spectre.Console.Tests.Unit;

[ExpectationPath("AlternateScreenAsync")]
public sealed class AlternateScreenAsyncTests
{
    [Fact]
    public async Task Should_Throw_If_Alternative_Buffer_Is_Not_Supported_By_Terminal()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.AlternateBuffer = false;

        // When
        var result = await Record.ExceptionAsync(async () =>
        {
            console.WriteLine("Foo");
            await console.AlternateScreenAsync(async () =>
            {
                await Task.Yield(); // Simulate some work without wasting time.
                console.WriteLine("Bar");
            });
        });

        // Then
        result.ShouldNotBeNull();
        result.Message.ShouldBe("Alternate buffers are not supported by your terminal.");
    }

    [Fact]
    public async Task Should_Throw_If_Ansi_Is_Not_Supported_By_Terminal()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Ansi = false;
        console.Profile.Capabilities.AlternateBuffer = true;

        // When
        var result = await Record.ExceptionAsync(async () =>
        {
            console.WriteLine("Foo");
            await console.AlternateScreenAsync(async () =>
            {
                await Task.Yield(); // Simulate some work without wasting time.
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
        console.Profile.Capabilities.AlternateBuffer = true;
        console.EmitAnsiSequences = true;

        // When
        console.WriteLine("Foo");
        await console.AlternateScreenAsync(async () =>
        {
            await Task.Yield(); // Simulate some work without wasting time.
            console.WriteLine("Bar");
        });

        // Then
        await Verifier.Verify(console.Output);
    }
}