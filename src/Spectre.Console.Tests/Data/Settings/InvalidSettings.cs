using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class InvalidSettings : CommandSettings
    {
        [CommandOption("-f|--foo [BAR]")]
        public string Value { get; set; }
    }
}
