namespace Spectre.Console.Cli;

/// <summary>
/// A base class attribute used for parameter completion.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class ParameterValueProviderAttribute : Attribute
{
    /// <summary>
    /// Gets a value for the parameter.
    /// </summary>
    /// <param name="context">The parameter context.</param>
    /// <param name="result">The resulting value.</param>
    /// <returns><c>true</c> if a value was provided; otherwise, <c>false</c>.</returns>
    public abstract bool TryGetValue(CommandParameterContext context, out object? result);
}