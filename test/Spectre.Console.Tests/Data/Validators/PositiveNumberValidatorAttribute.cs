namespace Spectre.Console.Tests.Data;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class PositiveNumberValidatorAttribute : ParameterValidationAttribute
{
    public PositiveNumberValidatorAttribute(string errorMessage)
        : base(errorMessage)
    {
    }

    public override ValidationResult Validate(CommandParameterContext context)
    {
        if (context.Value is int integer)
        {
            if (integer > 0)
            {
                return ValidationResult.Success();
            }

            return ValidationResult.Error($"Number is not greater than 0 ({context.Parameter.PropertyName}).");
        }

        throw new InvalidOperationException($"Parameter is not a number ({context.Parameter.PropertyName}).");
    }
}
