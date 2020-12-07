using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    public sealed partial class StatusTests
    {
        [Fact]
        public void Should_Render_Status_Correctly()
        {
            // Given
            var console = new TestableAnsiConsole(ColorSystem.TrueColor, width: 10);

            var status = new Status(console);
            status.AutoRefresh = false;
            status.Spinner = new DummySpinner1();

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
            console.Output
                .NormalizeLineEndings()
                .ShouldBe(
                    "[?25l     \n" +
                    "[38;5;11m*[0m foo\n" +
                    "     [1A[1A     \n" +
                    "[38;5;11m-[0m bar\n" +
                    "     [1A[1A     \n" +
                    "[38;5;11m*[0m baz\n" +
                    "     [2K[1A[2K[1A[2K[?25h");
        }
    }
}
