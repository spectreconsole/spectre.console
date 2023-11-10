namespace Spectre.Console.Cli.Internal.Configuration;

internal class AutoCompletionSettings : IAutoCompletionSettings
{
    public bool IsEnabled { get; set; } = true;

    public bool IsPowershellEnabled { get; set; } = true;
}
