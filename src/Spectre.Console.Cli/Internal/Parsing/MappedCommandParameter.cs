namespace Spectre.Console.Cli;

// Consider removing this in favor for value tuples at some point.
[DebuggerDisplay("{DebuggerDisplay,nq}")]
internal sealed class MappedCommandParameter
{
    public CommandParameter Parameter { get; }
    public string? Value { get; }

    public MappedCommandParameter(CommandParameter parameter, string? value)
    {
        Parameter = parameter;
        Value = value;
    }

    [DebuggerHidden]
    private string DebuggerDisplay
    {
        get
        {
            var value = Parameter switch
            {
                CommandOption option => option.GetOptionName(),
                CommandArgument argument => argument.Value,
                _ => Parameter.ToString(),
            };

            return $"CommandParameter {value}: {Value}";
        }
    }
}