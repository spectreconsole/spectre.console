namespace Spectre.Console.Tests.Data;

[Description("The turtle command.")]
public class TurtleCommand : AnimalCommand<TurtleSettings>
{
    public override int Execute(CommandContext context, TurtleSettings settings)
    {
        return 0;
    }
}
