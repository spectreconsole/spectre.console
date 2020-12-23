using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class BarCommandSettings : FooCommandSettings
    {
        [CommandArgument(0, "<CORGI>")]
        [Description("The corgi value.")]
        public string Corgi { get; set; }
    }
}
