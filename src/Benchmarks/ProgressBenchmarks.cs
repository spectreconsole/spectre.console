using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spectre.Console;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class ProgressBenchmarks
{
    private const int FramesPerInvoke = 100;

    private IAnsiConsole _console = null!;

    [GlobalSetup]
    public void Setup()
    {
        _console =
            AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Yes,
                ColorSystem = ColorSystemSupport.TrueColor,
                Out = new AnsiConsoleOutput(TextWriter.Null),
                Interactive = InteractionSupport.Yes,
                EnvironmentVariables = null,
            });
    }

    [Benchmark(OperationsPerInvoke = FramesPerInvoke)]
    public void RefreshFrame_DefaultColumns()
    {
        RunFrames(new Progress(_console)
        {
            AutoRefresh = false,
            AutoClear = true,
        });
    }

    [Benchmark(OperationsPerInvoke = FramesPerInvoke)]
    public void RefreshFrame_DownloadColumns()
    {
        var progress = new Progress(_console)
        {
            AutoRefresh = false,
            AutoClear = true,
        };

        progress.Columns(
            new SpinnerColumn(),
            new TaskDescriptionColumn(),
            new ProgressBarColumn(),
            new PercentageColumn(),
            new DownloadedColumn(),
            new TransferSpeedColumn(),
            new RemainingTimeColumn());

        RunFrames(progress);
    }

    private static void RunFrames(Progress progress)
    {
        progress.Start(ctx =>
        {
            var first = ctx.AddTask("[green]left-pad[/] 1.3.0");
            var second = ctx.AddTask("[green]is-odd[/] 3.0.1");
            var third = ctx.AddTask("[yellow]react-dom[/] 19.2.0");
            var fourth = ctx.AddTask("[yellow]typescript[/] 5.9.3");

            var increment = 100.0 / FramesPerInvoke;
            for (var i = 0; i < FramesPerInvoke; i++)
            {
                first.Increment(increment);
                second.Increment(increment);
                third.Increment(increment);
                fourth.Increment(increment);
                ctx.Refresh();
            }
        });
    }
}
