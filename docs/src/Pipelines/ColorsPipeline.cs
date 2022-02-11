using System.Linq;
using Docs.Models;
using Statiq.Common;
using Statiq.Core;

namespace Docs.Pipelines
{
    public class ColorsPipeline : Pipeline
    {
        public ColorsPipeline()
        {
            InputModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromContext(_ => {
                        return new ReadWeb(Constants.Colors.Url);
                    }))
            };

            ProcessModules = new ModuleList
            {
                new ExecuteConfig(
                    Config.FromDocument(async (doc, _) =>
                    {
                        var data = Color.Parse(await doc.GetContentStringAsync()).ToList();
                        return data.ToDocument(Constants.Colors.Root);
                    }))
            };
        }
    }
}
