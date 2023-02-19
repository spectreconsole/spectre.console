namespace Spectre.Console.Analyzer;

/// <summary>
/// Analyzer to detect calls to live renderables within a live renderable context.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[Shared]
public class NoConcurrentLiveRenderablesAnalyzer : SpectreAnalyzer
{
    private static readonly DiagnosticDescriptor _diagnosticDescriptor =
        Descriptors.S1020_AvoidConcurrentCallsToMultipleLiveRenderables;

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(_diagnosticDescriptor);

    /// <inheritdoc />
    protected override void AnalyzeCompilation(CompilationStartAnalysisContext compilationStartContext)
    {
        compilationStartContext.RegisterOperationAction(
            context =>
            {
                var invocationOperation = (IInvocationOperation)context.Operation;
                var methodSymbol = invocationOperation.TargetMethod;

                const string StartMethod = "Start";
                if (methodSymbol.Name != StartMethod)
                {
                    return;
                }

                var liveTypes = Constants.LiveRenderables
                    .Select(i => context.Compilation.GetTypeByMetadataName(i))
                    .ToImmutableArray();

                if (liveTypes.All(i => !SymbolEqualityComparer.Default.Equals(i, methodSymbol.ContainingType)))
                {
                    return;
                }

                var model = context.Operation.SemanticModel!;
                var parentInvocations = invocationOperation
                    .Syntax.Ancestors()
                    .OfType<InvocationExpressionSyntax>()
                    .Select(i => model.GetOperation(i, context.CancellationToken))
                    .OfType<IInvocationOperation>()
                    .ToList();

                if (parentInvocations.All(parent =>
                    parent.TargetMethod.Name != StartMethod || !liveTypes.Contains(parent.TargetMethod.ContainingType)))
                {
                    return;
                }

                var displayString = SymbolDisplay.ToDisplayString(
                    methodSymbol,
                    SymbolDisplayFormat.CSharpShortErrorMessageFormat
                        .WithParameterOptions(SymbolDisplayParameterOptions.None)
                        .WithGenericsOptions(SymbolDisplayGenericsOptions.None));

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        _diagnosticDescriptor,
                        invocationOperation.Syntax.GetLocation(),
                        displayString));
            }, OperationKind.Invocation);
    }
}