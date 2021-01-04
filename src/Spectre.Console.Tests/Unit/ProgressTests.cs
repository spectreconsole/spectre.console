using System.Threading.Tasks;
using Shouldly;
using Spectre.Console.Testing;
using Spectre.Verify.Extensions;
using VerifyXunit;
using Xunit;

namespace Spectre.Console.Tests.Unit
{
    [UsesVerify]
    [ExpectationPath("Widgets/Progress")]
    public sealed class ProgressTests
    {
        [Fact]
        public void Should_Render_Task_Correctly()
        {
            // Given
            var console = new FakeAnsiConsole(ColorSystem.TrueColor, width: 10);

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
            var console = new FakeAnsiConsole(ColorSystem.TrueColor, width: 10);

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
        [Expectation("Render_ReduceWidth")]
        public Task Should_Reduce_Width_If_Needed()
        {
            // Given
            var console = new FakeConsole(width: 20);

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

        [Fact]
        public void Setting_Max_Value_Should_Set_The_MaxValue_And_Cap_Value()
        {
            // Given
            var task = default(ProgressTask);
            var console = new FakeConsole();
            var progress = new Progress(console)
                .Columns(new[] { new ProgressBarColumn() })
                .AutoRefresh(false)
                .AutoClear(false);

            // When
            progress.Start(ctx =>
            {
                task = ctx.AddTask("foo");
                task.Increment(100);
                task.MaxValue = 20;
            });

            // Then
            task.MaxValue.ShouldBe(20);
            task.Value.ShouldBe(20);
        }
    }
}
