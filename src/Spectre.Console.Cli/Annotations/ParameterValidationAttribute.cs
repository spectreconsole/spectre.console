namespace Spectre.Console.Cli;

/// <summary>
/// A base class attribute used for parameter validation.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public abstract class ParameterValidationAttribute : Attribute
{
    /// <summary>
    /// Gets the validation error message.
    /// </summary>
    /// <value>The validation error message.</value>
    public string ErrorMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterValidationAttribute"/> class.
    /// </summary>
    /// <param name="errorMessage">The validation error message.</param>
    protected ParameterValidationAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Validates the parameter value.
    /// </summary>
    /// <param name="context">The parameter context.</param>
    /// <returns>The validation result.</returns>
    public abstract ValidationResult Validate(CommandParameterContext context);
}