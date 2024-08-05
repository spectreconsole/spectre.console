namespace Spectre.Console.Cli;

internal sealed class Configurator : IUnsafeConfigurator, IConfigurator, IConfiguration
{
    private readonly ITypeRegistrar _registrar;

    public IList<ConfiguredCommand> Commands { get; }
    public CommandAppSettings Settings { get; }
    public ConfiguredCommand? DefaultCommand { get; private set; }
    public IList<string[]> Examples { get; }

    ICommandAppSettings IConfigurator.Settings => Settings;

    public Configurator(ITypeRegistrar registrar)
    {
        _registrar = registrar;

        Commands = new List<ConfiguredCommand>();
        Settings = new CommandAppSettings(registrar);
        Examples = new List<string[]>();
    }

    public void SetHelpProvider(IHelpProvider helpProvider)
    {
        // Register the help provider
        _registrar.RegisterInstance(typeof(IHelpProvider), helpProvider);
    }

    public void SetHelpProvider<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
        where T : IHelpProvider
    {
        // Register the help provider
        _registrar.Register(typeof(IHelpProvider), typeof(T));
    }

    public void AddExample(params string[] args)
    {
        Examples.Add(args);
    }

    [RequiresUnreferencedCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
    public ConfiguredCommand SetDefaultCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDefaultCommand>()
        where TDefaultCommand : class, ICommand
    {
        DefaultCommand = ConfiguredCommand.FromType<TDefaultCommand>(
            CliConstants.DefaultCommandName, isDefaultCommand: true);
        return DefaultCommand;
    }

    public ConfiguredCommand SetDefaultCommand<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDefaultCommand,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TDefaultSettings>()
        where TDefaultCommand : class, ICommand
    {
        DefaultCommand = ConfiguredCommand.FromType<TDefaultCommand, TDefaultSettings>(
            CliConstants.DefaultCommandName, isDefaultCommand: true);
        return DefaultCommand;
    }

    [RequiresUnreferencedCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
    public ICommandConfigurator AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommand>(string name)
        where TCommand : class, ICommand
    {
        var command = Commands.AddAndReturn(ConfiguredCommand.FromType<TCommand>(name, isDefaultCommand: false));
        return new CommandConfigurator(command);
    }

    /// <summary>
    /// Adds a command with an explicit settings type.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TSettings">The command settings type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    public ICommandConfigurator AddCommand<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommand,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSettings
    >(string name)
        where TCommand : class, ICommand
        where TSettings : CommandSettings
    {
        var command = Commands.AddAndReturn(ConfiguredCommand.FromType<TCommand, TSettings>(name, isDefaultCommand: false));
        return new CommandConfigurator(command);
    }

    public ICommandConfigurator AddDelegate<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSettings
    >(string name, Func<CommandContext, TSettings, int> func)
        where TSettings : CommandSettings
    {
        var command = Commands.AddAndReturn(ConfiguredCommand.FromDelegate<TSettings>(
            name, (context, settings) => Task.FromResult(func(context, (TSettings)settings))));
        return new CommandConfigurator(command);
    }

    public ICommandConfigurator AddAsyncDelegate<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSettings
    >(string name, Func<CommandContext, TSettings, Task<int>> func)
        where TSettings : CommandSettings
    {
        var command = Commands.AddAndReturn(ConfiguredCommand.FromDelegate<TSettings>(
            name, (context, settings) => func(context, (TSettings)settings)));
        return new CommandConfigurator(command);
    }

    public IBranchConfigurator AddBranch<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSettings
    >(string name, Action<IConfigurator<TSettings>> action)
        where TSettings : CommandSettings
    {
        var command = ConfiguredCommand.FromBranch<TSettings>(name);
        action(new Configurator<TSettings>(command, _registrar));
        var added = Commands.AddAndReturn(command);
        return new BranchConfigurator(added);
    }

    [RequiresDynamicCode("Uses MakeGenericType")]
    [RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can\'t validate that the requirements of those annotations are met.")]
    ICommandConfigurator IUnsafeConfigurator.AddCommand(
        string name,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type command)
    {
        var method = GetType().GetMethods().FirstOrDefault(i => i.Name == "AddCommand" && i.GetGenericArguments().Length == 1);
        if (method == null)
        {
            throw new CommandConfigurationException("Could not find AddCommand by reflection.");
        }

        method = method.MakeGenericMethod(command);

        if (!(method.Invoke(this, new object[] { name }) is ICommandConfigurator result))
        {
            throw new CommandConfigurationException("Invoking AddCommand returned null.");
        }

        return result;
    }

    [RequiresDynamicCode("Uses MakeGenericType")]
    IBranchConfigurator IUnsafeConfigurator.AddBranch(
        string name,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties
            | DynamicallyAccessedMemberTypes.Interfaces)]Type settings,
        Action<IUnsafeBranchConfigurator> action)
    {
        var command = ConfiguredCommand.FromBranch(settings, name);

        // Create the configurator.
        var configuratorType = typeof(Configurator<>).MakeGenericType(settings);
        if (!(Activator.CreateInstance(configuratorType, new object?[] { command, _registrar }) is IUnsafeBranchConfigurator configurator))
        {
            throw new CommandConfigurationException("Could not create configurator by reflection.");
        }

        action(configurator);
        var added = Commands.AddAndReturn(command);
        return new BranchConfigurator(added);
    }
}