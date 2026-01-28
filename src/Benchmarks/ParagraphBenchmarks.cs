using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Spectre.Console;

namespace Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class ParagraphBenchmarks
{
    private const string TextOneLine = "This string is all on one line.";
    private const string TextMultiLine = "This string\nhas multiple\nlines.";

    public static IEnumerable<object[]> Arguments => [
        [TextOneLine],
        [TextMultiLine],
    ];

    [Benchmark]
    [ArgumentsSource(nameof(Arguments))]
    public void Append(string text)
    {
        new Paragraph().Append(text);
    }
}