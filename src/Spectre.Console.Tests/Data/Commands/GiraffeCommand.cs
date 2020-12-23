using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    [Description("The giraffe command.")]
    public sealed class GiraffeCommand : Command<GiraffeSettings>
    {
        public override int Execute(CommandContext context, GiraffeSettings settings)
        {
            return 0;
        }
    }
}
