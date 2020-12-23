using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class FooCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[QUX]")]
        [Description("The qux value.")]
        public string Qux { get; set; }
    }
}
