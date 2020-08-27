using System.Collections.Generic;
using Statiq.Common;
using System.Xml.Linq;

namespace Docs.Shortcodes
{
    public class ChildrenShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            var ul = new XElement("ul", new XAttribute("class", "list-group"));

            foreach (var child in document.GetChildren().OnlyVisible())
            {
                var li = new XElement("li", new XAttribute("class", "list-group-item"));

                var link = new XElement("a", new XAttribute("href", child.GetLink()));
                link.Add(child.GetTitle());
                li.Add(link);

                var description = child.GetDescription();
                if (description.IsNotEmpty())
                {
                    li.Add(new XElement("br"));
                    li.Add(new XElement("i", description));
                }

                ul.Add(li);
            }

            return ul.ToString();
        }
    }
}