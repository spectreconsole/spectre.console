namespace Spectre.Console.Tests.Data;

public class RequiredOptionsSettings : CommandSettings
{
    [CommandOption("--foo <VALUE>", true)]
    [Description("Foos the bars")]
    public string Foo { get; set; }
}

public class RequiredOptionsWithoutDescriptionSettings : CommandSettings
{
    [CommandOption("--foo <VALUE>", true)]
    public string Foo { get; set; }
}