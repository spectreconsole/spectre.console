using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class LionSettings : CatSettings
    {
        [CommandArgument(0, "<TEETH>")]
        [Description("The number of teeth the lion has.")]
        public int Teeth { get; set; }

        [CommandOption("-c <CHILDREN>")]
        [Description("The number of children the lion has.")]
        public int Children { get; set; }
    }
}