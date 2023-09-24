namespace Spectre.Console.Tests.Unit;

[UsesVerify]
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
    [Expectation("WriteLineOverflow")]
    public Task Should_Render_Correctly_When_WriteLine_Exceeds_Console_Width()
    {
        // Given
        var console = new TestConsole()
            .Colors(ColorSystem.TrueColor)
            .Width(100)
            .Interactive()
            .EmitAnsiSequences();
        var status = new Status(console)
        {
            AutoRefresh = false,
        };

        // When
        status.Start("long text that should not interfere writeline text", ctx =>
        {
            ctx.Refresh();
            console.WriteLine("x".Repeat(console.Profile.Width + 10), new Style(foreground: Color.White));
        });

        // Then
        return Verifier.Verify(console.Output);
    }
}
