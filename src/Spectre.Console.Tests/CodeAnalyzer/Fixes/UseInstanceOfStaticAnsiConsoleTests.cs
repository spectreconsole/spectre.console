using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Spectre.Console.Analyzer;
using Xunit;
using AnalyzerVerify =
    Spectre.Console.Tests.CodeAnalyzers.SpectreAnalyzerVerifier<
        Spectre.Console.Analyzer.FavorInstanceAnsiConsoleOverStaticAnalyzer>;

namespace Spectre.Console.Tests.CodeAnalyzers.Fixes
{
    public class UseInstanceOfStaticAnsiConsoleTests
    {
        private static readonly DiagnosticResult _expectedDiagnostic = new(
            Descriptors.S1010_FavorInstanceAnsiConsoleOverStatic.Id,
            DiagnosticSeverity.Info);

        [Fact]
        public async Task SystemConsole_replaced_with_AnsiConsole()
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

            const string FixedSource = @"
using Spectre.Console;

class TestClass 
{
    IAnsiConsole _ansiConsole = AnsiConsole.Console;    

    void TestMethod() 
    {
        _ansiConsole.Write(""this is fine"");
        _ansiConsole.Write(""Hello, World"");
    } 
}";

            await AnalyzerVerify
                .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource)
                .ConfigureAwait(false);
        }
    }
}