using System;
using System.Collections.Generic;
using System.Linq;
using Docs.Models;
using Statiq.Common;
using Statiq.Core;

namespace Docs.Pipelines
{
    public class ColorsPipeline : Pipeline
    {
        public const string Url = "https://raw.githubusercontent.com/spectresystems/spectre.console/main/resources/scripts/Generator/Data/colors.json";

        public ColorsPipeline()
        {
            InputModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromContext(ctx => {
                        return new ReadWeb(Url);
                    }))
            };

            ProcessModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromDocument(async (doc, ctx) =>
                    {
                        var colors = Color.Parse(await doc.GetContentStringAsync()).ToList(); 
                        var definitions = new List<IDocument> { colors.ToDocument(Constants.Colors.Root) };

                        return doc.Clone(new MetadataDictionary
                        {
                            [Constants.Colors.Root] = definitions
                        });
                    }))
            };
        }
    }
}