using Logging.Commands;
using Spectre.Console.Cli;

namespace Logging
{
    public class LogInterceptor : ICommandInterceptor
    {
        public void Intercept(CommandContext context, CommandSettings settings)
        {
            if (settings is LogCommandSettings logSettings)
            {
                LogFileNameEnricher.Path = logSettings.LogFile;
            }
        }
    }
}