using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spectre.Console;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class TableBenchmarks
{
    private IAnsiConsole _console = null!;
    private Table _table = null!;

    [Params(100, 1000)]
    public int Rows { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _console =
            AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Yes,
                ColorSystem = ColorSystemSupport.TrueColor,
                Out = new AnsiConsoleOutput(TextWriter.Null),
                Interactive = InteractionSupport.No,
                EnvironmentVariables = null,
            });

        _table = new Table()
            .AddColumn("Package")
            .AddColumn("Version")
            .AddColumn("Description");

        for (var i = 0; i < Rows; i++)
        {
            _table.AddRow(
                $"[blue]package-{i}[/]",
                $"{i % 10}.{i % 7}.{i % 13}",
                (i % 3) switch
                {
                    0 => "A useful library for building console applications",
                    1 => "Une bibliothèque géniale à découvrir",
                    _ => "控制台应用程序的实用工具库",
                });
        }
    }

    [Benchmark]
    public void Render() => _console.Write(_table);
}
