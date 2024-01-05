namespace Spectre.Console.Cli.Help;

public struct HelpProviderStyle
{
    public DescriptionStyle Description;
    public UsageStyle Usage;
    public ExampleStyle Examples;
    public ArgumentStyle Arguments;
    public CommandStyle Commands;
    public OptionStyle Options;
}

public struct DescriptionStyle
{
    public Style Header;
}

public struct UsageStyle
{
    public Style Header;
    public Style CurrentCommand;
    public Style Command;
    public Style Options;
    public Style RequiredArgument;
    public Style OptionalArgument;
}

public struct ExampleStyle
{
    public Style Header;
    public Style Arguments;
}

public struct ArgumentStyle
{
    public Style Header;
    public Style RequiredArgument;
    public Style OptionalArgument;
}

public struct CommandStyle
{
    public Style Header;
    public Style ChildCommand;
    public Style RequiredArgument;
}

public struct OptionStyle
{
    public Style Header;
    public Style DefaultValueHeader;
    public Style DefaultValue;
    public Style RequiredOption;
    public Style OptionalOption;
}

public struct Style
{
    public string Markup;
}