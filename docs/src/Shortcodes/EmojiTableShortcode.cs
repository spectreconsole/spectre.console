using System.Collections.Generic;
using System.Linq;
using Statiq.Common;
using System.Xml.Linq;
using Docs.Pipelines;
using Docs.Models;

namespace Docs.Shortcodes
{
    public class EmojiTableShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            var emojis = context.Outputs
                .FromPipeline(nameof(EmojiPipeline))
                .OfType<ObjectDocument<List<Emoji>>>()
                .First().Object;

            // Headers
            var table = new XElement("table", new XAttribute("class", "table"), new XAttribute("id", "emoji-results"));
            var tHead = new XElement("thead");
            var header = new XElement("tr", new XAttribute("class", "emoji-row-header"));
            header.Add(new XElement("th", ""));
            header.Add(new XElement("th", "Markup"));
            header.Add(new XElement("th", "Constant", new XAttribute("class", "hidden md:table-cell")));
            tHead.Add(header);
            table.Add(tHead);

            var tBody = new XElement("tbody");

            foreach (var emoji in emojis)
            {
                var icon = $"&#x{emoji.Code.Replace("U+", string.Empty)};";

                var row = new XElement("tr", new XAttribute("class", "search-row"));
                row.Add(new XElement("td", icon));
                row.Add(new XElement("td", new XElement("code", $":{emoji.Id}:")));
                row.Add(new XElement("td", new XElement("code", emoji.Name), new XAttribute("class", "hidden md:table-cell")));

                tBody.Add(row);
            }

            table.Add(tBody);

            return table.ToString()
                .Replace("&amp;#x", "&#x");
        }
    }
}