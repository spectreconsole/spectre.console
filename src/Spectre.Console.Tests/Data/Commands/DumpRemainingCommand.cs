using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class DumpRemainingCommand : Command<EmptyCommandSettings>
    {
        private readonly IAnsiConsole _console;

        public DumpRemainingCommand(IAnsiConsole console)
        {
            _console = console;
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] EmptyCommandSettings settings)
        {
            if (context.Remaining.Raw.Count > 0)
            {
                _console.WriteLine("# Raw");
                foreach (var item in context.Remaining.Raw)
                {
                    _console.WriteLine(item);
                }
            }

            if (context.Remaining.Parsed.Count > 0)
            {
                _console.WriteLine("# Parsed");
                foreach (var item in context.Remaining.Parsed)
                {
                    _console.WriteLine(string.Format("{0}={1}", item.Key, string.Join(",", item.Select(x => x))));
                }
            }

            return 0;
        }
    }
}
