using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Spectre.Console.Analyzer
{
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

                    var ansiConsoleType = context.Compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsole");
                    var ansiConsoleExtensionsType = context.Compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsoleExtensions");

                    if (!Equals(methodSymbol.ContainingType, ansiConsoleType) && !Equals(methodSymbol.ContainingType, ansiConsoleExtensionsType))
                    {
                        return;
                    }

                    var model = context.Compilation.GetSemanticModel(context.Operation.Syntax.SyntaxTree);
                    var parentInvocations = invocationOperation
                        .Syntax.Ancestors()
                        .OfType<InvocationExpressionSyntax>()
                        .Select(i => model.GetOperation(i))
                        .OfType<IInvocationOperation>()
                        .ToList();

                    var liveTypes = Constants.LiveRenderables
                        .Select(i => context.Compilation.GetTypeByMetadataName(i))
                        .ToImmutableArray();

                    if (parentInvocations.All(parent =>
                        parent.TargetMethod.Name != "Start" ||
                        !liveTypes.Contains(parent.TargetMethod.ContainingType)))
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
}