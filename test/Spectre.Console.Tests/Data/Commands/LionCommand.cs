using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    [Description("The lion command.")]
    public class LionCommand : AnimalCommand<LionSettings>
    {
        public override int Execute(CommandContext context, LionSettings settings)
        {
            return 0;
        }
    }
}