using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spectre.Console;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class MarkupBenchmarks
{
    private const string Input = "[yellow]Hello [italic][link=https://spectreconsole.net]Spectre.Console Docs[/][/]![/]";

    [Benchmark]
    public void Markup_Constructor()
    {
        _ = new Markup(Input);
    }

    [Benchmark]
    public void AnsiMarkup_Parse()
    {
        AnsiMarkup.Parse(Input);
    }
}