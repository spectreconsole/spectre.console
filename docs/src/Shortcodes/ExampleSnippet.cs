using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Statiq.Common;
using Document = Microsoft.CodeAnalysis.Document;

namespace Docs.Shortcodes;

public class ExampleSnippet : Shortcode
{
    private static readonly ConcurrentDictionary<string,
            Lazy<Task<Dictionary<string, (Document Document, int Indent, TextSpan TextSpan)>>>>
        _solutionSyntaxTrees = new();

    public override async Task<ShortcodeResult> ExecuteAsync(KeyValuePair<string, string>[] args, string content,
        IDocument document, IExecutionContext context)
    {
        var solutionFileKey = args.FirstOrDefault(i => i.Key == "solution").Value ?? "ExampleSolution";
        var solutionFile = context.GetString(solutionFileKey);
        var symbol = args.First(i => i.Key == "symbol").Value.ToLowerInvariant();

        var syntaxTrees = await _solutionSyntaxTrees.GetOrAdd(solutionFile,
            s => new Lazy<Task<Dictionary<string, (Document, int, TextSpan)>>>(async () =>
                await GetSyntaxTrees(s, context))).Value;

        if (syntaxTrees.ContainsKey(symbol) == false)
        {
            context.LogWarning("Could not find snippet {symbol}", symbol);
            return $"<div class=\"bg-red\">unknown symbol name - {symbol}.</div>";
        }

        var (doc, indent, textSpan) = syntaxTrees[symbol];
        var highlightElement = await HighlightElement(doc, indent, textSpan);
        ShortcodeResult shortcodeResult = $"<pre><code>{highlightElement}</code></pre>";
        return shortcodeResult;
    }

    public static AdhocWorkspace GetWorkspace(IAnalyzerManager manager)
    {
        var workspace = new AdhocWorkspace();
        if (!string.IsNullOrEmpty(manager.SolutionFilePath))
        {
            var solutionInfo = SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Default, manager.SolutionFilePath);
            workspace.AddSolution(solutionInfo);
        }

        foreach (var analyzerResult in  manager.Projects.Values)
        {
            analyzerResult.
            analyzerResult.AddToWorkspace(workspace, false);
        }

        return workspace;
    }

    private static async
        Task<Dictionary<string, (Document Document, int Indent, TextSpan SourceSpan)>> GetSyntaxTrees(
            string solutionFile, IExecutionContext context)
    {
        context.LogInformation("Parsing {solutionFile}", solutionFile);
        var syntaxTreeDictionary =
            new Dictionary<string, (Document Document, int Indent, TextSpan SourceSpan)>();
        var analyzerManager = new AnalyzerManager(solutionFile,
            new AnalyzerManagerOptions { LoggerFactory = context.GetRequiredService<ILoggerFactory>() });

        using var workspace = GetWorkspace(analyzerManager);
        foreach (var project in workspace.CurrentSolution.Projects)
        {
            context.LogInformation("Parsing {project}", project.Name);
            if (project.Name.StartsWith("Spectre.Console")) continue;

            foreach (var document in project.Documents)
            {
                var syntaxTree = await document.GetSyntaxTreeAsync();
                var model = await document.GetSemanticModelAsync();
                if (syntaxTree == null || model == null)
                {
                    continue;
                }

                var root = await syntaxTree.GetRootAsync();
                var classes = root
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>();

                foreach (var classDeclarationSyntax in classes)
                {
                    var className = classDeclarationSyntax.Identifier.ToString();

                    var classKey = $"{project.Name}.{className}".ToLowerInvariant();
                    if (syntaxTreeDictionary.ContainsKey(classKey) == false)
                    {
                        syntaxTreeDictionary.Add(classKey,
                            (
                                document,
                                GetIndent(classDeclarationSyntax.GetLeadingTrivia()),
                                classDeclarationSyntax.GetLocation().SourceSpan)
                            );
                    }

                    var methods = classDeclarationSyntax
                        .DescendantNodes()
                        .OfType<MethodDeclarationSyntax>();

                    foreach (var methodDeclarationSyntax in methods)
                    {
                        var methodName = methodDeclarationSyntax.Identifier.ToString();
                        var methodKey = $"{project.Name}.{className}.{methodName}".ToLowerInvariant();
                        syntaxTreeDictionary.Add(methodKey,
                            (
                                document,
                                GetIndent(methodDeclarationSyntax.GetLeadingTrivia()),
                                methodDeclarationSyntax.GetLocation().SourceSpan)
                            );

                        // add a special named case with just the body of a method.
                        var bodySyntax = methodDeclarationSyntax.Body;
                        if (bodySyntax == null) continue;

                        var leadingTrivia = bodySyntax.Statements.First().GetLeadingTrivia();
                        var location = bodySyntax.GetLocation();
                        var bodyKey = $"{project.Name}.{className}.{methodName}::body".ToLowerInvariant();
                        syntaxTreeDictionary.Add(bodyKey,
                            (document, GetIndent(leadingTrivia),
                                TextSpan.FromBounds(location.SourceSpan.Start + 1, location.SourceSpan.End - 1)));
                    }
                }
            }
        }

        context.LogInformation("Parsing {solutionFile} complete", solutionFile);
        return syntaxTreeDictionary;
    }

    private static int GetIndent(SyntaxTriviaList leadingTrivia)
    {
        var whitespace = leadingTrivia.FirstOrDefault(i => i.Kind() == SyntaxKind.WhitespaceTrivia);
        return whitespace == default ? 0 : whitespace.Span.Length;
    }

    private static async Task<string> HighlightElement(Document document, int indent, TextSpan textSpan)
    {
        var classifiedSpans = await Classifier.GetClassifiedSpansAsync(document, textSpan);
        var fullSourceText = await document.GetTextAsync();
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

        public Range(ClassifiedSpan classifiedSpan, string text)
        {
            ClassifiedSpan = classifiedSpan;
            Text = text;
        }

        public string ClassificationType => ClassifiedSpan.ClassificationType;

        public TextSpan TextSpan => ClassifiedSpan.TextSpan;
    }
}