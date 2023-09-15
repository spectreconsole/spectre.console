namespace Spectre.Console.Cli.Help;

internal static class ICommandInfoExtensions
{
    /// <summary>
    /// Walks up the command.Parent tree, adding each command into a list as it goes.
    /// </summary>
    /// <remarks>The first command added to the list is the current (ie. this one).</remarks>
    /// <returns>The list of commands from current to root, as traversed by <see cref="CommandInfo.Parent"/>.</returns>
    public static List<ICommandInfo> Flatten(this ICommandInfo commandInfo)
    {
        var result = new Stack<Help.ICommandInfo>();

        var current = commandInfo;
        while (current != null)
        {
            result.Push(current);
            current = current.Parent;
        }

        return result.ToList();
    }
}
