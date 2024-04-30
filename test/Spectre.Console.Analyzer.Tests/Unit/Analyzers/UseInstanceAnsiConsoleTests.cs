namespace Spectre.Console.Analyzer.Tests.Unit.Analyzers;

public class FavorInstanceAnsiConsoleOverStaticAnalyzerTests
{
    private static readonly DiagnosticResult _expectedDiagnostics = new(
        Descriptors.S1010_FavorInstanceAnsiConsoleOverStatic.Id,
        DiagnosticSeverity.Info);

    [Fact]
    public async void Should_only_warn_within_methods()
    {
        const string Source = @"
using Spectre.Console;

internal sealed class Foo
{
    private readonly IAnsiConsole _console;

    public Foo(IAnsiConsole console = null)
    {
        _console = console ?? AnsiConsole.Create(new AnsiConsoleSettings());
    }
}
";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }

    [Fact]
    public async void Instance_console_has_no_warnings()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    IAnsiConsole _ansiConsole = AnsiConsole.Console;

    void TestMethod()
    {
        _ansiConsole.Write(""this is fine"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }

    [Fact]
    public async void Static_console_with_no_instance_variables_has_no_warnings()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    void TestMethod()
    {
        AnsiConsole.Write(""this is fine"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }

    [Fact]
    public async void Console_Write_Has_Warning()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    IAnsiConsole _ansiConsole = AnsiConsole.Console;

    void TestMethod()
    {
        _ansiConsole.Write(""this is fine"");
        AnsiConsole.Write(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(11, 9));
    }
}
