using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Generator.Models;
using Scriban;
using Scriban.Runtime;
using Spectre.Console.Cli;
using Spectre.IO;
using Path = Spectre.IO.Path;
using SpectreEnvironment = Spectre.IO.Environment;

namespace Generator.Commands
{
    public sealed class EmojiGeneratorCommand : AsyncCommand<EmojiGeneratorCommand.Settings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly IEnvironment _environment;
        private readonly IHtmlParser _parser;

        private readonly Dictionary<string, string> _templates = new Dictionary<string, string>
        {
            { "Templates/Emoji.Generated.template", "Emoji.Generated.cs" },
            { "Templates/Emoji.Json.template", "emojis.json" }, // For documentation
        };

        public sealed class Settings : GeneratorSettings
        {
            [CommandOption("-i|--input <PATH>")]
            public string Input { get; set; }
        }

        public EmojiGeneratorCommand()
        {
            _fileSystem = new FileSystem();
            _environment = new SpectreEnvironment();
            _parser = new HtmlParser();
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var output = new DirectoryPath(settings.Output);
            if (!_fileSystem.Directory.Exists(settings.Output))
            {
                _fileSystem.Directory.Create(settings.Output);
            }

            var stream = await FetchEmojis(settings);
            var document = await _parser.ParseDocumentAsync(stream);
            var emojis = Emoji.Parse(document).OrderBy(x => x.Name)
                .Where(emoji => !emoji.HasCombinators)
                .ToList();

            // Render all templates
            foreach (var (templateFilename, outputFilename) in _templates)
            {
                var result = await RenderTemplate(new FilePath(templateFilename), emojis);

                var outputPath = output.CombineWithFilePath(outputFilename);
                await File.WriteAllTextAsync(outputPath.FullPath, result);
            }

            return 0;
        }

        private async Task<Stream> FetchEmojis(Settings settings)
        {
            var input = string.IsNullOrEmpty(settings.Input)
                ? _environment.WorkingDirectory
                : new DirectoryPath(settings.Input);

            var file = _fileSystem.File.Retrieve(input.CombineWithFilePath("emoji-list.html"));
            if (!file.Exists)
            {
                using var http = new HttpClient();
                using var httpStream = await http.GetStreamAsync("http://www.unicode.org/emoji/charts/emoji-list.html");
                using var outStream = file.OpenWrite();

                await httpStream.CopyToAsync(outStream);
            }

            return file.OpenRead();
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
