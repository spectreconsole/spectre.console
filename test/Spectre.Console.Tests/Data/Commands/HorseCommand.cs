using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    [Description("The horse command.")]
    public class HorseCommand : AnimalCommand<MammalSettings>
    {
        public override int Execute(CommandContext context, MammalSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}
