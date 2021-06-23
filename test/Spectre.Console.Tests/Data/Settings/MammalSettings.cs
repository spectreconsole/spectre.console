using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public class MammalSettings : AnimalSettings
    {
        [CommandOption("-n|-p|--name|--pet-name <VALUE>")]
        public string Name { get; set; }
    }
}