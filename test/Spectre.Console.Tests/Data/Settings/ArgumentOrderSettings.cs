using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class ArgumentOrderSettings : CommandSettings
    {
        [CommandArgument(0, "[QUX]")]
        public int Qux { get; set; }

        [CommandArgument(3, "<CORGI>")]
        public int Corgi { get; set; }

        [CommandArgument(1, "<BAR>")]
        public int Bar { get; set; }

        [CommandArgument(2, "<BAZ>")]
        public int Baz { get; set; }

        [CommandArgument(0, "<FOO>")]
        public int Foo { get; set; }
    }
}
