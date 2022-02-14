using System.Collections.Generic;
using System.Linq;
using Statiq.Common;
using System.Xml.Linq;
using Docs.Pipelines;
using Docs.Models;

namespace Docs.Shortcodes
{
    public class ColorTableShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            // Get the definition.
            var colors = context.Outputs
                .FromPipeline(nameof(ColorsPipeline))
                .OfType<ObjectDocument<List<Color>>>()
                .First().Object;

            // Headers
            var table = new XElement("table", new XAttribute("class", "table"), new XAttribute("id", "color-results"));
            var tHead = new XElement("thead");
            var header = new XElement("tr", new XAttribute("class", "color-row"));
            header.Add(new XElement("th", ""));
            header.Add(new XElement("th", "#", new XAttribute("class", "hidden md:table-cell")));
            header.Add(new XElement("th", "Name"));
            header.Add(new XElement("th", "RGB", new XAttribute("class", "hidden md:table-cell")));
            header.Add(new XElement("th", "Hex"));
            header.Add(new XElement("th", new XElement("span", "System.ConsoleColor"), new XAttribute("class", "break-all")));
            tHead.Add(header);
            table.Add(tHead);

            var tBody = new XElement("tbody");
            foreach (var color in colors)
            {
                var rep = new XElement("td",
                    new XElement("span",
                    new XAttribute("class", "inline-block w-4 md:w-16 h-4 border border-black border-opacity-75 rounded"),
                    new XAttribute("style", $"background-color:{color.Hex};")));
                var name = new XElement("td", new XElement("code", color.Number.ToString()), new XAttribute("class", "hidden md:table-cell"));
                var number = new XElement("td", new XElement("code", color.Name.ToLower()));
                var rgb = new XElement("td", new XElement("code", $"{color.R},{color.G},{color.B}"), new XAttribute("class", "hidden md:table-cell"));
                var hex = new XElement("td", new XElement("code", color.Hex));
                var clr = new XElement("td", new XElement("code", color.ClrName));

                // Create row
                var row = new XElement("tr", new XAttribute("class", "search-row"));
                row.Add(rep);
                row.Add(name);
                row.Add(number);
                row.Add(rgb);
                row.Add(hex);
                row.Add(clr);
                tBody.Add(row);
            }

            table.Add(tBody);
            return table.ToString();
        }
    }
}