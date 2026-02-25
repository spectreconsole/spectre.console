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
            .Columns(new ProgressBarColumn())
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
            .Columns(new ProgressBarColumn())
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
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
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
            .Columns(new ProgressBarColumn())
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
        task.ShouldNotBeNull();
        task.MaxValue.ShouldBe(20);
        task.Value.ShouldBe(20);
    }

    [Fact]
    public void Setting_Max_Value_To_Zero_Should_Make_Percentage_OneHundred()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.MaxValue = 0;
        });

        // Then
        task.ShouldNotBeNull();
        task.Value.ShouldBe(0);
        task.Percentage.ShouldBe(100);
    }

    [Fact]
    public void Setting_Value_Should_Override_Incremented_Value()
    {
        // Given
        var console = new TestConsole()
            .Interactive();

        var task = default(ProgressTask);
        var progress = new Progress(console)
            .Columns(new ProgressBarColumn())
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
        task.ShouldNotBeNull();
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
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Value = task.MaxValue;
        });

        // Then
        task.ShouldNotBeNull();
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
            .Columns(new ProgressBarColumn())
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
        task.ShouldNotBeNull();
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
            .Columns(new ProgressBarColumn())
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
        var console = new TestConsole().Interactive();
        var task = default(ProgressTask);
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new RemainingTimeColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Increment(double.Epsilon);

            // Make sure that at least one millisecond has elapsed between the increments else the RemainingTime is null
            // when the last timestamp is equal to the first timestamp of the samples.
            time.Advance(TimeSpan.FromMilliseconds(1));

            task.Increment(double.Epsilon);
        });

        // Then
        task.ShouldNotBeNull();
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
                "[?25l", "foo1", "afterFoo1", "foo2", "beforeFoo3", "foo3",
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
                "[?25l", "foo1", "afterFoo1", "foo2", "beforeFoo3", "foo3",
                "[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[1A[2K[?25h",
            });
    }

    [Fact]
    public void Should_Store_And_Retrieve_Task_Tag()
    {
        // Given
        var console = new TestConsole().Interactive();
        var progress = new Progress(console)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        var task = default(ProgressTask);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Tag = "my custom tag";
        });

        // Then
        task.ShouldNotBeNull();
        task.Tag.ShouldBe("my custom tag");
    }

    [Fact]
    public void Should_Expose_Task_Tag_In_RenderHook()
    {
        // Given
        var console = new TestConsole().Interactive();
        var tag = new object();
        object? capturedTag = null;

        var progress = new Progress(console)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false)
            .UseRenderHook((renderable, tasks) =>
            {
                capturedTag = tasks.Single().Tag;
                return renderable;
            });

        // When
        progress.Start(ctx =>
        {
            var task = ctx.AddTask("foo");
            task.Tag = tag;
        });

        // Then
        capturedTag.ShouldBeSameAs(tag);
    }

    [Fact]
    public void Should_Remove_Task_From_Context()
    {
        // Given
        var console = new TestConsole().Interactive();
        var progress = new Progress(console)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When, Then
        progress.Start(ctx =>
        {
            var task = ctx.AddTask("foo");
            var removed = ctx.RemoveTask(task);
            removed.ShouldBeTrue();

            var removedAgain = ctx.RemoveTask(task);
            removedAgain.ShouldBeFalse();
        });
    }

    [Fact]
    public void Should_Not_Render_Removed_Task_From_Context()
    {
        // Given
        var console = new TestConsole()
            .Width(20)
            .Interactive()
            .EmitAnsiSequences();

        var progress = new Progress(console)
            .Columns(new TaskDescriptionColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            var removedTask = ctx.AddTask("removed");
            ctx.AddTask("kept");
            ctx.RemoveTask(removedTask).ShouldBeTrue();
        });

        // Then
        console.Output.ShouldContain("kept");
        console.Output.ShouldNotContain("removed");
    }

    [Fact]
    public void Should_Override_HideCompleted_On_Per_Task_Basis()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var progress = new Progress(console)
            .Columns(new TaskDescriptionColumn())
            .AutoRefresh(false)
            .AutoClear(false)
            .HideCompleted(true);

        // When
        progress.Start(ctx =>
        {
            var task1 = ctx.AddTask("foo");
            task1.HideWhenCompleted = false;
            task1.Value = task1.MaxValue;

            var task2 = ctx.AddTask("bar");
            task2.Value = task2.MaxValue;
        });

        // Then
        console.Output.ShouldContain("foo");
        console.Output.ShouldNotContain("bar");
    }

    [Fact]
    public void Should_Override_HideCompleted_False_On_Per_Task_Basis()
    {
        // Given
        var console = new TestConsole()
            .Width(10)
            .Interactive()
            .EmitAnsiSequences();

        var progress = new Progress(console)
            .Columns(new TaskDescriptionColumn())
            .AutoRefresh(false)
            .AutoClear(false)
            .HideCompleted(false);

        // When
        progress.Start(ctx =>
        {
            var task1 = ctx.AddTask("foo");
            task1.HideWhenCompleted = true;
            task1.Value = task1.MaxValue;

            var task2 = ctx.AddTask("bar");
            task2.Value = task2.MaxValue;
        });

        // Then
        console.Output.ShouldNotContain("foo");
        console.Output.ShouldContain("bar");
    }

    [Fact]
    public void Should_Respect_MaxSamplesKept()
    {
        // Given
        var console = new TestConsole().Interactive();
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        var task = default(ProgressTask);

        double? speed = 0;

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo", maxValue: 5000);
            task.MaxSamplesKept = 3;
            task.Increment(400);
            task.Increment(10);
            task.Increment(10);
            time.Advance(TimeSpan.FromMilliseconds(100));
            task.Increment(10);

            speed = task.Speed;
        });

        // Then
        task.ShouldNotBeNull();
        speed.ShouldNotBeNull();
        speed.Value.ShouldBe(300); // 30 over 100ms = 300 over 1 sec
    }

    [Fact]
    public void RemainingTimeColumn_Should_Return_Blank_For_Indeterminate_Task()
    {
        // Given
        var console = new TestConsole().Interactive().Width(10);
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new RemainingTimeColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        // When
        progress.Start(ctx =>
        {
            var task = ctx.AddTask("foo", autoStart: false);
            task.IsIndeterminate = true;
            task.StartTask();

            // Need to progress to make sure it tries to calculate remaining time
            task.Increment(10);
            time.Advance(TimeSpan.FromSeconds(10));
        });

        // Then
        console.Output.ShouldContain("**:**:**");
    }

    [Fact]
    public void Should_Drop_Samples_Older_Than_MaxSamplingAge()
    {
        // Given
        var console = new TestConsole().Interactive();
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        var task = default(ProgressTask);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.MaxSamplingAge = TimeSpan.FromMilliseconds(10);
            task.Increment(10);
            time.Advance(TimeSpan.FromMilliseconds(50));
            task.Increment(10);
        });

        // Then
        task.ShouldNotBeNull();
        task.Speed.ShouldBeNull();
    }

    [Fact]
    public void Should_Calculate_Speed_When_Task_Stopped()
    {
        // Given
        var console = new TestConsole().Interactive();
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        var task = default(ProgressTask);
        var speedBeforeStop = default(double?);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo");
            task.Increment(10);
            time.Advance(TimeSpan.FromMilliseconds(100));
            task.Increment(10);
            task.Increment(10);
            time.Advance(TimeSpan.FromMilliseconds(100));
            task.Increment(10);

            speedBeforeStop = task.Speed;
            task.StopTask();
        });

        // Then
        task.ShouldNotBeNull();
        speedBeforeStop.ShouldNotBeNull();
        speedBeforeStop.Value.ShouldBe(200); // at 40/200ms  that is 200/s or so
    }

    [Fact]
    public void Should_Include_StartTime_In_Speed_Calculation()
    {
        // Given
        var console = new TestConsole().Interactive();
        var time = new FakeTimeProvider(new DateTime(2024, 1, 1, 12, 0, 0));
        var progress = new Progress(console, time)
            .Columns(new ProgressBarColumn())
            .AutoRefresh(false)
            .AutoClear(false);

        var task = default(ProgressTask);

        // When
        progress.Start(ctx =>
        {
            task = ctx.AddTask("foo", autoStart: false);
            task.StartTask();
            time.Advance(TimeSpan.FromMilliseconds(50));
            task.Increment(10);
        });

        // Then
        task?.Speed
            .ShouldNotBeNull()
            .ShouldBeGreaterThan(0);
    }
}