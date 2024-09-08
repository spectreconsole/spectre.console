namespace Spectre.Console.Tests.Data;

public sealed class InvalidCommand : Command<InvalidSettings>
{
    public override int Execute(CommandContext context, InvalidSettings settings, CancellationToken cancellationToken)
    {
        return 0;
    }
}
