namespace Spectre.Console.Cli;

/// <summary>
/// Indicates that a "Description" feature should display its localization description.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class LocalizationAttribute : Attribute
{
    /// <summary>
    ///   Gets or Sets a strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    public Type ResourceClass { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizationAttribute"/> class.
    /// </summary>
    /// <param name="resourceClass">
    ///      The type of the resource manager.
    /// </param>
    public LocalizationAttribute(Type resourceClass)
    {
        ResourceClass = resourceClass;
    }
}