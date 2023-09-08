using Spectre.Console.Rendering;

namespace Spectre.Console.Cli.Tests.Data.Help;

internal class RedirectHelpProvider : IHelpProvider
{
    public virtual IEnumerable<IRenderable> Write(ICommandModel model)
    {
        return Write(model, null);
    }
#nullable enable
    public virtual IEnumerable<IRenderable> Write(ICommandModel model, ICommandInfo? command)
#nullable disable
    {
        return new[]
        {
            new Text("Help has moved online. Please see: http://www.example.com"),
            Text.NewLine,
        };
    }
}