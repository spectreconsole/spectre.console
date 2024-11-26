using System.ComponentModel;
using Spectre.Console.Cli;
using Spectre.Console.TrimTest.Utilities;

namespace Spectre.Console.TrimTest.Commands.Add;

public sealed class AddReferenceCommand : Command<AddReferenceCommand.Settings>
{
    public sealed class Settings : AddSettings
    {
        [CommandArgument(0, "<PROJECTPATH>")]
        [Description("The package reference to add.")]
        public string ProjectPath { get; set; }

        [CommandOption("-f|--framework <FRAMEWORK>")]
        [Description("Add the reference only when targeting a specific framework.")]
        public string Framework { get; set; }

        [CommandOption("--interactive")]
        [Description("Allows the command to stop and wait for user input or action (for example to complete authentication).")]
        public bool Interactive { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        SettingsDumper.Dump<Settings>(settings);
        return 0;
    }
}
