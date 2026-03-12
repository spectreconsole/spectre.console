namespace Spectre.Console.Tests.Unit;

public sealed class InteractionDetectorTests
{
    [Fact]
    public void Should_Not_Be_Interactive_When_Output_Is_Redirected()
    {
        var result = InteractionDetector.IsInteractive(
            InteractionSupport.Detect,
            isInputRedirected: false,
            isOutputRedirected: true,
            isErrorRedirected: false);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Not_Be_Interactive_When_Error_Is_Redirected()
    {
        var result = InteractionDetector.IsInteractive(
            InteractionSupport.Detect,
            isInputRedirected: false,
            isOutputRedirected: false,
            isErrorRedirected: true);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Should_Be_Interactive_When_Nothing_Is_Redirected()
    {
        var result = InteractionDetector.IsInteractive(
            InteractionSupport.Detect,
            isInputRedirected: false,
            isOutputRedirected: false,
            isErrorRedirected: false);

        result.ShouldBeTrue();
    }
}