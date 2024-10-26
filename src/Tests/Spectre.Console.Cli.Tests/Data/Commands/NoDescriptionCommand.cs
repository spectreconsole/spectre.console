namespace Spectre.Console.Tests.Data;

public sealed class NoDescriptionCommand : Command<EmptyCommandSettings>
{
    [CommandOption("-f|--foo <VALUE>")]
    public int Foo { get; set; }

    public override int Execute(CommandContext context, EmptyCommandSettings settings)
    {
        return 0;
    }
}
