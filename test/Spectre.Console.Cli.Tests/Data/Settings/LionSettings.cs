namespace Spectre.Console.Tests.Data;

public class LionSettings : CatSettings
{
    [CommandArgument(0, "<TEETH>")]
    [Description("The number of teeth the lion has.")]
    public int Teeth { get; set; }

    [CommandOption("-c <CHILDREN>")]
    [Description("The number of children the lion has.")]
    public int Children { get; set; }

    [CommandOption("-d <DAY>")]
    [Description("The days the lion goes hunting.")]
    [DefaultValue(new[] { DayOfWeek.Monday, DayOfWeek.Thursday })]
    public DayOfWeek[] HuntDays { get; set; }
}
