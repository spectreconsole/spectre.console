using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
        var originalCaller = ((MemberAccessExpressionSyntax)_originalInvocation.Expression).Name.ToString();

        var syntaxTree = await _document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
        if (syntaxTree == null)
        {
            return _document;
        }

        var root = (CompilationUnitSyntax)await syntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);

        // If there is an ansiConsole passed into the method then we'll use it.
        // otherwise we'll check for a field level instance.
        // if neither of those exist we'll fall back to the static param.
        var ansiConsoleParameterDeclaration = GetAnsiConsoleParameterDeclaration();
        var ansiConsoleFieldIdentifier = GetAnsiConsoleFieldDeclaration();
        var ansiConsoleIdentifier = ansiConsoleParameterDeclaration ??
                                    ansiConsoleFieldIdentifier ??
                                    Constants.StaticInstance;

        // Replace the System.Console call with a call to the identifier above.
        var newRoot = root.ReplaceNode(
            _originalInvocation,
            GetImportedSpectreCall(originalCaller, ansiConsoleIdentifier));

        // If we are calling the static instance and Spectre isn't imported yet we should do so.
        if (ansiConsoleIdentifier == Constants.StaticInstance && root.Usings.ToList().All(i => i.Name.ToString() != Constants.SpectreConsole))
        {
            newRoot = newRoot.AddUsings(Syntax.SpectreUsing);
        }

        return _document.WithSyntaxRoot(newRoot);
    }

    private string? GetAnsiConsoleParameterDeclaration()
    {
        return _originalInvocation
            .Ancestors().OfType<MethodDeclarationSyntax>()
            .First()
            .ParameterList.Parameters
            .FirstOrDefault(i => i.Type?.NormalizeWhitespace()?.ToString() == "IAnsiConsole")
            ?.Identifier.Text;
    }

    private string? GetAnsiConsoleFieldDeclaration()
    {
        // let's look to see if our call is in a static method.
        // if so we'll only want to look for static IAnsiConsoles
        // and vice-versa if we aren't.
        var isStatic = _originalInvocation
            .Ancestors()
            .OfType<MethodDeclarationSyntax>()
            .First()
            .Modifiers.Any(i => i.IsKind(SyntaxKind.StaticKeyword));

        return _originalInvocation
            .Ancestors().OfType<ClassDeclarationSyntax>()
            .First()
            .Members
            .OfType<FieldDeclarationSyntax>()
            .FirstOrDefault(i =>
                i.Declaration.Type.NormalizeWhitespace().ToString() == "IAnsiConsole" &&
                (!isStatic ^ i.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword))))
            ?.Declaration.Variables.First().Identifier.Text;
    }

    private ExpressionSyntax GetImportedSpectreCall(string originalCaller, string ansiConsoleIdentifier)
    {
        return ExpressionStatement(
                InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(ansiConsoleIdentifier),
                            IdentifierName(originalCaller)))
                    .WithArgumentList(_originalInvocation.ArgumentList)
                    .WithTrailingTrivia(_originalInvocation.GetTrailingTrivia())
                    .WithLeadingTrivia(_originalInvocation.GetLeadingTrivia()))
        .Expression;
    }
}