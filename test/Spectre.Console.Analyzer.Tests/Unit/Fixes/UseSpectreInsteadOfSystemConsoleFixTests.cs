namespace Spectre.Console.Analyzer.Tests.Unit.Fixes;

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

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(8, 9), FixedSource);
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

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(8, 9), FixedSource);
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

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_replaced_with_local_variable_AnsiConsole()
    {
        const string Source = @"
using System;
using Spectre.Console;

class TestClass
{
    void TestMethod()
    {
        IAnsiConsole ansiConsole = null;
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
        IAnsiConsole ansiConsole = null;
        ansiConsole.WriteLine(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(10, 9), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_not_replaced_with_local_variable_declared_after_the_call()
    {
        const string Source = @"
using System;
using Spectre.Console;

class TestClass
{
    void TestMethod()
    {
        Console.WriteLine(""Hello, World"");
        IAnsiConsole ansiConsole;
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
        IAnsiConsole ansiConsole;
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(9, 9), FixedSource);
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

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_replaced_with_AnsiConsole_when_field_is_not_static()
    {
        const string Source = @"
using System;
using Spectre.Console;

class TestClass
{
    IAnsiConsole _ansiConsole;

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
    IAnsiConsole _ansiConsole;

    static void TestMethod()
    {
        AnsiConsole.WriteLine(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(11, 9), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_replaced_with_AnsiConsole_from_local_function_parameter()
    {
        const string Source = @"
using System;
using Spectre.Console;

class TestClass
{
    static void TestMethod()
    {
        static void LocalFunction(IAnsiConsole ansiConsole) => Console.WriteLine(""Hello, World"");
    }
}";

        const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass
{
    static void TestMethod()
    {
        static void LocalFunction(IAnsiConsole ansiConsole) => ansiConsole.WriteLine(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(9, 64), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_do_not_use_variable_from_parent_method_in_static_local_function()
    {
        const string Source = @"
using System;
using Spectre.Console;

class TestClass
{
    static void TestMethod()
    {
        IAnsiConsole ansiConsole = null;
        static void LocalFunction() => Console.WriteLine(""Hello, World"");
    }
}";

        const string FixedSource = @"
using System;
using Spectre.Console;

class TestClass
{
    static void TestMethod()
    {
        IAnsiConsole ansiConsole = null;
        static void LocalFunction() => AnsiConsole.WriteLine(""Hello, World"");
    }
}";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, _expectedDiagnostic.WithLocation(10, 40), FixedSource);
    }

    [Fact]
    public async Task SystemConsole_replaced_with_AnsiConsole_in_top_level_statements()
    {
        const string Source = @"
using System;

Console.WriteLine(""Hello, World"");
";

        const string FixedSource = @"
using System;
using Spectre.Console;

AnsiConsole.WriteLine(""Hello, World"");
";

        await SpectreAnalyzerVerifier<UseSpectreInsteadOfSystemConsoleAnalyzer>
            .VerifyCodeFixAsync(Source, OutputKind.ConsoleApplication, _expectedDiagnostic.WithLocation(4, 1),
                FixedSource);
    }
}
