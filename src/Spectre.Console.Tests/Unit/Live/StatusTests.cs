namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Live/Status")]
public sealed class StatusTests
{
    public sealed class DummySpinner1 : Spinner
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
        public override bool IsUnicode => true;
        public override IReadOnlyList<string> Frames => new List<string> { "*", };
    }

    public sealed class DummySpinner2 : Spinner
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);
        public override bool IsUnicode => true;
        public override IReadOnlyList<string> Frames => new List<string> { "-", };
    }

    [Fact]
    [Expectation("Render")]
    public Task Should_Render_Status_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.TrueColor)
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var status = new Status(console)
        {
            AutoRefresh = false,
            Spinner = new DummySpinner1(),
        };

        // When
        status.Start("foo", ctx =>
        {
            ctx.Refresh();
            ctx.Spinner(new DummySpinner2());
            ctx.Status("bar");
            ctx.Refresh();
            ctx.Spinner(new DummySpinner1());
            ctx.Status("baz");
        });

        // Then
        return Verifier.Verify(console.Output);
    }

    [Fact]
    [Expectation("Render_LongText")]
    [GitHubIssue("https://github.com/spectreconsole/spectre.console/issues/2152")]
    public Task Should_Wrap_Status_Text_That_Exceeds_The_Console_Width()
    {
        // Given
        var console = new TestConsole()
            .Width(40)
            .Interactive();

        var status = new Status(console)
        {
            AutoRefresh = false,
            Spinner = new DummySpinner1(),
        };

        // When
        status.Start(
            "Building AppHost... playground\\HealthChecks\\HealthChecksSandbox.AppHost\\AppHost.csproj",
            ctx => ctx.Refresh());

        // Then
        return Verifier.Verify(console.Output);
    }
}