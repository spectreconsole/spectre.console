using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DemoAot.Utilities;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DemoAot.Commands.Add;

public sealed class AddReferenceCommand : Command<AddReferenceCommand.Settings>
{
    public sealed class Settings : AddSettings
    {
        [CommandArgument(0, "<PROJECTPATH>")]
        [Description("The package reference to add.")]
        public DirectoryInfo ProjectPath { get; set; }

        [CommandOption("-f|--framework <FRAMEWORK>")]
        [Description("Add the reference only when targeting a specific framework.")]
        public string Framework { get; set; }

        [CommandOption("--interactive")]
        [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
        public bool Interactive { get; set; }
    }

    // In non-AOT scenarios, we can dynamically call the constructor to DirectoryInfo via reflection.
    // With trimming enabled we need to be explicit about requiring that constructor.
    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(DirectoryInfo))]
    public override int Execute(CommandContext context, Settings settings)
    {
        SettingsDumper.Dump(settings);
        return 0;
    }
}
