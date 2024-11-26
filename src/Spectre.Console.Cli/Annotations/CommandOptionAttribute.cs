namespace Spectre.Console.Cli;

/// <summary>
/// An attribute representing a command option.
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class CommandOptionAttribute : Attribute
{
    [DynamicallyAccessedMembers(PublicConstructors)]
    internal Type? OptionType { get; }

    /// <summary>
    /// Gets the long names of the option.
    /// </summary>
    /// <value>The option's long names.</value>
    public IReadOnlyList<string> LongNames { get; }

    /// <summary>
    /// Gets the short names of the option.
    /// </summary>
    /// <value>The option's short names.</value>
    public IReadOnlyList<string> ShortNames { get; }

    /// <summary>
    /// Gets the value name of the option.
    /// </summary>
    /// <value>The option's value name.</value>
    public string? ValueName { get; }

    /// <summary>
    /// Gets a value indicating whether the value is optional.
    /// </summary>
    public bool ValueIsOptional { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this option is hidden from the help text.
    /// </summary>
    public bool IsHidden { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandOptionAttribute"/> class.
    /// </summary>
    /// <param name="template">The option template.</param>
    /// <param name="optionType">The type of the parameter. Required for AOT scenarios.</param>
    public CommandOptionAttribute(string template, [DynamicallyAccessedMembers(PublicConstructors)] Type? optionType = null)
    {
        if (template == null)
        {
            throw new ArgumentNullException(nameof(template));
        }

        // Parse the option template.
        var result = TemplateParser.ParseOptionTemplate(template);

        // Assign the result.
        LongNames = result.LongNames;
        ShortNames = result.ShortNames;
        ValueName = result.Value;
        ValueIsOptional = result.ValueIsOptional;

        // if someone was explicit about the type of option, then we need to register an
        // explicit builder for this type to be used.
        OptionType = optionType;
        if (OptionType != null)
        {
            CreateInstanceHelpers.RegisterNewInstanceBuilder(OptionType);
        }
    }

    internal bool IsMatch(string name)
    {
        return
            ShortNames.Contains(name, StringComparer.Ordinal) ||
            LongNames.Contains(name, StringComparer.Ordinal);
    }
}