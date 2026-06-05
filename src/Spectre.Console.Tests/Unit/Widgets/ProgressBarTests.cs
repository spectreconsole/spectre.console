namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Widgets/ProgressBar")]
public class ProgressBarTests
{
    [Fact]
    [Expectation("Render")]
    public async Task Should_Render_Correctly()
    {
        // Given
        var console = new TestConsole();

        var bar = new ProgressBar()
        {
            Width = 60,
            Value = 9000,
            MaxValue = 9000,
            ShowValue = true,
        };

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
            ValueFormatter = (value, _) => value.ToString("N0", CultureInfo.InvariantCulture),
        };

        // When
        console.Write(bar);

        // Then
        await Verifier.Verify(console.Output);
    }

    [Fact]
    public void Should_Render_Indeterminate_When_Value_Reaches_MaxValue()
    {
        // Given
        var console = new TestConsole()
            .SupportsUnicode(false)
            .Colors(ColorSystem.NoColors);

        var bar = new ProgressBar()
        {
            Width = 20,
            Value = 100,
            MaxValue = 100,
            IsIndeterminate = true,
        };

        // When
        console.Write(bar);

        // Then
        console.Output.ShouldContain(" ");
        console.Output.ShouldNotBe(new string('-', 20));
    }
}
