using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Cli
{
    internal static class CommandModelValidator
    {
        public static void Validate(CommandModel model, CommandAppSettings settings)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (model.Commands.Count == 0 && model.DefaultCommand == null)
            {
                throw CommandConfigurationException.NoCommandConfigured();
            }

            foreach (var command in model.Commands)
            {
                // Alias collision?
                foreach (var alias in command.Aliases)
                {
                    if (model.Commands.Any(x => x.Name.Equals(alias, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw CommandConfigurationException.CommandNameConflict(command, alias);
                    }
                }
            }

            Validate(model.DefaultCommand);
            foreach (var command in model.Commands)
            {
                Validate(command);
            }

            if (settings.ValidateExamples)
            {
                ValidateExamples(model, settings);
            }
        }

        private static void Validate(CommandInfo? command)
        {
            if (command == null)
            {
                return;
            }

            // Get duplicate options for command.
            var duplicateOptions = GetDuplicates(command);
            if (duplicateOptions.Length > 0)
            {
                throw CommandConfigurationException.DuplicateOption(command, duplicateOptions);
            }

            // No children?
            if (command.IsBranch && command.Children.Count == 0)
            {
                throw CommandConfigurationException.BranchHasNoChildren(command);
            }

            // Multiple vector arguments?
            var arguments = command.Parameters.OfType<CommandArgument>();
            if (arguments.Any(x => x.ParameterKind == ParameterKind.Vector))
            {
                // Multiple vector arguments for command?
                if (arguments.Count(x => x.ParameterKind == ParameterKind.Vector) > 1)
                {
                    throw CommandConfigurationException.TooManyVectorArguments(command);
                }

                // Make sure that vector arguments are specified last.
                if (arguments.Last().ParameterKind != ParameterKind.Vector)
                {
                    throw CommandConfigurationException.VectorArgumentNotSpecifiedLast(command);
                }
            }

            // Arguments
            var argumnets = command.Parameters.OfType<CommandArgument>();
            foreach (var argument in arguments)
            {
                if (argument.Required && argument.DefaultValue != null)
                {
                    throw CommandConfigurationException.RequiredArgumentsCannotHaveDefaultValue(argument);
                }
            }

            // Options
            var options = command.Parameters.OfType<CommandOption>();
            foreach (var option in options)
            {
                // Pair deconstructable?
                if (option.Property.PropertyType.IsPairDeconstructable())
                {
                    if (option.PairDeconstructor != null && option.Converter != null)
                    {
                        throw CommandConfigurationException.OptionBothHasPairDeconstructorAndTypeParameter(option);
                    }
                }
                else if (option.PairDeconstructor != null)
                {
                    throw CommandConfigurationException.OptionTypeDoesNotSupportDeconstruction(option);
                }

                // Optional options that are not flags?
                if (option.ParameterKind == ParameterKind.FlagWithValue && !option.IsFlagValue())
                {
                    throw CommandConfigurationException.OptionalOptionValueMustBeFlagWithValue(option);
                }
            }

            // Validate child commands.
            foreach (var childCommand in command.Children)
            {
                Validate(childCommand);
            }
        }

        private static void ValidateExamples(CommandModel model, CommandAppSettings settings)
        {
            var examples = new List<string[]>();
            examples.AddRangeIfNotNull(model.Examples);

            // Get all examples.
            var queue = new Queue<ICommandContainer>(new[] { model });
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var command in current.Commands)
                {
                    examples.AddRangeIfNotNull(command.Examples);
                    queue.Enqueue(command);
                }
            }

            // Validate all examples.
            foreach (var example in examples)
            {
                try
                {
                    var parser = new CommandTreeParser(model, settings, ParsingMode.Strict);
                    parser.Parse(example);
                }
                catch (Exception ex)
                {
                    throw new CommandConfigurationException("Validation of examples failed.", ex);
                }
            }
        }

        private static string[] GetDuplicates(CommandInfo command)
        {
            var result = new Dictionary<string, int>(StringComparer.Ordinal);

            void AddToResult(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        if (!result.ContainsKey(key))
                        {
                            result.Add(key, 0);
                        }

                        result[key]++;
                    }
                }
            }

            foreach (var option in command.Parameters.OfType<CommandOption>())
            {
                AddToResult(option.ShortNames);
                AddToResult(option.LongNames);
            }

            return result.Where(x => x.Value > 1)
                .Select(x => x.Key).ToArray();
        }
    }
}
