namespace Spectre.Console.Tests.Unit;

public sealed class AnsiDetectorTests
{
    [Fact]
    public void Should_Not_Support_Ansi_When_Output_Is_Redirected()
    {
        var result = AnsiDetector.Detect(
            global::System.Console.Out,
            AnsiSupport.Detect,
            isOutputRedirected: true,
            isErrorRedirected: false);

        result.Ansi.ShouldBeFalse();
        result.Legacy.ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Support_Ansi_When_Error_Is_Redirected()
    {
        var result = AnsiDetector.Detect(
            global::System.Console.Error,
            AnsiSupport.Detect,
            isOutputRedirected: false,
            isErrorRedirected: true);

        result.Ansi.ShouldBeFalse();
        result.Legacy.ShouldBeFalse();
    }

    [Fact]
    public void Should_Support_Ansi_When_Explicitly_Enabled_Even_If_Output_Is_Redirected()
    {
        var result = AnsiDetector.Detect(
            global::System.Console.Out,
            AnsiSupport.Yes,
            isOutputRedirected: true,
            isErrorRedirected: false);

        result.Ansi.ShouldBeTrue();
    }
}