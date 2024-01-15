namespace Spectre.Console.Cli.Help;

/// <summary>
/// Styles for the HelpProvider to use when rendering help text.
/// </summary>
public struct HelpProviderStyle
{
    /// <summary>
    /// The style for describing the purpose or details of a command.
    /// </summary>
    public DescriptionStyle Description;

    /// <summary>
    /// The style for specifying the usage format of a command.
    /// </summary>
    public UsageStyle Usage;

    /// <summary>
    /// The style for providing examples of command usage.
    /// </summary>
    public ExampleStyle Examples;

    /// <summary>
    /// The style for specifying arguments in a command.
    /// </summary>
    public ArgumentStyle Arguments;

    /// <summary>
    /// The style for specifying options or flags in a command.
    /// </summary>
    public OptionStyle Options;

    /// <summary>
    /// The style for specifying subcommands or nested commands.
    /// </summary>
    public CommandStyle Commands;

    /// <summary>
    /// Gets the default HelpProvider styles.
    /// </summary>
    public static HelpProviderStyle Default
    {
        get
        {
            HelpProviderStyle styles;

            styles.Description.Header.Markup = "yellow";
            styles.Usage.Header.Markup = "yellow";
            styles.Usage.CurrentCommand.Markup = "underline";
            styles.Usage.Command.Markup = "aqua";
            styles.Usage.Options.Markup = "grey";
            styles.Usage.RequiredArgument.Markup = "aqua";
            styles.Usage.OptionalArgument.Markup = "silver";
            styles.Examples.Header.Markup = "yellow";
            styles.Examples.Arguments.Markup = "grey";
            styles.Arguments.Header.Markup = "yellow";
            styles.Arguments.RequiredArgument.Markup = "silver";
            styles.Arguments.OptionalArgument.Markup = "silver";
            styles.Commands.Header.Markup = "yellow";
            styles.Commands.ChildCommand.Markup = "silver";
            styles.Commands.RequiredArgument.Markup = "silver";
            styles.Options.Header.Markup = "yellow";
            styles.Options.DefaultValueHeader.Markup = "lime";
            styles.Options.DefaultValue.Markup = "bold";
            styles.Options.RequiredOption.Markup = "silver";
            styles.Options.OptionalOption.Markup = "grey";

            return styles;
        }
    }

    /// <summary>
    /// Gets the unstyled HelpProvider styles.
    /// </summary>
    public static HelpProviderStyle None
    {
        get
        {
            HelpProviderStyle styles;

            styles.Description.Header.Markup = string.Empty;
            styles.Usage.Header.Markup = string.Empty;
            styles.Usage.CurrentCommand.Markup = string.Empty;
            styles.Usage.Command.Markup = string.Empty;
            styles.Usage.Options.Markup = string.Empty;
            styles.Usage.RequiredArgument.Markup = string.Empty;
            styles.Usage.OptionalArgument.Markup = string.Empty;
            styles.Examples.Header.Markup = string.Empty;
            styles.Examples.Arguments.Markup = string.Empty;
            styles.Arguments.Header.Markup = string.Empty;
            styles.Arguments.RequiredArgument.Markup = string.Empty;
            styles.Arguments.OptionalArgument.Markup = string.Empty;
            styles.Commands.Header.Markup = string.Empty;
            styles.Commands.ChildCommand.Markup = string.Empty;
            styles.Commands.RequiredArgument.Markup = string.Empty;
            styles.Options.Header.Markup = string.Empty;
            styles.Options.DefaultValueHeader.Markup = string.Empty;
            styles.Options.DefaultValue.Markup = string.Empty;
            styles.Options.RequiredOption.Markup = string.Empty;
            styles.Options.OptionalOption.Markup = string.Empty;

            return styles;
        }
    }
}

#pragma warning disable SA1600 // XML documentation for public members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public struct DescriptionStyle
{
    public MarkupStyle Header;
}

public struct UsageStyle
{
    public MarkupStyle Header;
    public MarkupStyle CurrentCommand;
    public MarkupStyle Command;
    public MarkupStyle Options;
    public MarkupStyle RequiredArgument;
    public MarkupStyle OptionalArgument;
}

public struct ExampleStyle
{
    public MarkupStyle Header;
    public MarkupStyle Arguments;
}

public struct ArgumentStyle
{
    public MarkupStyle Header;
    public MarkupStyle RequiredArgument;
    public MarkupStyle OptionalArgument;
}

public struct CommandStyle
{
    public MarkupStyle Header;
    public MarkupStyle ChildCommand;
    public MarkupStyle RequiredArgument;
}

public struct OptionStyle
{
    public MarkupStyle Header;
    public MarkupStyle DefaultValueHeader;
    public MarkupStyle DefaultValue;
    public MarkupStyle RequiredOption;
    public MarkupStyle OptionalOption;
}

public struct MarkupStyle
{
    public string Markup;
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore SA1600 // XML documentation for public members