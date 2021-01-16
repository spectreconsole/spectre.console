using System.Linq;

namespace Spectre.Console.Cli
{
    internal static class CommandInfoExtensions
    {
        public static bool HaveParentWithOption(this CommandInfo command, CommandOption option)
        {
            var parent = command?.Parent;
            while (parent != null)
            {
                foreach (var parentOption in parent.Parameters.OfType<CommandOption>())
                {
                    if (option.HaveSameBackingPropertyAs(parentOption))
                    {
                        return true;
                    }
                }

                parent = parent.Parent;
            }

            return false;
        }

        public static bool AllowParentOption(this CommandInfo command, CommandOption option)
        {
            // Got an immediate parent?
            if (command?.Parent != null)
            {
                // Is the current node's settings type the same as the previous one?
                if (command.SettingsType == command.Parent.SettingsType)
                {
                    var parameters = command.Parent.Parameters.OfType<CommandOption>().ToArray();
                    if (parameters.Length > 0)
                    {
                        foreach (var parentOption in parameters)
                        {
                            // Is this the same one?
                            if (option.HaveSameBackingPropertyAs(parentOption))
                            {
                                // Is it part of the same settings class.
                                if (option.Property.DeclaringType == command.SettingsType)
                                {
                                    // Allow it.
                                    return true;
                                }

                                // Don't allow it.
                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static bool HaveParentWithArgument(this CommandInfo command, CommandArgument argument)
        {
            var parent = command?.Parent;
            while (parent != null)
            {
                foreach (var parentOption in parent.Parameters.OfType<CommandArgument>())
                {
                    if (argument.HaveSameBackingPropertyAs(parentOption))
                    {
                        return true;
                    }
                }

                parent = parent.Parent;
            }

            return false;
        }
    }
}
