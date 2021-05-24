using System.Collections.Generic;
using System.IO;
using Generator.Models;
using Scriban;
using Spectre.Console.Cli;
using Spectre.IO;

namespace Generator.Commands
{
    public sealed class SpinnerGeneratorCommand : Command<GeneratorSettings>
    {
        private readonly IFileSystem _fileSystem;

        public SpinnerGeneratorCommand()
        {
            _fileSystem = new FileSystem();
        }

        public override int Execute(CommandContext context, GeneratorSettings settings)
        {
            // Read the spinner model.
            var spinners = new List<Spinner>();
            spinners.AddRange(Spinner.Parse(File.ReadAllText("Data/spinners_default.json")));
            spinners.AddRange(Spinner.Parse(File.ReadAllText("Data/spinners_sindresorhus.json")));

            var output = new DirectoryPath(settings.Output);
            if (!_fileSystem.Directory.Exists(settings.Output))
            {
                _fileSystem.Directory.Create(settings.Output);
            }

            // Parse the Scriban template.
            var templatePath = new FilePath("Templates/Spinner.Generated.template");
            var template = Template.Parse(File.ReadAllText(templatePath.FullPath));

            // Render the template with the model.
            var result = template.Render(new { Spinners = spinners });

            // Write output to file
            var file = output.CombineWithFilePath(templatePath.GetFilename().ChangeExtension(".cs"));
            File.WriteAllText(file.FullPath, result);

            return 0;
        }
    }
}
