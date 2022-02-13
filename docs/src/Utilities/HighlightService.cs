using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Docs.Utilities;

internal static class HighlightService
{
    internal enum HighlightOption
    {
        All,
        Body
    }

    private static readonly AdhocWorkspace _emptyWorkspace = new();

    public static async Task<string> Highlight(Compilation compilation, ISymbol symbol, HighlightOption option = HighlightOption.All)
    {
        var syntaxReference = symbol.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxReference == null)
        {
            return null;
        }

        var syntax = await syntaxReference.GetSyntaxAsync();
        var indent = GetIndent(syntax.GetLeadingTrivia());
        var model = compilation.GetSemanticModel(syntaxReference.SyntaxTree);

        var methodWithBodySyntax = syntax as BaseMethodDeclarationSyntax;

        TextSpan textSpan;
        switch (option)
        {
            case HighlightOption.Body when methodWithBodySyntax is { Body: { } }:
                {
                    syntax = methodWithBodySyntax.Body;
                    indent = GetIndent(methodWithBodySyntax.Body.Statements.First().GetLeadingTrivia());
                    textSpan = TextSpan.FromBounds(syntax.Span.Start + 1, syntax.Span.End - 1);
                    break;
                }
            case HighlightOption.Body when methodWithBodySyntax is { ExpressionBody: { } }:
                {
                    syntax = methodWithBodySyntax.ExpressionBody;
                    textSpan = syntax.Span;
                    break;
                }
            case HighlightOption.All:
            default:
                textSpan = syntax.Span;
                break;
        }

        var text = await syntaxReference.SyntaxTree.GetTextAsync();
        // we need a workspace, but it seems it is only used to resolve a few services and nothing else so an empty one will suffice
        return HighlightElement(_emptyWorkspace, model, text, textSpan, indent);
    }

    private static int GetIndent(SyntaxTriviaList leadingTrivia)
    {
        var whitespace = leadingTrivia.FirstOrDefault(i => i.Kind() == SyntaxKind.WhitespaceTrivia);
        return whitespace == default ? 0 : whitespace.Span.Length;
    }

    private static string HighlightElement(Workspace workspace, SemanticModel semanticModel, SourceText fullSourceText,
        TextSpan textSpan, int indent)
    {

        var classifiedSpans = Classifier.GetClassifiedSpans(semanticModel, textSpan, workspace);
        return HighlightElement(classifiedSpans, fullSourceText, indent);
    }

    private static string HighlightElement(IEnumerable<ClassifiedSpan> classifiedSpans, SourceText fullSourceText, int indent)
    {

        var ranges = classifiedSpans.Select(classifiedSpan =>
            new Range(classifiedSpan.ClassificationType, classifiedSpan.TextSpan, fullSourceText)).ToList();

        // the classified text won't include the whitespace so we need to add to fill in those gaps.
        ranges = FillGaps(fullSourceText, ranges).ToList();

        var sb = new StringBuilder();

        foreach (var range in ranges)
        {
            var cssClass = ClassificationTypeToPrismClass(range.ClassificationType);
            if (string.IsNullOrWhiteSpace(cssClass))
            {
                sb.Append(range.Text);
            }
            else
            {
                // include the prism css class but also include the roslyn classification.
                sb.Append(
                    $"<span class=\"token {cssClass} roslyn-{range.ClassificationType.Replace(" ", "-")}\">{range.Text}</span>");
            }
        }

        // there might be a way to do this with roslyn, but for now we'll just normalize everything off of the length of the
        // leading trivia of the element we are looking at.
        var indentString = new string(' ', indent);
        var allLines = sb.ToString()
            .ReplaceLineEndings()
            .Split(Environment.NewLine)
            .Select(i => i.StartsWith(indentString) == false ? i : i[indent..]);

        return string.Join(Environment.NewLine, allLines);
    }

    private static string ClassificationTypeToPrismClass(string rangeClassificationType)
    {
        if (rangeClassificationType == null)
            return string.Empty;

        switch (rangeClassificationType)
        {
            case ClassificationTypeNames.Identifier:
                return "symbol";
            case ClassificationTypeNames.LocalName:
                return "variable";
            case ClassificationTypeNames.ParameterName:
            case ClassificationTypeNames.PropertyName:
            case ClassificationTypeNames.EnumMemberName:
            case ClassificationTypeNames.FieldName:
                return "property";
            case ClassificationTypeNames.ClassName:
            case ClassificationTypeNames.StructName:
            case ClassificationTypeNames.RecordClassName:
            case ClassificationTypeNames.RecordStructName:
            case ClassificationTypeNames.InterfaceName:
            case ClassificationTypeNames.DelegateName:
            case ClassificationTypeNames.EnumName:
            case ClassificationTypeNames.ModuleName:
            case ClassificationTypeNames.TypeParameterName:
                return "title.class";
            case ClassificationTypeNames.MethodName:
            case ClassificationTypeNames.ExtensionMethodName:
                return "title.function";
            case ClassificationTypeNames.Comment:
                return "comment";
            case ClassificationTypeNames.Keyword:
            case ClassificationTypeNames.ControlKeyword:
            case ClassificationTypeNames.PreprocessorKeyword:
                return "keyword";
            case ClassificationTypeNames.StringLiteral:
            case ClassificationTypeNames.VerbatimStringLiteral:
                return "string";
            case ClassificationTypeNames.NumericLiteral:
                return "number";
            case ClassificationTypeNames.Operator:
            case ClassificationTypeNames.StringEscapeCharacter:
                return "operator";
            case ClassificationTypeNames.Punctuation:
                return "punctuation";
            case ClassificationTypeNames.StaticSymbol:
                return string.Empty;
            case ClassificationTypeNames.XmlDocCommentComment:
            case ClassificationTypeNames.XmlDocCommentDelimiter:
            case ClassificationTypeNames.XmlDocCommentName:
            case ClassificationTypeNames.XmlDocCommentText:
            case ClassificationTypeNames.XmlDocCommentAttributeName:
            case ClassificationTypeNames.XmlDocCommentAttributeQuotes:
            case ClassificationTypeNames.XmlDocCommentAttributeValue:
            case ClassificationTypeNames.XmlDocCommentEntityReference:
            case ClassificationTypeNames.XmlDocCommentProcessingInstruction:
            case ClassificationTypeNames.XmlDocCommentCDataSection:
                return "comment";
            default:
                return rangeClassificationType.Replace(" ", "-");
        }
    }

    private static IEnumerable<Range> FillGaps(SourceText text, IList<Range> ranges)
    {
        const string WhitespaceClassification = null;
        var current = ranges.First().TextSpan.Start;
        var end = ranges.Last().TextSpan.End;
        Range previous = null;

        foreach (var range in ranges)
        {
            var start = range.TextSpan.Start;
            if (start > current)
            {
                yield return new Range(WhitespaceClassification, TextSpan.FromBounds(current, start), text);
            }

            if (previous == null || range.TextSpan != previous.TextSpan)
            {
                yield return range;
            }

            previous = range;
            current = range.TextSpan.End;
        }

        if (current < end)
        {
            yield return new Range(WhitespaceClassification, TextSpan.FromBounds(current, end), text);
        }
    }

    private class Range
    {
        private ClassifiedSpan ClassifiedSpan { get; }
        public string Text { get; }

        public Range(string classification, TextSpan span, SourceText text) :
            this(classification, span, text.GetSubText(span).ToString())
        {
        }

        private Range(string classification, TextSpan span, string text) :
            this(new ClassifiedSpan(classification, span), text)
        {
        }

        private Range(ClassifiedSpan classifiedSpan, string text)
        {
            ClassifiedSpan = classifiedSpan;
            Text = text;
        }

        public string ClassificationType => ClassifiedSpan.ClassificationType;

        public TextSpan TextSpan => ClassifiedSpan.TextSpan;
    }
}