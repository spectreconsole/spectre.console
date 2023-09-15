using System.ComponentModel;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Completion;

namespace AutoCompletion;

public class LionSettings : CommandSettings
{
    [CommandArgument(0, "<TEETH>")]
    [Description("The number of teeth the lion has.")]
    public int Teeth { get; set; }

    [CommandArgument(1, "[LEGS]")]
    [Description("The number of legs.")]
    public int Legs { get; set; }

    [CommandOption("-c <CHILDREN>")]
    [Description("The number of children the lion has.")]
    public int Children { get; set; }

    [CommandOption("-d <DAY>")]
    [Description("The days the lion goes hunting.")]
    [DefaultValue(new[] { DayOfWeek.Monday, DayOfWeek.Thursday })]
    public DayOfWeek[] HuntDays { get; set; }

    [CommandOption("-n|-p|--name|--pet-name <VALUE>")]
    public string Name { get; set; }

    [CompletionSuggestions("10", "15", "20", "30")]
    public int Age { get; set; }
}
