namespace Spectre.Console.Analyzer.FixProviders;

/// <summary>
/// Fix provider to change System.Console calls to AnsiConsole calls.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public class SystemConsoleToAnsiConsoleFix : CodeFixProvider
{
    /// <inheritdoc />
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(
        Descriptors.S1000_UseAnsiConsoleOverSystemConsole.Id);

    /// <inheritdoc />
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root != null)
        {
            var methodDeclaration = root.FindNode(context.Span).FirstAncestorOrSelf<InvocationExpressionSyntax>();
            if (methodDeclaration != null)
            {
                context.RegisterCodeFix(
                    new SwitchToAnsiConsoleAction(
                        context.Document,
                        methodDeclaration,
                        "Convert static call to AnsiConsole to Spectre.Console.AnsiConsole"),
                    context.Diagnostics);
            }
        }
    }
}