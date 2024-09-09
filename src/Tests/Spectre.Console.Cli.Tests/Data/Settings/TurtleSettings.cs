namespace Spectre.Console.Tests.Data;

public sealed class TurtleSettings : ReptileSettings
{
    public TurtleSettings(string name)
    {
        Name = name;
    }

    public override ValidationResult Validate()
    {
        return Name != "Lonely George"
            ? ValidationResult.Error("Only 'Lonely George' is valid name for a turtle!")
            : ValidationResult.Success();
    }
}
