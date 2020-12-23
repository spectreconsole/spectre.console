using System.ComponentModel;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public abstract class AnimalSettings : CommandSettings
    {
        [CommandOption("-a|--alive|--not-dead")]
        [Description("Indicates whether or not the animal is alive.")]
        public bool IsAlive { get; set; }

        [CommandArgument(1, "[LEGS]")]
        [Description("The number of legs.")]
        [EvenNumberValidator("Animals must have an even number of legs.")]
        [PositiveNumberValidator("Number of legs must be greater than 0.")]
        public int Legs { get; set; }
    }
}