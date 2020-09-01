using System;
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
        private const string ColorStyle = "display: inline-block;width: 60px; height: 15px;";

        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            // Get the definition.
            var colors = context.Outputs
                .FromPipeline(nameof(ColorsPipeline))
                .First()
                .GetChildren(Constants.Colors.Root)
                .OfType<ObjectDocument<List<Color>>>()
                .First().Object;

            var table = new XElement("table", new XAttribute("class", "table"));
            var header = new XElement("tr", new XAttribute("class", "color-row"));
            header.Add(new XElement("th", ""));
            header.Add(new XElement("th", "#"));
            header.Add(new XElement("th", "Name"));
            header.Add(new XElement("th", "RGB"));
            header.Add(new XElement("th", "Hex"));
            header.Add(new XElement("th", "System.ConsoleColor"));
            table.Add(header);

            foreach (var color in colors)
            {
                var rep = new XElement("td", 
                    new XElement("span", 
                    new XAttribute("class", "color-representation"),
                    new XAttribute("style", $"background-color:{color.Hex};")));
                var name = new XElement("td", new XElement("code", color.Number.ToString()));
                var number = new XElement("td", new XElement("code", color.Name.ToLower()));
                var rgb = new XElement("td", new XElement("code", $"{color.R},{color.G},{color.B}"));
                var hex = new XElement("td", new XElement("code", color.Hex));
                var clr = new XElement("td", new XElement("code", color.ClrName));

                // Create row
                var row = new XElement("tr");
                row.Add(rep);
                row.Add(name);
                row.Add(number);
                row.Add(rgb);
                row.Add(hex);
                row.Add(clr);
                table.Add(row);
            }

            return table.ToString();
        }
    }
}