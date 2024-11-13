namespace Spectre.Console.Tests.Data;

public class CatCommand : AnimalCommand<CatSettings>
{
    public override int Execute(CommandContext context, CatSettings settings)
    {
        return 0;
    }
}
