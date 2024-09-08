namespace Spectre.Console.Tests.Data;

public sealed class HiddenOptionsCommand : Command<HiddenOptionSettings>
{
    public override int Execute(CommandContext context, HiddenOptionSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}
