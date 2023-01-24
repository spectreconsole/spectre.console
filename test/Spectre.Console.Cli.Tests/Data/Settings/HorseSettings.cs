namespace Spectre.Console.Tests.Data;

public class HorseSettings : MammalSettings
{
    [CommandOption("-d|--day")]
    public DayOfWeek Day { get; set; }
}