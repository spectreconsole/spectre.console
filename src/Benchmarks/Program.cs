using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Benchmarks;

public static class Program
{
    public static int Main(string[] args)
    {
        BenchmarkRunner.Run(typeof(Program).Assembly);
        return 0;
    }
}

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class RenderBenchmarks
{
    private IAnsiConsole _console = null!;
    private IRenderable _renderable = null!;

    [GlobalSetup]
    public void Setup()
    {
        _console =
            AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.TrueColor,
                Out = new AnsiConsoleOutput(new StringWriter()),
                Interactive = InteractionSupport.No,
                EnvironmentVariables = null
            });

        _renderable = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow(new Text("1"), new Table()
                .AddColumns("Baz", "Qux")
                .AddRow("1", "2")
                .AddRow("3", "4"));
    }

    [Benchmark]
    public void Render() => _console.Write(_renderable);
}

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class AnsiBenchmarks
{
    private IAnsiConsole _console = null!;
    private IRenderable _renderable = null!;

    [GlobalSetup]
    public void Setup()
    {
        _console =
            AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.TrueColor,
                Out = new AnsiConsoleOutput(new StringWriter()),
                Interactive = InteractionSupport.No,
                EnvironmentVariables = null
            });

        _renderable = new Table()
            .AddColumns("Foo", "Bar")
            .AddRow(new Text("1"), new Table()
                .AddColumns("Baz", "Qux")
                .AddRow("1", "2")
                .AddRow("3", "4"));
    }

    [Benchmark]
    public void RenderableToAnsi() => _console.ToAnsi(_renderable);
}