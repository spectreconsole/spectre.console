namespace Spectre.Console.Cli.Help;

/// <summary>
/// Styles for the HelpProvider to use when rendering help text.
/// </summary>
public sealed class HelpProviderStyle
{
    /// <summary>
    /// Gets or sets the style for describing the purpose or details of a command.
    /// </summary>
    public DescriptionStyle? Description { get; set; }

    /// <summary>
    /// Gets or sets the style for specifying the usage format of a command.
    /// </summary>
    public UsageStyle? Usage { get; set; }

    /// <summary>
    /// Gets or sets the style for providing examples of command usage.
    /// </summary>
    public ExampleStyle? Examples { get; set; }

    /// <summary>
    /// Gets or sets the style for specifying arguments in a command.
    /// </summary>
    public ArgumentStyle? Arguments { get; set; }

    /// <summary>
    /// Gets or sets the style for specifying options or flags in a command.
    /// </summary>
    public OptionStyle? Options { get; set; }

    /// <summary>
    /// Gets or sets the style for specifying subcommands or nested commands.
    /// </summary>
    public CommandStyle? Commands { get; set; }

    /// <summary>
    /// Gets the default HelpProvider styles.
    /// </summary>
    public static HelpProviderStyle Default { get; } =
        new HelpProviderStyle()
        {
            Description = new DescriptionStyle()
            {
                Header = "yellow",
            },
            Usage = new UsageStyle()
            {
                Header = "yellow",
                CurrentCommand = "underline",
                Command = "aqua",
                Options = "grey",
                RequiredArgument = "aqua",
                OptionalArgument = "silver",
            },
            Examples = new ExampleStyle()
            {
                Header = "yellow",
                Arguments = "grey",
            },
            Arguments = new ArgumentStyle()
            {
                Header = "yellow",
                RequiredArgument = "silver",
                OptionalArgument = "silver",
            },
            Commands = new CommandStyle()
            {
                Header = "yellow",
                ChildCommand = "silver",
                RequiredArgument = "silver",
            },
            Options = new OptionStyle()
            {
                Header = "yellow",
                DefaultValueHeader = "lime",
                DefaultValue = "bold",
                RequiredOption = "silver",
                OptionalOption = "grey",
            },
        };
}

/// <summary>
/// Defines styles for describing the purpose or details of a command.
/// </summary>
public sealed class DescriptionStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the description.
    /// </summary>
    public Style? Header { get; set; }
}

/// <summary>
/// Defines styles for specifying the usage format of a command.
/// </summary>
public sealed class UsageStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the usage.
    /// </summary>
    public Style? Header { get; set; }

    /// <summary>
    /// Gets or sets the style for the current command in the usage.
    /// </summary>
    public Style? CurrentCommand { get; set; }

    /// <summary>
    /// Gets or sets the style for general commands in the usage.
    /// </summary>
    public Style? Command { get; set; }

    /// <summary>
    /// Gets or sets the style for options in the usage.
    /// </summary>
    public Style? Options { get; set; }

    /// <summary>
    /// Gets or sets the style for required arguments in the usage.
    /// </summary>
    public Style? RequiredArgument { get; set; }

    /// <summary>
    /// Gets or sets the style for optional arguments in the usage.
    /// </summary>
    public Style? OptionalArgument { get; set; }
}

/// <summary>
/// Defines styles for providing examples of command usage.
/// </summary>
public sealed class ExampleStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the examples.
    /// </summary>
    public Style? Header { get; set; }

    /// <summary>
    /// Gets or sets the style for arguments in the examples.
    /// </summary>
    public Style? Arguments { get; set; }
}

/// <summary>
/// Defines styles for specifying arguments in a command.
/// </summary>
public sealed class ArgumentStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the arguments.
    /// </summary>
    public Style? Header { get; set; }

    /// <summary>
    /// Gets or sets the style for required arguments.
    /// </summary>
    public Style? RequiredArgument { get; set; }

    /// <summary>
    /// Gets or sets the style for optional arguments.
    /// </summary>
    public Style? OptionalArgument { get; set; }
}

/// <summary>
/// Defines styles for specifying subcommands or nested commands.
/// </summary>
public sealed class CommandStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the command section.
    /// </summary>
    public Style? Header { get; set; }

    /// <summary>
    /// Gets or sets the style for child commands in the command section.
    /// </summary>
    public Style? ChildCommand { get; set; }

    /// <summary>
    /// Gets or sets the style for required arguments in the command section.
    /// </summary>
    public Style? RequiredArgument { get; set; }
}

/// <summary>
/// Defines styles for specifying options or flags in a command.
/// </summary>
public sealed class OptionStyle
{
    /// <summary>
    /// Gets or sets the style for the header in the options.
    /// </summary>
    public Style? Header { get; set; }

    /// <summary>
    /// Gets or sets the style for the header of default values in the options.
    /// </summary>
    public Style? DefaultValueHeader { get; set; }

    /// <summary>
    /// Gets or sets the style for default values in the options.
    /// </summary>
    public Style? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the style for required options.
    /// </summary>
    public Style? RequiredOption { get; set; }

    /// <summary>
    /// Gets or sets the style for optional options.
    /// </summary>
    public Style? OptionalOption { get; set; }
}
