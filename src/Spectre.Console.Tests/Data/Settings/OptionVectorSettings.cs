using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class OptionVectorSettings : CommandSettings
    {
        [CommandOption("--foo")]
        public string[] Foo { get; set; }

        [CommandOption("--bar")]
        public int[] Bar { get; set; }
    }
}
