using Spectre.Console.Cli.Help;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli.Tests.Data.Help;

internal class RedirectHelpProvider : IHelpProvider
{
    public virtual IEnumerable<IRenderable> Write(ICommandModel model)
    {
        return WriteCommand(model, null);
    }
#nullable enable
    public virtual IEnumerable<IRenderable> WriteCommand(ICommandModel model, ICommandInfo? command)
#nullable disable
    {
        var result = new List<IRenderable>();

        result.AddRange(Enumerable.Repeat(new Composer().Text("Help has moved online. Please see: http://www.example.com").LineBreak(), 1));

        return result;
    }
}