namespace Spectre.Console.Tests.Unit;

[ExpectationPath("Live/Progress")]
public sealed class ProgressTests
{
    [Fact]
    public void Should_Render_Task_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

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
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

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
        var console = new TestConsole()
            .Width(20)
            .Interactive();

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
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
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

    [Fact]
    public void Setting_Value_Should_Override_Incremented_Value()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new[] { new ProgressBarColumn() })
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Increment(50);
            task.Value = 20;
        });

        // Then
        task.MaxValue.ShouldBe(100);
        task.Value.ShouldBe(20);
    }

    [Fact]
    public void Setting_Value_To_MaxValue_Should_Finish_Task()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new[] { new ProgressBarColumn() })
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Value = task.MaxValue;
        });

        // Then
        task.IsFinished.ShouldBe(true);
    }

    [Fact]
    public void Should_Increment_Manually_Set_Value()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new[] { new ProgressBarColumn() })
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Value = 50;
            task.Increment(10);
        });

        // Then
        task.Value.ShouldBe(60);
    }

    [Fact]
    public void Should_Hide_Completed_Tasks()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var taskFinished = default(ProgressTask);
        var taskInProgress1 = default(ProgressTask);
        var taskInProgress2 = default(ProgressTask);

        var progress = new Progress(console)
            .Columns(new[] { new ProgressBarColumn() })
            .AutoRefresh(false)
            .AutoClear(false)
            .HideCompleted(true);

        // When
        progress.Start(ctx =>
        {
            taskInProgress1 = ctx.AddTask("foo");
            taskFinished = ctx.AddTask("bar");
            taskInProgress2 = ctx.AddTask("baz");
            taskInProgress2.Increment(20);
            taskFinished.Value = taskFinished.MaxValue;
        });

        // Then
        console.Output
            .NormalizeLineEndings()
            .ShouldBe(
                "[?25l" + // Hide cursor
                "          \n" + // top padding
                "[38;5;8m━━━━━━━━━━[0m\n" + // taskInProgress1
                "[38;5;11m━━[0m[38;5;8m━━━━━━━━[0m\n" + // taskInProgress2
                "          \n" + // bottom padding
                "[?25h"); // show cursor
    }

    [Fact]
    public void Should_Report_Max_Remaining_Time_For_Extremely_Small_Progress()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new[] { new RemainingTimeColumn() })
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Increment(double.Epsilon);

            // Make sure that at least one millisecond has elapsed between the increments else the RemainingTime is null
            // when the last timestamp is equal to the first timestamp of the samples.
            Thread.Sleep(1);

            task.Increment(double.Epsilon);
        });

        // Then
        task.RemainingTime.ShouldBe(TimeSpan.MaxValue);
    }

    [Fact]
    public void Should_Render_Tasks_Added_Before_And_After_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var progress = new Progress(console)
            .Columns(new TaskDescriptionColumn())
            .AutoRefresh(false)
            .AutoClear(true);

        // When
        progress.Start(ctx =>
        {
            var foo1 = ctx.AddTask("foo1");
            var foo2 = ctx.AddTask("foo2");
            var foo3 = ctx.AddTask("foo3");

            var afterFoo1 = ctx.AddTaskAfter("afterFoo1", foo1);
            var beforeFoo3 = ctx.AddTaskBefore("beforeFoo3", foo3);
        });

        // Then
        console.Output.SplitLines().Select(x => x.Trim()).ToArray()
            .ShouldBeEquivalentTo(new[]
                {
                    "[?25l",
                    "foo1",
                    "afterFoo1",
                    "foo2",
                    "beforeFoo3",
                    "foo3",
                    "[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[?25h",
                });
    }

    [Fact]
    public void Should_Render_Tasks_At_Specified_Indexes_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var progress = new Progress(console)
            .Columns(new TaskDescriptionColumn())
            .AutoRefresh(false)
            .AutoClear(true);

        // When
        progress.Start(ctx =>
        {
            var foo1 = ctx.AddTask("foo1");
            var foo2 = ctx.AddTask("foo2");
            var foo3 = ctx.AddTask("foo3");

            var afterFoo1 = ctx.AddTaskAt("afterFoo1", 1);
            var beforeFoo3 = ctx.AddTaskAt("beforeFoo3", 3);
        });

        // Then
        console.Output.SplitLines().Select(x => x.Trim()).ToArray()
            .ShouldBeEquivalentTo(new[]
            {
                "[?25l",
                "foo1",
                "afterFoo1",
                "foo2",
                "beforeFoo3",
                "foo3",
                "[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[?25h",
            });
    }
}
