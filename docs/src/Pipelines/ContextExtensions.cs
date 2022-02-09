using System;
using System.Linq;
using Docs.Pipelines;
using Statiq.CodeAnalysis;
using Statiq.Common;

namespace Docs.Pipeline;

internal static class ContextExtensions
{
    public static bool TryGetCommentIdDocument(this IExecutionContext context, string commentId, out IDocument document,
        out string error)
    {
        context.ThrowIfNull(nameof(context));

        var documents = context.Outputs.FromPipeline(nameof(Api)).Flatten();
        var matches = documents
            .Where(x =>
                x.GetString(CodeAnalysisKeys.CommentId)?.Equals(commentId, StringComparison.OrdinalIgnoreCase) == true)
            .ToImmutableDocumentArray();

        if (matches.Length == 1)
        {
            document = matches[0];
            error = default;
            return true;
        }

        document = default;
        error = matches.Length > 1
            ? $"Multiple ambiguous matching documents found for commentId \"{commentId}\""
            : $"Couldn't find document with xref \"{commentId}\"";
        return false;
    }
}