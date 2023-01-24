namespace Spectre.Console.Tests.Data;

[Description("The horse command.")]
public class HorseCommand : AnimalCommand<HorseSettings>
{
    public override int Execute(CommandContext context, HorseSettings settings)
    {
        DumpSettings(context, settings);
        return 0;
    }
}
