using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class ArgumentVectorSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        public string[] Foo { get; set; }
    }
}
