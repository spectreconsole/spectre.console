namespace Spectre.Console.Cli;

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

    public static void ValidateParameter(CommandParameter parameter, CommandValueLookup settings, ITypeResolver resolver)
    {
        var assignedValue = settings.GetValue(parameter);
        foreach (var validator in parameter.Validators)
        {
            var context = new CommandParameterContext(parameter, resolver, assignedValue);
            var validationResult = validator.Validate(context);
            if (!validationResult.Successful)
            {
                // If there is an error message specified in the parameter validator attribute,
                // then use that one, otherwise use the validation result.
                var result = string.IsNullOrWhiteSpace(validator.ErrorMessage)
                    ? validationResult
                    : ValidationResult.Error(validator.ErrorMessage);

                throw CommandRuntimeException.ValidationFailed(result);
            }
        }
    }
}