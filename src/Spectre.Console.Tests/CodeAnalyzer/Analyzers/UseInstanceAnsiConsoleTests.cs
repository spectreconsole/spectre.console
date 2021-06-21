using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Spectre.Console.Analyzer;
using Xunit;
using AnalyzerVerify =
    Spectre.Console.Tests.CodeAnalyzers.SpectreAnalyzerVerifier<
        Spectre.Console.Analyzer.FavorInstanceAnsiConsoleOverStaticAnalyzer>;

namespace Spectre.Console.Tests.CodeAnalyzers.Analyzers
{
    public class FavorInstanceAnsiConsoleOverStaticAnalyzerTests
    {
        private static readonly DiagnosticResult _expectedDiagnostics = new(
            Descriptors.S1010_FavorInstanceAnsiConsoleOverStatic.Id,
            DiagnosticSeverity.Info);

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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source)
                .ConfigureAwait(false);
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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source)
                .ConfigureAwait(false);
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

            await AnalyzerVerify
                .VerifyAnalyzerAsync(Source, _expectedDiagnostics.WithLocation(11, 9))
                .ConfigureAwait(false);
        }
    }
}