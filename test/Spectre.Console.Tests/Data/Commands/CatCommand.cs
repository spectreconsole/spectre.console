using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class CatCommand : AnimalCommand<CatSettings>
    {
        public override int Execute(CommandContext context, CatSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}
