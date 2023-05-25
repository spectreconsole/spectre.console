using System.ComponentModel;
using Spectre.Console.Cli;

namespace Delegates;

public static partial class Program
{
    public sealed class BarSettings : CommandSettings
    {
        [CommandOption("--count")]
        [Description("The number of bars to print")]
        [DefaultValue(3)]
        public int Count { get; set; }
    }
}
