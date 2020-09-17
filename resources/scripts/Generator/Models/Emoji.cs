using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Humanizer;

namespace Generator.Models
{
    public class Emoji
    {
        private static readonly string[] _headers = { "count", "code", "sample", "name" };

        private Emoji(string identifier, string name, string code, string description)
        {
            Identifier = identifier;
            Name = name;
            Code = code;
            Description = description;
            NormalizedCode = Code.Replace("\\U", "U+");
            HasCombinators = Code.Split(new[] { "\\U" }, System.StringSplitOptions.RemoveEmptyEntries).Length > 1;
        }

        public string Identifier { get; set; }
        public string Code { get; }
        public string NormalizedCode { get; }
        public string Name { get; }
        public string Description { get; set; }
        public bool HasCombinators { get; set; }

        public static IEnumerable<Emoji> Parse(IHtmlDocument document)
        {
            var rows = document
                .GetNodes<IHtmlTableRowElement>(predicate: row =>
                    row.Cells.Length >= _headers.Length && // Filter out rows that don't have enough cells.
                    row.Cells.All(x => x.LocalName == TagNames.Td)); // We're only interested in td cells, not th.

            foreach (var row in rows)
            {
                var dictionary = _headers
                    .Zip(row.Cells, (header, cell) => (Header: header, cell.TextContent.Trim()))
                    .ToDictionary(x => x.Item1, x => x.Item2);

                var code = TransformCode(dictionary["code"]);
                var identifier = TransformName(dictionary["name"])
                    .Replace("-", "_")
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty);

                var description = dictionary["name"].Humanize();

                var name = identifier
                    .Replace("1st", "first")
                    .Replace("2nd", "second")
                    .Replace("3rd", "third")
                    .Pascalize();

                yield return new Emoji(identifier, name, code, description);
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
                .Replace(' ', '_')
                .Replace("’s", "s")
                .Replace("’", "_")
                .Replace("&", "and")
                .Replace("#", "hash")
                .Replace("*", "star")
                .Replace("!", string.Empty)
                .Trim()
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
