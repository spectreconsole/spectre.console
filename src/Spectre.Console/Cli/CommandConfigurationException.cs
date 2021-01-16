using System;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    /// <summary>
    /// Represents errors that occur during configuration.
    /// </summary>
    public class CommandConfigurationException : CommandAppException
    {
        internal override bool AlwaysPropagateWhenDebugging => true;

        internal CommandConfigurationException(string message, IRenderable? pretty = null)
            : base(message, pretty)
        {
        }

        internal CommandConfigurationException(string message, Exception ex, IRenderable? pretty = null)
            : base(message, ex, pretty)
        {
        }

        internal static CommandConfigurationException NoCommandConfigured()
        {
            return new CommandConfigurationException("No commands have been configured.");
        }

        internal static CommandConfigurationException CommandNameConflict(CommandInfo command, string alias)
        {
            return new CommandConfigurationException($"The alias '{alias}' for '{command.Name}' conflicts with another command.");
        }

        internal static CommandConfigurationException DuplicateOption(CommandInfo command, string[] options)
        {
            var keys = string.Join(", ", options.Select(x => x.Length > 1 ? $"--{x}" : $"-{x}"));
            if (options.Length > 1)
            {
                return new CommandConfigurationException($"Options {keys} are duplicated in command '{command.Name}'.");
            }

            return new CommandConfigurationException($"Option {keys} is duplicated in command '{command.Name}'.");
        }

        internal static CommandConfigurationException BranchHasNoChildren(CommandInfo command)
        {
            throw new CommandConfigurationException($"The branch '{command.Name}' does not define any commands.");
        }

        internal static CommandConfigurationException TooManyVectorArguments(CommandInfo command)
        {
            throw new CommandConfigurationException($"The command '{command.Name}' specifies more than one vector argument.");
        }

        internal static CommandConfigurationException VectorArgumentNotSpecifiedLast(CommandInfo command)
        {
            throw new CommandConfigurationException($"The command '{command.Name}' specifies an argument vector that is not the last argument.");
        }

        internal static CommandConfigurationException OptionalOptionValueMustBeFlagWithValue(CommandOption option)
        {
            return new CommandConfigurationException($"The option '{option.GetOptionName()}' has an optional value but does not implement IFlagValue.");
        }

        internal static CommandConfigurationException OptionBothHasPairDeconstructorAndTypeParameter(CommandOption option)
        {
            return new CommandConfigurationException($"The option '{option.GetOptionName()}' is both marked as pair deconstructable and convertable.");
        }

        internal static CommandConfigurationException OptionTypeDoesNotSupportDeconstruction(CommandOption option)
        {
            return new CommandConfigurationException($"The option '{option.GetOptionName()}' is marked as " +
                "pair deconstructable, but the underlying type does not support that.");
        }

        internal static CommandConfigurationException RequiredArgumentsCannotHaveDefaultValue(CommandArgument option)
        {
            return new CommandConfigurationException($"The required argument '{option.Value}' cannot have a default value.");
        }
    }
}
