namespace Spectre.Console.Tests.Unit;

[UsesVerify]
[ExpectationPath("Widgets/ProgressBar")]
public class ProgressBarTests
{
    [Fact]
    [Expectation("Render")]
    public async Task Should_Render_Correctly()
    {
        // Given
        var console = new TestConsole();

        var bar = new ProgressBar() { Width = 60, Value = 9000, MaxValue = 9000, ShowValue = true };

        // When
        console.Write(bar);
        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Formatted")]
    public async Task Should_Render_ValueFormatted()
    {
        // Given
        var console = new TestConsole();

        var bar = new ProgressBar()
        {
            Width = 60,
            Value = 9000,
            MaxValue = 9000,
            ShowValue = true,
            ValueFormatter = (value, _) => value.ToString("N0"),
        };

        // When
        console.Write(bar);
        // Then
        await Verifier.Verify(console.Output);
    }
}