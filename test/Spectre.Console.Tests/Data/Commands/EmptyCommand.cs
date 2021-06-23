using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class EmptyCommand : Command<EmptyCommandSettings>
    {
        public override int Execute(CommandContext context, EmptyCommandSettings settings)
        {
            return 0;
        }
    }
}
