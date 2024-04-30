namespace Spectre.Console.Analyzer.Tests.Unit.Analyzers;

public class NoCurrentLiveRenderablesTests
{
    private static readonly DiagnosticResult _expectedDiagnostics = new(
        Descriptors.S1020_AvoidConcurrentCallsToMultipleLiveRenderables.Id,
        DiagnosticSeverity.Warning);

    [Fact]
    public async void Status_call_within_live_call_warns()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    void Go()
    {
        AnsiConsole.Live(new Table()).Start(ctx =>
        {
            AnsiConsole.Status().Start(""go"", innerCtx => {});
        });
    }
}";

        await SpectreAnalyzerVerifier<NoConcurrentLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(10, 13));
    }

    [Fact]
    public async void Status_call_within_live_call_warns_with_instance()
    {
        const string Source = @"
using Spectre.Console;

class Child
{
    public readonly IAnsiConsole _console = AnsiConsole.Console;

    public void Go()
    {
        _console.Status().Start(""starting"", context =>
        {
            _console.Progress().Start(progressContext => { });
        });
    }
}";

        await SpectreAnalyzerVerifier<NoConcurrentLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(12, 13));
    }

    [Fact]
    public async void Calling_start_on_non_live_renderable_has_no_warning()
    {
        const string Source = @"
using Spectre.Console;

class Program
{
    static void Main()
    {
        Start();
    }

    static void Start() =>  AnsiConsole.WriteLine(""Starting..."");
}";

        await SpectreAnalyzerVerifier<NoConcurrentLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }
}
