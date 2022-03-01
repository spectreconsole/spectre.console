using System.ComponentModel;
using Spectre.Console.Cli;

namespace Demo.Commands.Add;

public abstract class AddSettings : CommandSettings
{
    [CommandArgument(0, "<PROJECT>")]
    [Description("The project file to operate on. If a file is not specified, the command will search the current directory for one.")]
    public string Project { get; set; }
}
