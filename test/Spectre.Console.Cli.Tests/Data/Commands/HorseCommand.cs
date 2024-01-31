namespace Spectre.Console.Tests.Data;

[Description("The horse command.")]
public class HorseCommand : AnimalCommand<HorseSettings>
{
    public override int Execute(CommandContext context, HorseSettings settings)
    {
        return 0;
    }
}
