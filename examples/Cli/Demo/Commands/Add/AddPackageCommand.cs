using System.ComponentModel;
using Demo.Utilities;
using Spectre.Console.Cli;

namespace Demo.Commands.Add;

[Description("Add a NuGet package reference to the project.")]
public sealed class AddPackageCommand : Command<AddPackageCommand.Settings>
{
    public sealed class Settings : AddSettings
    {
        [CommandArgument(0, "<PACKAGENAME>")]
        [Description("The package reference to add.")]
        public string PackageName { get; set; }

        [CommandOption("-v|--version <VERSION>")]
        [Description("The version of the package to add.")]
        public string Version { get; set; }

        [CommandOption("-f|--framework <FRAMEWORK>")]
        [Description("Add the reference only when targeting a specific framework.")]
        public string Framework { get; set; }

        [CommandOption("--no-restore")]
        [Description("Add the reference without performing restore preview and compatibility check.")]
        public bool NoRestore { get; set; }

        [CommandOption("--source <SOURCE>")]
        [Description("The NuGet package source to use during the restore.")]
        public string Source { get; set; }

        [CommandOption("--package-directory <PACKAGEDIR>")]
        [Description("The directory to restore packages to.")]
        public string PackageDirectory { get; set; }

        [CommandOption("--interactive")]
        [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
        public bool Interactive { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        SettingsDumper.Dump(settings);
        return 0;
    }
}
