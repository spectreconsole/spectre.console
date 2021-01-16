using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class MultipleArgumentVectorSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        public string[] Foo { get; set; }

        [CommandArgument(0, "<Bars>")]
        public string[] Bar { get; set; }
    }

    public class MultipleArgumentVectorSpecifiedFirstSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        public string[] Foo { get; set; }

        [CommandArgument(1, "<Bar>")]
        public string Bar { get; set; }
    }
}
