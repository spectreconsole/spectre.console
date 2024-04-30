namespace Spectre.Console.Cli;

internal sealed class CommandInfo : ICommandContainer, ICommandInfo
{
    public string Name { get; }
    public HashSet<string> Aliases { get; }
    public string? Description { get; }
    public object? Data { get; }
    public Type? CommandType { get; }
    public Type SettingsType { get; }
    public Func<CommandContext, CommandSettings, Task<int>>? Delegate { get; }
    public bool IsDefaultCommand { get; }
    public CommandInfo? Parent { get; }
    public IList<CommandInfo> Children { get; }
    public IList<CommandParameter> Parameters { get; }
    public IList<string[]> Examples { get; }

    public bool IsBranch => CommandType == null && Delegate == null;
    IList<CommandInfo> ICommandContainer.Commands => Children;

    // only branches can have a default command
    public CommandInfo? DefaultCommand => IsBranch ? Children.FirstOrDefault(c => c.IsDefaultCommand) : null;
    public bool IsHidden { get; }

    IReadOnlyList<ICommandInfo> Help.ICommandContainer.Commands => Children.Cast<ICommandInfo>().ToList();
    ICommandInfo? Help.ICommandContainer.DefaultCommand => DefaultCommand;
    IReadOnlyList<ICommandParameter> ICommandInfo.Parameters => Parameters.Cast<ICommandParameter>().ToList();
    ICommandInfo? ICommandInfo.Parent => Parent;
    IReadOnlyList<string[]> Help.ICommandContainer.Examples => (IReadOnlyList<string[]>)Examples;

    public CommandInfo(CommandInfo? parent, ConfiguredCommand prototype)
    {
        Parent = parent;

        Name = prototype.Name;
        Aliases = new HashSet<string>(prototype.Aliases);
        Description = prototype.Description;
        Data = prototype.Data;
        CommandType = prototype.CommandType;
        SettingsType = prototype.SettingsType;
        Delegate = prototype.Delegate;
        IsDefaultCommand = prototype.IsDefaultCommand;
        IsHidden = prototype.IsHidden;

        Children = new List<CommandInfo>();
        Parameters = new List<CommandParameter>();
        Examples = prototype.Examples ?? new List<string[]>();

        if (CommandType != null && string.IsNullOrWhiteSpace(Description))
        {
            var description = CommandType.GetCustomAttribute<DescriptionAttribute>();
            if (description != null)
            {
                Description = description.Description;
            }
        }
    }
}