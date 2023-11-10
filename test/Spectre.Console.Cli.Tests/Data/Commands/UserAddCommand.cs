namespace Spectre.Console.Cli.Tests.Data.Commands;

[Description("The user command.")]
internal class UserAddCommand : Command<UserAddSettings>
{
    public override int Execute(CommandContext context, UserAddSettings settings)
    {
        return 0;
    }
}