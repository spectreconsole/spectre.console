using System.Collections.Generic;
using System.Linq;
using Statiq.Common;

namespace Docs.Extensions
{
    public static class DocumentExtensions
    {
        public static string GetDescription(this IDocument document)
        {
            return document?.GetString(Constants.Description, string.Empty) ?? string.Empty;
        }

        public static bool IsVisible(this IDocument document)
        {
            return !document.GetBool(Constants.Hidden, false);
        }

        public static bool ShowLink(this IDocument document)
        {
            return !document.GetBool(Constants.NoLink, false);
        }

        public static IEnumerable<IDocument> OnlyVisible(this IEnumerable<IDocument> source)
        {
            return source.Where(x => x.IsVisible());
        }
    }
}
