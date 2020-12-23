using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class ArgumentVectorSettings : CommandSettings
    {
        [CommandArgument(0, "<Foos>")]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
        public string[] Foo { get; set; }
    }
}
