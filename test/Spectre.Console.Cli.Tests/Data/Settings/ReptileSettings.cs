namespace Spectre.Console.Tests.Data;

public class ReptileSettings : AnimalSettings
{
    [CommandOption("-n|-p|--name|--pet-name <VALUE>")]
    public string Name { get; set; }
}