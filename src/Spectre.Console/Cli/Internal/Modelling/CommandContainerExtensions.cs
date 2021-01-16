using System.Linq;

namespace Spectre.Console.Cli
{
    internal static class CommandContainerExtensions
    {
        public static CommandInfo? FindCommand(this ICommandContainer root, string name, CaseSensitivity sensitivity)
        {
            var result = root.Commands.FirstOrDefault(
                c => c.Name.Equals(name, sensitivity.GetStringComparison(CommandPart.CommandName)));

            if (result == null)
            {
                result = root.Commands.FirstOrDefault(
                    c => c.Aliases.Contains(name, sensitivity.GetStringComparer(CommandPart.CommandName)));
            }

            return result;
        }
    }
}
