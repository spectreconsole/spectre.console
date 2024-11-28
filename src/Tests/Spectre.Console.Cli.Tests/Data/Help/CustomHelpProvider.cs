using Spectre.Console.Rendering;

namespace Spectre.Console.Cli.Tests.Data.Help;

internal class CustomHelpProvider : HelpProvider
{
    private readonly string version;

    public CustomHelpProvider(ICommandAppSettings settings, string version)
        : base(settings)
    {
        this.version = version;
    }

    public override IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo command)
    {
        return new IRenderable[]
        {
            new Text("--------------------------------------"), Text.NewLine,
            new Text("---      CUSTOM HELP PROVIDER      ---"), Text.NewLine,
            new Text("--------------------------------------"), Text.NewLine,
            Text.NewLine,
        };
    }

    public override IEnumerable<IRenderable> GetFooter(ICommandModel model, ICommandInfo command)
    {
        return new IRenderable[]
        {
            Text.NewLine,
            new Text($"Version {version}"),
        };
    }
}
