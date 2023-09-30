namespace Spectre.Console.Tests.Data;

public sealed class ThrowingCommand : Command<ThrowingCommandSettings>
{
    public const string Message = "W00t?";

    public override int Execute(CommandContext context, ThrowingCommandSettings settings)
    {
        throw new InvalidOperationException(Message);
    }
}