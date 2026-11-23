namespace Spectre.Console.Tests.Unit;

public sealed class SpinnerExtensionsTests
{
    private sealed class WideSpinner : Spinner
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(10);
        public override bool IsUnicode => true;

        // "＊＊" is 2 chars but 4 cells wide, "x" is 1 cell wide
        public override IReadOnlyList<string> Frames => new List<string> { "＊＊", "x" };
    }

    [Fact]
    [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2261")]
    public async Task Should_Move_Cursor_By_Widest_Frame_Cell_Width()
    {
        // Given
        var console = new TestConsole()
            .Interactive()
            .EmitAnsiSequences();

        // When
        await Task.Delay(50).Spinner(new WideSpinner(), ansiConsole: console);

        // Then
        Regex.Matches(console.Output, @"\e\[(\d+)D")
            .Select(match => match.Groups[1].Value)
            .ShouldAllBe(columns => columns == "4");

        console.Output.ShouldEndWith("    \e[4D\e[?25h");
    }
}
