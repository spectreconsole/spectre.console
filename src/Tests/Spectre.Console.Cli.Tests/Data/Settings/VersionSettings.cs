namespace Spectre.Console.Tests.Data;

public sealed class VersionSettings : CommandSettings
{
    [CommandOption("-v|--version")]
    public string Version { get; set; }
}