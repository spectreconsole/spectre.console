using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class OptionVectorSettings : CommandSettings
    {
        [CommandOption("--foo")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public string[] Foo { get; set; }

        [CommandOption("--bar")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public int[] Bar { get; set; }
    }
}
