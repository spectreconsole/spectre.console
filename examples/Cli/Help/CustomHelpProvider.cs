using System.Linq;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace Help;

/// <summary>
/// Example showing how to extend the built-in Spectre.Console help provider
/// by rendering a custom banner at the top of the help information
/// </summary>
internal class CustomHelpProvider : HelpProvider
{
    public CustomHelpProvider(ICommandAppSettings settings)
        : base(settings)
    {
    }

    public override IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo? command)
    {
        return new[]
        {
            new Text("--------------------------------------"), Text.NewLine,
            new Text("---      CUSTOM HELP PROVIDER      ---"), Text.NewLine,
            new Text("--------------------------------------"), Text.NewLine,
            Text.NewLine,
        };
    }
}