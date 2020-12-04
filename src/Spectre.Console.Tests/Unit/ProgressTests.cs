using System.Threading.Tasks;
using Shouldly;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    public sealed class ProgressTests
    {
        [Fact]
        public void Should_Render_Task_Correctly()
        {
            // Given
            var console = new TestableAnsiConsole(ColorSystem.TrueColor, width: 10);

            var progress = new Progress(console)
                .Columns(new[] { new ProgressBarColumn() })
                .AutoRefresh(false)
                .AutoClear(true);

            // When
            progress.Start(ctx => ctx.AddTask("foo"));

            // Then
            console.Output
                .NormalizeLineEndings()
                .ShouldBe(
                    "[?25l" + // Hide cursor
                    "          \n" + // Top padding
                    "[38;5;8m━━━━━━━━━━[0m\n" + // Task
                    "          " + // Bottom padding
                    "[2K[1A[2K[1A[2K[?25h"); // Clear + show cursor
        }

        [Fact]
        public void Should_Not_Auto_Clear_If_Specified()
        {
            // Given
            var console = new TestableAnsiConsole(ColorSystem.TrueColor, width: 10);

            var progress = new Progress(console)
                .Columns(new[] { new ProgressBarColumn() })
                .AutoRefresh(false)
                .AutoClear(false);

            // When
            progress.Start(ctx => ctx.AddTask("foo"));

            // Then
            console.Output
                .NormalizeLineEndings()
                .ShouldBe(
                    "[?25l" + // Hide cursor
                    "          \n" + // Top padding
                    "[38;5;8m━━━━━━━━━━[0m\n" + // Task
                    "          \n" + // Bottom padding
                    "[?25h"); // show cursor
        }

        [Fact]
        public Task Foo()
        {
            // Given
            var console = new PlainConsole(width: 20);

            var progress = new Progress(console)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),
                    new ProgressBarColumn(),
                    new PercentageColumn(),
                    new RemainingTimeColumn(),
                    new SpinnerColumn(),
                })
                .AutoRefresh(false)
                .AutoClear(false);

            // When
            progress.Start(ctx =>
            {
                ctx.AddTask("foo");
                ctx.AddTask("bar");
                ctx.AddTask("baz");
            });

            // Then
            return Verifier.Verify(console.Output);
        }
    }
}
