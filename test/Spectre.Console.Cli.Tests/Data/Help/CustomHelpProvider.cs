using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli.Tests.Data.Help;

internal class CustomHelpProvider : DefaultHelpProvider
{
    private readonly string version;

    public CustomHelpProvider(ICommandAppSettings settings, string version)
        : base(settings)
    {
        this.version = version;
    }

    public override IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo command)
    {
        var header = new Composer();
        header.Text("--------------------------------------").LineBreak();
        header.Text("---      CUSTOM HELP PROVIDER      ---").LineBreak();
        header.Text("--------------------------------------").LineBreak();
        header.LineBreak();

        return new[] { header };
    }

    public override IEnumerable<IRenderable> GetFooter(ICommandModel model, ICommandInfo command)
    {
        var footer = new Composer();
        footer.LineBreak().Text($"Version {version}").LineBreak();

        return new[] { footer };
    }
}
