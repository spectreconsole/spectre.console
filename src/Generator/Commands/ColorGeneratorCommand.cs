using System.IO;
using System.Linq;
using System.Threading;
using Generator.Models;
using Scriban;
using Spectre.Console.Cli;
using Spectre.IO;

namespace Generator.Commands;

public sealed class ColorGeneratorCommand : Command<ColorGeneratorCommand.Settings>
{
    private readonly IFileSystem _fileSystem;

    public ColorGeneratorCommand()
    {
        _fileSystem = new FileSystem();
    }

    public sealed class Settings : GeneratorSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var templates = new FilePath[]
        {
            "Templates/ColorPalette.Generated.template", "Templates/Color.Generated.template",
            "Templates/ColorTable.Generated.template"
        };

        // Read the color model.
        var model = Color
            .Parse(File.ReadAllText("Data/colors.json"))
            .OrderBy(x => x.Number)
            .ToArray();

        // Palettes
        var legacyPalette = model.Where(x => x.Number is >= 0 and < 8 && !x.IsAlias)
            .OrderBy(x => x.Number).ToArray();
        var standardPalette = model.Where(x => x.Number is >= 8 and < 16 && !x.IsAlias)
            .OrderBy(x => x.Number).ToArray();
        var eightBitPalette = model.Where(x => x.Number is >= 16 and < 256 && !x.IsAlias)
            .OrderBy(x => x.Number).ToArray();

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
            var result = template.Render(new
            {
                Colors = model,
                Legacy = legacyPalette,
                Standard = standardPalette,
                EightBit = eightBitPalette,
            });

            // Write output to file
            var file = output.CombineWithFilePath(templatePath.GetFilename().ChangeExtension(".cs"));
            File.WriteAllText(file.FullPath, result);
        }

        return 0;
    }
}