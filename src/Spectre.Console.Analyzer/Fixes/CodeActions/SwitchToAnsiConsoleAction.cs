using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Simplification;

namespace Spectre.Console.Analyzer.CodeActions;

/// <summary>
/// Code action to change calls to System.Console to AnsiConsole.
/// </summary>
public class SwitchToAnsiConsoleAction : CodeAction
{
    private readonly Document _document;
    private readonly InvocationExpressionSyntax _originalInvocation;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwitchToAnsiConsoleAction"/> class.
    /// </summary>
    /// <param name="document">Document to change.</param>
    /// <param name="originalInvocation">The method to change.</param>
    /// <param name="title">Title of the fix.</param>
    public SwitchToAnsiConsoleAction(Document document, InvocationExpressionSyntax originalInvocation, string title)
    {
        _document = document;
        _originalInvocation = originalInvocation;
        Title = title;
    }

    /// <inheritdoc />
    public override string Title { get; }

    /// <inheritdoc />
    public override string EquivalenceKey => Title;

    /// <inheritdoc />
    protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(_document, cancellationToken).ConfigureAwait(false);
        var compilation = editor.SemanticModel.Compilation;

        var operation = editor.SemanticModel.GetOperation(_originalInvocation, cancellationToken) as IInvocationOperation;
        if (operation == null)
        {
            return _document;
        }

        // If there is an IAnsiConsole passed into the method then we'll use it.
        // otherwise we'll check for a field level instance.
        // if neither of those exist we'll fall back to the static param.
        var spectreConsoleSymbol = compilation.GetTypeByMetadataName("Spectre.Console.AnsiConsole");
        var iansiConsoleSymbol = compilation.GetTypeByMetadataName("Spectre.Console.IAnsiConsole");

        ISymbol? accessibleConsoleSymbol = spectreConsoleSymbol;
        if (iansiConsoleSymbol != null)
        {
            var isInStaticContext = IsInStaticContext(operation, cancellationToken, out var parentStaticMemberStartPosition);

            foreach (var symbol in editor.SemanticModel.LookupSymbols(operation.Syntax.GetLocation().SourceSpan.Start))
            {
                // LookupSymbols check the accessibility of the symbol, but it can
                // suggest instance members when the current context is static.
                var symbolType = symbol switch
                {
                    IParameterSymbol parameter => parameter.Type,
                    IFieldSymbol field when !isInStaticContext || field.IsStatic => field.Type,
                    IPropertySymbol { GetMethod: not null } property when !isInStaticContext || property.IsStatic => property.Type,
                    ILocalSymbol local => local.Type,
                    _ => null,
                };

                // Locals can be returned even if there are not valid in the current context. For instance,
                // it can return locals declared after the current location. Or it can return locals that
                // should not be accessible in a static local function.
                //
                // void Sample()
                // {
                //    int local = 0;
                //    static void LocalFunction() => local; <-- local is invalid here but LookupSymbols suggests it
                // }
                //
                // Parameters from the ancestor methods or local functions are also returned even if the operation is in a static local function.
                if (symbol.Kind is SymbolKind.Local or SymbolKind.Parameter)
                {
                    var localPosition = symbol.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax(cancellationToken).GetLocation().SourceSpan.Start;

                    // The local is not part of the source tree
                    if (localPosition == null)
                    {
                        break;
                    }

                    // The local is declared after the current expression
                    if (localPosition > _originalInvocation.Span.Start)
                    {
                        break;
                    }

                    // The local is declared outside the static local function
                    if (isInStaticContext && localPosition < parentStaticMemberStartPosition)
                    {
                        break;
                    }
                }

                if (IsOrImplementSymbol(symbolType, iansiConsoleSymbol))
                {
                    accessibleConsoleSymbol = symbol;
                    break;
                }
            }
        }

        if (accessibleConsoleSymbol == null)
        {
            return _document;
        }

        // Replace the original invocation
        var generator = editor.Generator;
        var consoleExpression = accessibleConsoleSymbol switch
        {
            ITypeSymbol typeSymbol => generator.TypeExpression(typeSymbol, addImport: true).WithAdditionalAnnotations(Simplifier.AddImportsAnnotation),
            _ => generator.IdentifierName(accessibleConsoleSymbol.Name),
        };

        var newExpression = generator.InvocationExpression(generator.MemberAccessExpression(consoleExpression, operation.TargetMethod.Name), _originalInvocation.ArgumentList.Arguments)
                .WithLeadingTrivia(_originalInvocation.GetLeadingTrivia())
                .WithTrailingTrivia(_originalInvocation.GetTrailingTrivia());

        editor.ReplaceNode(_originalInvocation, newExpression);

        return editor.GetChangedDocument();
    }

    private static bool IsOrImplementSymbol(ITypeSymbol? symbol, ITypeSymbol interfaceSymbol)
    {
        if (symbol == null)
        {
            return false;
        }

        if (SymbolEqualityComparer.Default.Equals(symbol, interfaceSymbol))
        {
            return true;
        }

        foreach (var iface in symbol.AllInterfaces)
        {
            if (SymbolEqualityComparer.Default.Equals(iface, interfaceSymbol))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsInStaticContext(IOperation operation, CancellationToken cancellationToken, out int parentStaticMemberStartPosition)
    {
        // Local functions can be nested, and an instance local function can be declared
        // in a static local function. So, you need to continue to check ancestors when a
        // local function is not static.
        foreach (var member in operation.Syntax.Ancestors())
        {
            if (member is LocalFunctionStatementSyntax localFunction)
            {
                var symbol = operation.SemanticModel!.GetDeclaredSymbol(localFunction, cancellationToken);
                if (symbol != null && symbol.IsStatic)
                {
                    parentStaticMemberStartPosition = localFunction.GetLocation().SourceSpan.Start;
                    return true;
                }
            }
            else if (member is LambdaExpressionSyntax lambdaExpression)
            {
                var symbol = operation.SemanticModel!.GetSymbolInfo(lambdaExpression, cancellationToken).Symbol;
                if (symbol != null && symbol.IsStatic)
                {
                    parentStaticMemberStartPosition = lambdaExpression.GetLocation().SourceSpan.Start;
                    return true;
                }
            }
            else if (member is AnonymousMethodExpressionSyntax anonymousMethod)
            {
                var symbol = operation.SemanticModel!.GetSymbolInfo(anonymousMethod, cancellationToken).Symbol;
                if (symbol != null && symbol.IsStatic)
                {
                    parentStaticMemberStartPosition = anonymousMethod.GetLocation().SourceSpan.Start;
                    return true;
                }
            }
            else if (member is MethodDeclarationSyntax methodDeclaration)
            {
                parentStaticMemberStartPosition = methodDeclaration.GetLocation().SourceSpan.Start;

                var symbol = operation.SemanticModel!.GetDeclaredSymbol(methodDeclaration, cancellationToken);
                return symbol != null && symbol.IsStatic;
            }
        }

        parentStaticMemberStartPosition = -1;
        return false;
    }
}