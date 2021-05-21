using System.Collections.Generic;
using Statiq.Common;
using System.Linq;

namespace Docs.Shortcodes
{
    public class AsciiCastShortcode : SyncShortcode
    {
        public override ShortcodeResult Execute(KeyValuePair<string, string>[] args, string content, IDocument document, IExecutionContext context)
        {
            // in the future I'd like to expand this to have two tabs, one with the full color unicode version
            // and a second with the default plain.
            var cast = args.First(i => i.Key == "cast").Value;
            var profile = args.FirstOrDefault(i => i.Key == "profile").Value ?? "rich";

            return $"<asciinema-player src=\"/assets/casts/{cast}-{profile}.cast\" autoplay></asciinema-player>";
        }
    }
}