using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli.Tests.Data.Help;

internal class CustomHelpProvider : DefaultHelpProvider
{
    private readonly string version;

    public CustomHelpProvider()
        : this(true, 5, true, "1.0")
    {
    }

    public CustomHelpProvider(bool writeOptionsDefaultValue, int maxIndirectExamples, bool trimTrailingPeriod, string version)
        : base(writeOptionsDefaultValue, maxIndirectExamples, trimTrailingPeriod)
    {
        this.version = version;
    }

    public override IEnumerable<IRenderable> WriteCommand(ICommandModel model, ICommandInfo command)
    {
        var result = base.WriteCommand(model, command) as List<IRenderable>;

        var header = new Composer();
        header.Text("--------------------------------------").LineBreak();
        header.Text("---      CUSTOM HELP PROVIDER      ---").LineBreak();
        header.Text("--------------------------------------").LineBreak();
        result.Insert(0, header.LineBreak());

        var footer = new Composer();
        footer.LineBreak().Text($"Version {version}").LineBreak();
        result.Add(footer);

        return result;
    }
}
