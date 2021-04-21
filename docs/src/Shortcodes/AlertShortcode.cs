using System.Collections.Generic;
using Statiq.Common;
using System.Xml.Linq;

namespace Docs.Shortcodes
{
    public class AlertShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {

            return $"<div class=\"alert-warning\">{content}</div>";
        }
    }
}