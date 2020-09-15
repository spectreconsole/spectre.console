using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Generator.Models;
using Scriban;
using Scriban.Runtime;
using Spectre.Cli;
using Spectre.IO;
using Path = Spectre.IO.Path;

namespace Generator.Commands
{
    public sealed class EmojiGeneratorCommand : AsyncCommand<GeneratorCommandSettings>
    {
        private readonly IFileSystem _fileSystem;

        private readonly IHtmlParser _parser;

        public EmojiGeneratorCommand()
        {
            _fileSystem = new FileSystem();
            _parser = new HtmlParser();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, GeneratorCommandSettings settings)
        {
            var output = new DirectoryPath(settings.Output);

            if (!_fileSystem.Directory.Exists(settings.Output))
            {
                _fileSystem.Directory.Create(settings.Output);
            }

            var templatePath = new FilePath("Templates/Emoji.Generated.template");

            var emojis = await FetchEmojis("http://www.unicode.org/emoji/charts/emoji-list.html");

            var result = await RenderTemplate(templatePath, emojis);

            var outputPath = output.CombineWithFilePath(templatePath.GetFilename().ChangeExtension(".cs"));

            await File.WriteAllTextAsync(outputPath.FullPath, result);

            return 0;
        }

        private async Task<IReadOnlyCollection<Emoji>> FetchEmojis(string url)
        {
            using var http = new HttpClient();

            var htmlStream = await http.GetStreamAsync(url);

            var document = await _parser.ParseDocumentAsync(htmlStream);

            return Emoji.Parse(document).OrderBy(x => x.Name).ToList();
        }

        private static async Task<string> RenderTemplate(Path path, IReadOnlyCollection<Emoji> emojis)
        {
            var text = await File.ReadAllTextAsync(path.FullPath);

            var template = Template.Parse(text);

            var templateContext = new TemplateContext
            {
                // Because of the insane amount of Emojis,
                // we need to get rid of some secure defaults :P
                LoopLimit = int.MaxValue,
            };

            var scriptObject = new ScriptObject();

            scriptObject.Import(new { Emojis = emojis });

            templateContext.PushGlobal(scriptObject);

            return await template.RenderAsync(templateContext);
        }
    }
}
