namespace Spectre.Console.Cli.Internal
{
    internal static class CommandValidator
    {
        public static void ValidateRequiredParameters(CommandTree? tree)
        {
            var node = tree?.GetRootCommand();
            while (node != null)
            {
                foreach (var parameter in node.Unmapped)
                {
                    if (parameter.Required)
                    {
                        switch (parameter)
                        {
                            case CommandArgument argument:
                                throw CommandRuntimeException.MissingRequiredArgument(node, argument);
                        }
                    }
                }

                node = node.Next;
            }
        }

        public static void ValidateParameter(CommandParameter parameter, CommandValueLookup settings)
        {
            var assignedValue = settings.GetValue(parameter);
            foreach (var validator in parameter.Validators)
            {
                var validationResult = validator.Validate(parameter, assignedValue);
                if (!validationResult.Successful)
                {
                    // If there is a error message specified in the parameter validator attribute,
                    // then use that one, otherwise use the validation result.
                    var result = validator.ErrorMessage != null
                        ? ValidationResult.Error(validator.ErrorMessage)
                        : validationResult;

                    throw CommandRuntimeException.ValidationFailed(result);
                }
            }
        }
    }
}
