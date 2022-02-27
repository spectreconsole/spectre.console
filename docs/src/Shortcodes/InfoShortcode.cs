using System.Collections.Generic;
using Statiq.Common;

namespace Docs.Shortcodes
{
    public class InfoShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            return $"<div class=\"alert-info\">{content}</div>";
        }
    }
}