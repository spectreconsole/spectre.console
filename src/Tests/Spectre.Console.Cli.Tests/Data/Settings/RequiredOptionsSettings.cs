namespace Spectre.Console.Tests.Data;

public class RequiredOptionsSettings : CommandSettings
{
    [CommandOption("--foo <VALUE>", true)]
    public string Foo { get; set; }
}