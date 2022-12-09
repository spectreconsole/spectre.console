namespace Spectre.Console.Cli;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class LocalizationAttribute : Attribute
{
    /// <summary>
    ///     A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    public Type ResourceClass { get; set; }

    public LocalizationAttribute(Type resourceClass)
    {
        ResourceClass = resourceClass;
    }
}