using System.ComponentModel;
using Spectre.Console.Cli;

namespace Logging.Commands
{
    public class LogCommandSettings : CommandSettings
    {
        [CommandOption("--logFile")]
        [Description("Path and file name for logging")]
        public string LogFile { get; set; } = "output.log";
    }
}