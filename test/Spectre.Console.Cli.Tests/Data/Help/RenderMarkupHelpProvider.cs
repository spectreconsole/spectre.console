namespace Spectre.Console.Cli.Tests.Data.Help;

internal class RenderMarkupHelpProvider : HelpProvider
{
    protected override bool RenderMarkupInline { get; } = true;

    public RenderMarkupHelpProvider(ICommandAppSettings settings)
        : base(settings)
    {
    }
}
