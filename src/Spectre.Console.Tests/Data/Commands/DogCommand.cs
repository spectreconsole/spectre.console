using System.ComponentModel;
using System.Linq;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    [Description("The dog command.")]
    public class DogCommand : AnimalCommand<DogSettings>
    {
        public override ValidationResult Validate(CommandContext context, DogSettings settings)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                throw new System.ArgumentNullException(nameof(settings));
            }

            if (settings.Age > 100 && !context.Remaining.Raw.Contains("zombie"))
            {
                return ValidationResult.Error("Dog is too old...");
            }

            return base.Validate(context, settings);
        }

        public override int Execute(CommandContext context, DogSettings settings)
        {
            DumpSettings(context, settings);
            return 0;
        }
    }
}