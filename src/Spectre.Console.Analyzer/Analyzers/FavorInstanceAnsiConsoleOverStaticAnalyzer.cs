using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Spectre.Console.Analyzer
{
    /// <summary>
    /// Analyzer to suggest using available instances of AnsiConsole over the static methods.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class FavorInstanceAnsiConsoleOverStaticAnalyzer : BaseAnalyzer
    {
        private static readonly DiagnosticDescriptor _diagnosticDescriptor =
            Descriptors.S1010_FavorInstanceAnsiConsoleOverStatic;

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(_diagnosticDescriptor);

        /// <inheritdoc />
        protected override void AnalyzeCompilation(CompilationStartAnalysisContext compilationStartContext)
        {
            compilationStartContext.RegisterOperationAction(
                context =>
                {
                    var ansiConsoleType = context.Compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsole");

                    // if this operation isn't an invocation against one of the System.Console methods
                    // defined in _methods then we can safely stop analyzing and return;
                    var invocationOperation = (IInvocationOperation)context.Operation;
                    if (!Equals(invocationOperation.TargetMethod.ContainingType, ansiConsoleType))
                    {
                        return;
                    }

                    if (!HasFieldAnsiConsole(invocationOperation.Syntax) &&
                        !HasParameterAnsiConsole(invocationOperation.Syntax))
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

        private static bool HasParameterAnsiConsole(SyntaxNode syntaxNode)
        {
            return syntaxNode
                .Ancestors().OfType<MethodDeclarationSyntax>()
                .First()
                .ParameterList.Parameters
                .Any(i => i.Type.NormalizeWhitespace().ToString() == "IAnsiConsole");
        }

        private static bool HasFieldAnsiConsole(SyntaxNode syntaxNode)
        {
            var isStatic = syntaxNode
                .Ancestors()
                .OfType<MethodDeclarationSyntax>()
                .First()
                .Modifiers.Any(i => i.Kind() == SyntaxKind.StaticKeyword);

            return syntaxNode
                .Ancestors().OfType<ClassDeclarationSyntax>()
                .First()
                .Members
                .OfType<FieldDeclarationSyntax>()
                .Any(i =>
                    i.Declaration.Type.NormalizeWhitespace().ToString() == "IAnsiConsole" &&
                    (!isStatic ^ i.Modifiers.Any(modifier => modifier.Kind() == SyntaxKind.StaticKeyword)));
        }
    }
}