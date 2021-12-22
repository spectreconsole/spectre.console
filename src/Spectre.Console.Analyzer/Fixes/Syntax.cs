using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Spectre.Console.Analyzer;

internal static class Syntax
{
    public static readonly UsingDirectiveSyntax SpectreUsing = UsingDirective(QualifiedName(IdentifierName("Spectre"), IdentifierName("Console")));
}