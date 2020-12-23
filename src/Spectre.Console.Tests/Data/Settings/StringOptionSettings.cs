using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class StringOptionSettings : CommandSettings
    {
        [CommandOption("-f|--foo")]
        public string Foo { get; set; }
    }
}
