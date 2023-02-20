namespace Spectre.Console.Analyzer;

/// <summary>
/// Analyzer to detect calls to live renderables within a live renderable context.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[Shared]
public class NoPromptsDuringLiveRenderablesAnalyzer : SpectreAnalyzer
{
    private static readonly DiagnosticDescriptor _diagnosticDescriptor =
        Descriptors.S1021_AvoidPromptCallsDuringLiveRenderables;

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(_diagnosticDescriptor);

    /// <inheritdoc />
    protected override void AnalyzeCompilation(CompilationStartAnalysisContext compilationStartContext)
    {
        var ansiConsoleType = compilationStartContext.Compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsole");
        var ansiConsoleExtensionsType = compilationStartContext.Compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsoleExtensions");

        if (ansiConsoleType is null && ansiConsoleExtensionsType is null)
        {
            return;
        }

        compilationStartContext.RegisterOperationAction(
            context =>
            {
                // if this operation isn't an invocation against one of the System.Console methods
                // defined in _methods then we can safely stop analyzing and return;
                var invocationOperation = (IInvocationOperation)context.Operation;
                var methodSymbol = invocationOperation.TargetMethod;

                var promptMethods = ImmutableArray.Create("Ask", "Confirm", "Prompt");
                if (!promptMethods.Contains(methodSymbol.Name))
                {
                    return;
                }

                if (!SymbolEqualityComparer.Default.Equals(methodSymbol.ContainingType, ansiConsoleType) &&
                    !SymbolEqualityComparer.Default.Equals(methodSymbol.ContainingType, ansiConsoleExtensionsType))
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

                var liveTypes = Constants.LiveRenderables
                    .Select(i => context.Compilation.GetTypeByMetadataName(i))
                    .ToImmutableArray();

                if (parentInvocations.All(parent =>
                    parent.TargetMethod.Name != "Start" ||
                    !liveTypes.Contains(parent.TargetMethod.ContainingType, SymbolEqualityComparer.Default)))
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