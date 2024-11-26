using Spectre.Console.Cli;
using Spectre.Console.TrimTest.Commands.Run;

namespace Spectre.Console.TrimTest;

internal class MyInterceptor : ICommandInterceptor
{
    public void Intercept(CommandContext context, CommandSettings settings)
    {
        if (settings is not RunCommand.Settings runCommandSettings)
        {
            return;
        }

        if (runCommandSettings.Framework.Length > 0 && !runCommandSettings.Framework.StartsWith('v'))
        {
            runCommandSettings.Framework = "v" + runCommandSettings.Framework;
        }
    }
}