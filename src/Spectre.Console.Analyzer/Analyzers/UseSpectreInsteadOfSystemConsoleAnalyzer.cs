using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Spectre.Console.Analyzer
{
    /// <summary>
    /// Analyzer to enforce the use of AnsiConsole over System.Console for known methods.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseSpectreInsteadOfSystemConsoleAnalyzer : BaseAnalyzer
    {
        private static readonly DiagnosticDescriptor _diagnosticDescriptor =
            Descriptors.S1000_UseAnsiConsoleOverSystemConsole;

        private static readonly string[] _methods = { "WriteLine", "Write" };

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
                var systemConsoleType = context.Compilation.GetTypeByMetadataName("System.Console");

                if (!Equals(invocationOperation.TargetMethod.ContainingType, systemConsoleType))
                {
                    return;
                }

                var methodName = System.Array.Find(_methods, i => i.Equals(invocationOperation.TargetMethod.Name));
                if (methodName == null)
                {
                    return;
                }

                var methodSymbol = invocationOperation.TargetMethod;

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