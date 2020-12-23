using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class MultipleArgumentVectorSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public string[] Foo { get; set; }

        [CommandArgument(0, "<Bars>")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public string[] Bar { get; set; }
    }

    public class MultipleArgumentVectorSpecifiedFirstSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public string[] Foo { get; set; }

        [CommandArgument(1, "<Bar>")]
        public string Bar { get; set; }
    }
}
