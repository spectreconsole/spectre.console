namespace Spectre.Console.Cli.Tests.Data.Commands;

[Description("The user command.")]
internal class UserSuperAddCommand : Command<UserSuperAddSettings>
{
    public override int Execute(CommandContext context, UserSuperAddSettings settings)
    {
        return 0;
    }
}