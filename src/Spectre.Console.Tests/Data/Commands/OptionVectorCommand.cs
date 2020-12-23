using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class OptionVectorCommand : Command<OptionVectorSettings>
    {
        public override int Execute(CommandContext context, OptionVectorSettings settings)
        {
            return 0;
        }
    }
}