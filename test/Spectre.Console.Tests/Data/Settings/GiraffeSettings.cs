using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class GiraffeSettings : MammalSettings
    {
        [CommandArgument(0, "<LENGTH>")]
        [Description("The option description.")]
        public int Length { get; set; }
    }
}