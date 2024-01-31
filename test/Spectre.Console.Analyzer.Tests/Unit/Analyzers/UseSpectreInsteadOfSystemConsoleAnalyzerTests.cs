namespace Spectre.Console.Analyzer.Tests.Unit.Analyzers;

public class UseSpectreInsteadOfSystemConsoleAnalyzerTests
{
    private static readonly DiagnosticResult _expectedDiagnostics = new(
        Descriptors.S1000_UseAnsiConsoleOverSystemConsole.Id,
        DiagnosticSeverity.Warning);

    [Fact]
    public async void Non_configured_SystemConsole_methods_report_no_warnings()
    {
        const string Source = @"
using System;

class TestClass {
    void TestMethod()
    {
        var s = Console.ReadLine();
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyAnalyzerAsync(Source);
    }

    [Fact]
    public async void Console_Write_Has_Warning()
    {
        const string Source = @"
using System;

class TestClass {
    void TestMethod()
    {
        Console.Write(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(7, 9));
    }

    [Fact]
    public async void Console_WriteLine_Has_Warning()
    {
        const string Source = @"
using System;

class TestClass
{
    void TestMethod() {
        Console.WriteLine(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(7, 9));
    }
}
