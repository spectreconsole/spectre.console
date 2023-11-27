namespace Spectre.Console.Analyzer.Tests;

public static class SpectreAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    public static Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
        => VerifyCodeFixAsync(source, OutputKind.DynamicallyLinkedLibrary, new[] { expected }, fixedSource);

    public static Task VerifyCodeFixAsync(string source, OutputKind outputKind, DiagnosticResult expected, string fixedSource)
        => VerifyCodeFixAsync(source, outputKind, new[] { expected }, fixedSource);

    private static Task VerifyCodeFixAsync(string source, OutputKind outputKind, IEnumerable<DiagnosticResult> expected, string fixedSource)
    {
        var test = new Test
        {
            TestCode = source,
            TestState =
            {
                OutputKind = outputKind,
            },
            FixedCode = fixedSource,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new Test
        {
            TestCode = source,
            CompilerDiagnostics = CompilerDiagnostics.All,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        return test.RunAsync();
    }

    // Code fix tests support both analyzer and code fix testing. This test class is derived from the code fix test
    // to avoid the need to maintain duplicate copies of the customization work.
    private class Test : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
    {
        public Test()
        {
            ReferenceAssemblies = CodeAnalyzerHelper.CurrentSpectre;
            TestBehaviors |= TestBehaviors.SkipGeneratedCodeCheck;
        }

        protected override IEnumerable<CodeFixProvider> GetCodeFixProviders()
        {
            var analyzer = new TAnalyzer();
            foreach (var provider in CodeFixProviderDiscovery.GetCodeFixProviders(Language))
            {
                if (analyzer.SupportedDiagnostics.Any(diagnostic => provider.FixableDiagnosticIds.Contains(diagnostic.Id)))
                {
                    yield return provider;
                }
            }
        }
    }
}
