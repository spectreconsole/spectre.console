using Statiq.Common;
using System.Collections.Generic;
using System.Linq;

namespace Docs
{
    public static class DocumentExtensions
    {
        public static string GetDescription(this IDocument document)
        {
            return document?.GetString(Constants.Description, string.Empty) ?? string.Empty;
        }

        public static bool HasVisibleChildren(this IDocument document)
        {
            if (document != null)
            {
                return document.HasChildren() && document.GetChildren().Any(x => x.IsVisible());
            }
            return false;
        }

        public static bool IsVisible(this IDocument document)
        {
            return !document.GetBool(Constants.Hidden, false);
        }

        public static IEnumerable<IDocument> OnlyVisible(this IEnumerable<IDocument> source)
        {
            return source.Where(x => x.IsVisible());
        }
    }
}
