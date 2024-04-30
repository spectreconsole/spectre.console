namespace Spectre.Console.Analyzer.Tests.Unit.Fixes;

public class UseInstanceOfStaticAnsiConsoleTests
{
    private static readonly DiagnosticResult _expectedDiagnostic = new(
        Descriptors.S1010_FavorInstanceAnsiConsoleOverStatic.Id,
        DiagnosticSeverity.Info);

    [Fact]
    public async Task Static_call_replaced_with_field_call()
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

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource);
    }

    [Fact]
    public async Task Static_call_replaced_with_field_call_Should_Preserve_Trivia()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    IAnsiConsole _ansiConsole = AnsiConsole.Console;

    void TestMethod()
    {
        var foo = 1;

        AnsiConsole.Write(""this is fine"");
        _ansiConsole.Write(""Hello, World"");
    }
}";

        const string FixedSource = @"
using Spectre.Console;

class TestClass
{
    IAnsiConsole _ansiConsole = AnsiConsole.Console;

    void TestMethod()
    {
        var foo = 1;

        _ansiConsole.Write(""this is fine"");
        _ansiConsole.Write(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(12, 9), FixedSource);
    }

    [Fact]
    public async Task Static_call_replaced_with_parameter_call()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    void TestMethod(IAnsiConsole ansiConsole)
    {
        AnsiConsole.Write(""Hello, World"");
    }
}";

        const string FixedSource = @"
using Spectre.Console;

class TestClass
{
    void TestMethod(IAnsiConsole ansiConsole)
    {
        ansiConsole.Write(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(8, 9), FixedSource);
    }

    [Fact]
    public async Task Static_call_replaced_with_static_field_if_valid()
    {
        const string Source = @"
using Spectre.Console;

class TestClass
{
    static IAnsiConsole staticConsole;
    IAnsiConsole instanceConsole;

    static void TestMethod()
    {
        AnsiConsole.Write(""Hello, World"");
    }
}";

        const string FixedSource = @"
using Spectre.Console;

class TestClass
{
    static IAnsiConsole staticConsole;
    IAnsiConsole instanceConsole;

    static void TestMethod()
    {
        staticConsole.Write(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<FavorInstanceAnsiConsoleOverStaticAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource);
    }
}
