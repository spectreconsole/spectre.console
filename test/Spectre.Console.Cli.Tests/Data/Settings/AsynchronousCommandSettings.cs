namespace Spectre.Console.Tests.Data;

public sealed class AsynchronousCommandSettings : CommandSettings
{
    [CommandOption("--ThrowException")]
    [DefaultValue(false)]
    public bool ThrowException { get; set; }
}