using System.Collections.Generic;
using Docs.Models;
using Docs.Modules;
using Statiq.Common;
using Statiq.Core;

namespace Docs.Pipelines
{
    public class EmojiPipeline : Pipeline
    {
        public EmojiPipeline()
        {
            InputModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromContext(ctx => {
                        return new ReadEmbedded(
                            typeof(EmojiPipeline).Assembly,
                            "Docs/src/Data/emojis.json");
                    }))
            };

            ProcessModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromDocument(async (doc, ctx) =>
                    {
                        var data = Emoji.Parse(await doc.GetContentStringAsync());
                        return data.ToDocument(Constants.Emojis.Root);
                    }))
            };
        }
    }
}