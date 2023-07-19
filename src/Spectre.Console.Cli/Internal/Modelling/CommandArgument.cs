namespace Spectre.Console.Cli;

[DebuggerDisplay("Command {Value}")]
internal sealed class CommandArgument : CommandParameter
{
    public string Value { get; }
    public int Position { get; set; }

    public CommandArgument(
        Type parameterType, ParameterKind parameterKind, PropertyInfo property, string? description,
        TypeConverterAttribute? converter, DefaultValueAttribute? defaultValue,
        CommandArgumentAttribute argument, ParameterValueProviderAttribute? valueProvider,
        IEnumerable<ParameterValidationAttribute> validators)
            : base(parameterType, parameterKind, property, description, converter, defaultValue,
                  null, valueProvider, validators, argument.IsRequired, false)
    {
        // Teeth: 0
        // Legs: 1
        Value = argument.ValueName;
        Position = argument.Position;
    }
}