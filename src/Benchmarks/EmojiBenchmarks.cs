using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spectre.Console;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class EmojiBenchmarks
{
    [Benchmark]
    public void Replace_NoEmoji()
    {
        Emoji.Replace("Hello Spectre.Console!");
    }

    [Benchmark]
    public void Replace_NoEmoji_ButColon()
    {
        Emoji.Replace("https://spectreconsole.net");
    }

    [Benchmark]
    public void Replace_Emoji()
    {
        Emoji.Replace("Hello :ghost:.Console!");
    }
}