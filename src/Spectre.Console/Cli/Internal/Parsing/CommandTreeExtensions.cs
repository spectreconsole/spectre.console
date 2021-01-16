using System;
using System.Linq;

namespace Spectre.Console.Cli
{
    internal static class CommandTreeExtensions
    {
        public static CommandTree? GetRootCommand(this CommandTree node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            return node;
        }

        public static CommandTree GetLeafCommand(this CommandTree node)
        {
            while (node.Next != null)
            {
                node = node.Next;
            }

            return node;
        }

        public static bool HasArguments(this CommandTree tree)
        {
            return tree.Command.Parameters.OfType<CommandArgument>().Any();
        }

        public static CommandArgument? FindArgument(this CommandTree tree, int position)
        {
            return tree.Command.Parameters
                .OfType<CommandArgument>()
                .FirstOrDefault(c => c.Position == position);
        }

        public static CommandOption? FindOption(this CommandTree tree, string name, bool longOption, CaseSensitivity sensitivity)
        {
            return tree.Command.Parameters
                .OfType<CommandOption>()
                .FirstOrDefault(o => longOption
                    ? o.LongNames.Contains(name, sensitivity.GetStringComparer(CommandPart.LongOption))
                    : o.ShortNames.Contains(name, StringComparer.Ordinal));
        }

        public static bool IsOptionMappedWithParent(this CommandTree tree, string name, bool longOption)
        {
            var node = tree.Parent;
            while (node != null)
            {
                var option = node.Command?.Parameters.OfType<CommandOption>()
                    .FirstOrDefault(o => longOption
                        ? o.LongNames.Contains(name, StringComparer.Ordinal)
                        : o.ShortNames.Contains(name, StringComparer.Ordinal));

                if (option != null)
                {
                    return node.Mapped.Any(p => p.Parameter == option);
                }

                node = node.Parent;
            }

            return false;
        }
    }
}
