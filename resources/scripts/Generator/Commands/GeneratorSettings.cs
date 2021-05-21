using Spectre.Console.Cli;

namespace Generator.Commands
{
    public class GeneratorSettings : CommandSettings
    {
        [CommandArgument(0, "<OUTPUT>")]
        public string Output { get; set; }
    }
}
