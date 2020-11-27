using Shouldly;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
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
    }
}
