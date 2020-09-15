using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Generator.Models
{
    public class Emoji
    {
        private static readonly string[] _headers = { "count", "code", "sample", "name" };

        private Emoji(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }

        public string Name { get; }

        public static IEnumerable<Emoji> Parse(IHtmlDocument document)
        {
            var rows = document
                .GetNodes<IHtmlTableRowElement>(predicate: row =>
                    row.Cells.Length >= _headers.Length && // Filter out rows that don't have enough cells.
                    row.Cells.All(x => x.LocalName == TagNames.Td)); // We're only interested in td cells, not th.

            foreach (var row in rows)
            {
                var dictionary = _headers
                    .Zip(row.Cells, (header, cell) => (header, cell.TextContent.Trim()))
                    .ToDictionary(x => x.Item1, x => x.Item2);

                var code = TransformCode(dictionary["code"]);
                var name = TransformName(dictionary["name"]);

                yield return new Emoji(code, name);
            }
        }

        private static string TransformName(string name)
        {
            return name.Replace(":", string.Empty)
                .Replace(",", string.Empty)
                .Replace(".", string.Empty)
                .Replace("\u201c", string.Empty)
                .Replace("\u201d", string.Empty)
                .Replace("\u229b", string.Empty)
                .Trim()
                .Replace(' ', '_')
                .ToLowerInvariant();
        }

        private static string TransformCode(string code)
        {
            var builder = new StringBuilder();

            foreach (var part in code.Split(' '))
            {
                builder.Append(part.Length == 6
                    ? part.Replace("+", "0000")
                    : part.Replace("+", "000"));
            }

            return builder.ToString().Replace("U", "\\U");
        }
    }
}
