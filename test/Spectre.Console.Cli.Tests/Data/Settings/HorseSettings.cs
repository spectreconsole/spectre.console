using System.IO;

namespace Spectre.Console.Tests.Data;

public class HorseSettings : MammalSettings
{
    [CommandOption("-d|--day")]
    public DayOfWeek Day { get; set; }

    [CommandOption("--file")]
    [DefaultValue("food.txt")]
    public FileInfo File { get; set; }

    [CommandOption("--directory")]
    public DirectoryInfo Directory { get; set; }
}