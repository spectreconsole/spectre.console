using System;
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
            var table = new XElement("table", new XAttribute("class", "table"));
            var header = new XElement("tr", new XAttribute("class", "emoji-row"));
            header.Add(new XElement("th", ""));
            header.Add(new XElement("th", "Markup"));
            header.Add(new XElement("th", "Constant"));
            table.Add(header);

            foreach (var emoji in emojis)
            {
                var code = emoji.Code.Replace("U+0000", "U+").Replace("U+000", "U+");
                var icon = string.Format("&#x{0};", emoji.Code.Replace("U+", string.Empty));

                var row = new XElement("tr");
                row.Add(new XElement("td", icon));
                row.Add(new XElement("td", new XElement("code", $":{emoji.Id}:")));
                row.Add(new XElement("td", new XElement("code", emoji.Name)));

                table.Add(row);
            }

            return table.ToString()
                .Replace("&amp;#x", "&#x");
        }
    }
}