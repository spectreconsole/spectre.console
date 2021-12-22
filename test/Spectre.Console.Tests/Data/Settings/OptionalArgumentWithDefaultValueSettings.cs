namespace Spectre.Console.Tests.Data;

public sealed class OptionalArgumentWithDefaultValueSettings : CommandSettings
{
    [CommandArgument(0, "[GREETING]")]
    [DefaultValue("Hello World")]
    public string Greeting { get; set; }
}

public sealed class OptionalArgumentWithPropertyInitializerSettings : CommandSettings
{
    [CommandArgument(0, "[NAMES]")]
    public string[] Names { get; set; } = Array.Empty<string>();

    [CommandOption("-c")]
    public int Count { get; set; } = 1;

    [CommandOption("-v")]
    public int Value { get; set; } = 0;
}

public sealed class OptionalArgumentWithDefaultValueAndTypeConverterSettings : CommandSettings
{
    [CommandArgument(0, "[GREETING]")]
    [DefaultValue("5")]
    [TypeConverter(typeof(StringToIntegerConverter))]
    public int Greeting { get; set; }
}

public sealed class RequiredArgumentWithDefaultValueSettings : CommandSettings
{
    [CommandArgument(0, "<GREETING>")]
    [DefaultValue("Hello World")]
    public string Greeting { get; set; }
}
