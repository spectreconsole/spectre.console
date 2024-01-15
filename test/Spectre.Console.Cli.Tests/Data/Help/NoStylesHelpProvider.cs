namespace Spectre.Console.Cli.Tests.Data.Help;

internal class NoStylesHelpProvider : HelpProvider
{
    protected override bool RenderMarkupInline { get; } = true;

    public NoStylesHelpProvider(ICommandAppSettings settings)
        : base(settings)
    {
    }
}
