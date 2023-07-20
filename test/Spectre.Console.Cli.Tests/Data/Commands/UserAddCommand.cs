using Spectre.Console.Cli.Completion;

namespace Spectre.Console.Cli.Tests.Data.Commands;

// mycommand user add [name] --age [age]
internal class UserAddSettings : CommandSettings
{
    [CommandArgument(0, "<name>")]
    [Description("The name of the user.")]
    [CompletionSuggestions("Angelika", "Arnold", "Bernd", "Cloud", "Jonas")]
    public string Name { get; set; }

    [CommandOption("-a|--age <age>")]
    [Description("The age of the user.")]
    [CompletionSuggestions("10", "15", "20", "30")]
    public int Age { get; set; }
}

[Description("The user command.")]
internal class UserAddCommand : Command<UserAddSettings>
{
    public override int Execute(CommandContext context, UserAddSettings settings)
    {
        return 0;
    }
}