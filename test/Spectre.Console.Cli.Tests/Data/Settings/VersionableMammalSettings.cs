namespace Spectre.Console.Tests.Data;

public sealed class VersionableMammalSettings : MammalSettings
{
    [CommandOption("-v|--version")]
    public string Version { get; set; }

    public override ValidationResult Validate()
    {
        return ValidationResult.Success();
    }
}
