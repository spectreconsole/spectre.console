using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Spectre.Console.Analyzer;
using Xunit;
using AnalyzerVerify =
    Spectre.Console.Tests.CodeAnalyzers.SpectreAnalyzerVerifier<
        Spectre.Console.Analyzer.UseSpectreInsteadOfSystemConsoleAnalyzer>;

namespace Spectre.Console.Tests.CodeAnalyzers.Fixes
{
    public class UseSpectreInsteadOfSystemConsoleFixTests
    {
        private static readonly DiagnosticResult _expectedDiagnostic = new(
            Descriptors.S1000_UseAnsiConsoleOverSystemConsole.Id,
            DiagnosticSeverity.Warning);

        [Fact]
        public async Task SystemConsole_replaced_with_AnsiConsole()
        {
            const string Source = @"
using System;

class TestClass 
{
    void TestMethod() 
    {
        Console.WriteLine(""Hello, World"");
    }
}";

            const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass 
{
    void TestMethod() 
    {
        AnsiConsole.WriteLine(""Hello, World"");
    }
}";

            await AnalyzerVerify
                .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(8, 9), FixedSource)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task SystemConsole_replaced_with_imported_AnsiConsole()
        {
            const string Source = @"
using System;

class TestClass 
{
    void TestMethod() 
    {
        Console.WriteLine(""Hello, World"");
    }
}";

            const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass 
{
    void TestMethod() 
    {
        AnsiConsole.WriteLine(""Hello, World"");
    }
}";

            await AnalyzerVerify
                .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(8, 9), FixedSource)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task SystemConsole_replaced_with_field_AnsiConsole()
        {
            const string Source = @"
using System;
using Spectre.Console;

class TestClass 
{
    IAnsiConsole _ansiConsole;

    void TestMethod() 
    {
        Console.WriteLine(""Hello, World"");
    }
}";

            const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass 
{
    IAnsiConsole _ansiConsole;

    void TestMethod() 
    {
        _ansiConsole.WriteLine(""Hello, World"");
    }
}";

            await AnalyzerVerify
                .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task SystemConsole_replaced_with_static_field_AnsiConsole()
        {
            const string Source = @"
using System;
using Spectre.Console;

class TestClass 
{
    static IAnsiConsole _ansiConsole;

    static void TestMethod() 
    {
        Console.WriteLine(""Hello, World"");
    }
}";

            const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass 
{
    static IAnsiConsole _ansiConsole;

    static void TestMethod() 
    {
        _ansiConsole.WriteLine(""Hello, World"");
    }
}";

            await AnalyzerVerify
                .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource)
                .ConfigureAwait(false);
        }
    }
}