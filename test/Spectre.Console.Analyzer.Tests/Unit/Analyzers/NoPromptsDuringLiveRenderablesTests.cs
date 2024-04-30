namespace Spectre.Console.Analyzer.Tests.Unit.Analyzers;

public class NoPromptsDuringLiveRenderablesTests
{
    private static readonly DiagnosticResult _expectedDiagnostics = new(
        Descriptors.S1021_AvoidPromptCallsDuringLiveRenderables.Id,
        DiagnosticSeverity.Warning);

    [Fact]
    public async Task Prompt_out_of_progress_does_not_warn()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    void Go()
    {
        var s = AnsiConsole.Ask<string>(""How are you?"");
    }
}";

        await SpectreAnalyzerVerifier<NoPromptsDuringLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }

    [Fact]
    public async Task Instance_variables_warn()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    public IAnsiConsole _console = AnsiConsole.Console;

    public void Go()
    {
        _console.Status().Start(""starting"", context =>
        {
            var result = _console.Confirm(""we ok?"");
        });
    }
}";

        await SpectreAnalyzerVerifier<NoPromptsDuringLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(12, 26));
    }

    [Fact]
    public async Task Prompt_in_progress_warns()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    void Go()
    {
        AnsiConsole.Progress().Start(_ =>
        {
            AnsiConsole.Ask<string>(""How are you?"");
        });
    }
}";

        await SpectreAnalyzerVerifier<NoPromptsDuringLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(10, 13));
    }

    [Fact]
    public async Task Can_call_other_methods_from_within_renderables()
    {
        const string Source = @"
using Spectre.Console;

class Program
{
    static void Main()
    {
        AnsiConsole.Status().Start(""here we go"", context =>
        {
            var result = Confirm();

        });
    }

    static string Confirm() => string.Empty;
}";

        await SpectreAnalyzerVerifier<NoPromptsDuringLiveRenderablesAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }
}
