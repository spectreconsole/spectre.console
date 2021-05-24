using System.IO;
using Generator.Models;
using Scriban;
using Spectre.Console.Cli;
using Spectre.IO;

namespace Generator.Commands
{
    public sealed class ColorGeneratorCommand : Command<ColorGeneratorCommand.Settings>
    {
        private readonly IFileSystem _fileSystem;

        public ColorGeneratorCommand()
        {
            _fileSystem = new FileSystem();
        }

        public sealed class Settings : GeneratorSettings
        {
            [CommandOption("-i|--input <PATH>")]
            public string Input { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var templates = new FilePath[]
            {
                "Templates/ColorPalette.Generated.template",
                "Templates/Color.Generated.template",
                "Templates/ColorTable.Generated.template"
            };

            // Read the color model.
            var model = Color.Parse(File.ReadAllText("Data/colors.json"));

            var output = new DirectoryPath(settings.Output);
            if (!_fileSystem.Directory.Exists(settings.Output))
            {
                _fileSystem.Directory.Create(settings.Output);
            }

            foreach (var templatePath in templates)
            {
                // Parse the Scriban template.
                var template = Template.Parse(File.ReadAllText(templatePath.FullPath));

                // Render the template with the model.
                var result = template.Render(new { Colors = model });

                // Write output to file
                var file = output.CombineWithFilePath(templatePath.GetFilename().ChangeExtension(".cs"));
                File.WriteAllText(file.FullPath, result);
            }

            return 0;
        }
    }
}
