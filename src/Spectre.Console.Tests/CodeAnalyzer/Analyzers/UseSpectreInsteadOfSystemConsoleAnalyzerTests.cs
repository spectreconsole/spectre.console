using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Spectre.Console.Analyzer;
using Xunit;
using AnalyzerVerify =
    Spectre.Console.Tests.CodeAnalyzers.SpectreAnalyzerVerifier<
        Spectre.Console.Analyzer.UseSpectreInsteadOfSystemConsoleAnalyzer>;

namespace Spectre.Console.Tests.CodeAnalyzers.Analyzers
{
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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source)
                .ConfigureAwait(false);
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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(7, 9))
                .ConfigureAwait(false);
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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(7, 9))
                .ConfigureAwait(false);
        }
    }
}