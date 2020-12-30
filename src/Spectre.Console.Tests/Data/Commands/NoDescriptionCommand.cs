using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class NoDescriptionCommand : Command<EmptyCommandSettings>
    {
        [CommandOption("-f|--foo <VALUE>")]
        public int Foo { get; set; }

        public override int Execute([NotNull] CommandContext context, [NotNull] EmptyCommandSettings settings)
        {
            return 0;
        }
    }
}
