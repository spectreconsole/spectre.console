namespace Spectre.Console.Cli;

/// <summary>
/// Represents errors that occur during runtime.
/// </summary>
public class CommandRuntimeException : CommandAppException
{
    internal CommandRuntimeException(string message, IRenderable? pretty = null)
        : base(message, pretty)
    {
    }

    internal CommandRuntimeException(string message, Exception ex, IRenderable? pretty = null)
        : base(message, ex, pretty)
    {
    }

    internal static CommandRuntimeException CouldNotResolveType(Type type, Exception? ex = null)
    {
        var message = $"Could not resolve type '{type.FullName}'.";
        if (ex != null)
        {
            // TODO: Show internal stuff here.
            return new CommandRuntimeException(message, ex);
        }

        return new CommandRuntimeException(message);
    }

    internal static CommandRuntimeException MissingRequiredArgument(CommandTree node, CommandArgument argument)
    {
        if (node.Command.Name == CliConstants.DefaultCommandName)
        {
            return new CommandRuntimeException($"Missing required argument '{argument.Value}'.");
        }

        return new CommandRuntimeException($"Command '{node.Command.Name}' is missing required argument '{argument.Value}'.");
    }

    internal static CommandRuntimeException NoConverterFound(CommandParameter parameter)
    {
        return new CommandRuntimeException($"Could not find converter for type '{parameter.ParameterType.FullName}'.");
    }

    internal static CommandRuntimeException ConversionFailed(MappedCommandParameter parameter, TypeConverter typeConverter, Exception exception)
    {
        var standardValues = typeConverter.GetStandardValuesSupported() ? typeConverter.GetStandardValues() : null;
        var validValues = standardValues == null ? string.Empty : $" Valid values are '{string.Join("', '", standardValues.Cast<object>().Select(Convert.ToString))}'";
        return new CommandRuntimeException($"Failed to convert '{parameter.Value}' to {parameter.Parameter.ParameterType.Name}.{validValues}", exception);
    }

    internal static CommandRuntimeException ValidationFailed(ValidationResult result)
    {
        return new CommandRuntimeException(result.Message ?? "Unknown validation error.");
    }

    internal static Exception CouldNotGetSettingsType(Type commandType)
    {
        return new CommandRuntimeException($"Could not get settings type for command of type '{commandType.FullName}'.");
    }
}