namespace Spectre.Console.Cli;

internal sealed class Configurator<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSettings> : IUnsafeBranchConfigurator, IConfigurator<TSettings>
    where TSettings : CommandSettings
{
    private readonly ConfiguredCommand _command;
    private readonly ITypeRegistrar? _registrar;

    public Configurator(ConfiguredCommand command, ITypeRegistrar? registrar)
    {
        _command = command;
        _registrar = registrar;
    }

    public void SetDescription(string description)
    {
        _command.Description = description;
    }

    public void AddExample(params string[] args)
    {
        _command.Examples.Add(args);
    }

    [RequiresUnreferencedCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
    public void SetDefaultCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TDefaultCommand>()
        where TDefaultCommand : class, ICommandLimiter<TSettings>
    {
        var defaultCommand = ConfiguredCommand.FromType<TDefaultCommand>(
            CliConstants.DefaultCommandName, isDefaultCommand: true);

        _command.Children.Add(defaultCommand);
    }

    public void SetDefaultCommand<
        TDefaultCommand,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TDefaultSettings>()
        where TDefaultCommand : class, ICommandLimiter<TSettings>
        where TDefaultSettings : CommandSettings
    {
        var defaultCommand = ConfiguredCommand.FromType<TDefaultCommand, TDefaultSettings>(
            CliConstants.DefaultCommandName, isDefaultCommand: true);

        _command.Children.Add(defaultCommand);
    }

    public void HideBranch()
    {
        _command.IsHidden = true;
    }

    [RequiresUnreferencedCode(TrimWarnings.AddCommandShouldBeExplicitAboutSettings)]
    public ICommandConfigurator AddCommand<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommand>(string name)
        where TCommand : class, ICommandLimiter<TSettings>
    {
        var command = ConfiguredCommand.FromType<TCommand>(name, isDefaultCommand: false);
        var configurator = new CommandConfigurator(command);

        _command.Children.Add(command);
        return configurator;
    }

    public ICommandConfigurator AddCommand<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TCommand,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TSetting
    >(string name)
        where TCommand : class, ICommandLimiter<TSettings>
        where TSetting : CommandSettings
    {
        var command = ConfiguredCommand.FromType<TCommand, TSetting>(name, isDefaultCommand: false);
        var configurator = new CommandConfigurator(command);

        _command.Children.Add(command);
        return configurator;
    }

    public ICommandConfigurator AddDelegate<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TDerivedSettings
    >(string name, Func<CommandContext, TDerivedSettings, int> func)
        where TDerivedSettings : TSettings
    {
        var command = ConfiguredCommand.FromDelegate<TDerivedSettings>(
            name, (context, settings) => Task.FromResult(func(context, (TDerivedSettings)settings)));

        _command.Children.Add(command);
        return new CommandConfigurator(command);
    }

    public ICommandConfigurator AddAsyncDelegate<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TDerivedSettings
    >(string name, Func<CommandContext, TDerivedSettings, Task<int>> func)
        where TDerivedSettings : TSettings
    {
        var command = ConfiguredCommand.FromDelegate<TDerivedSettings>(
            name, (context, settings) => func(context, (TDerivedSettings)settings));

        _command.Children.Add(command);
        return new CommandConfigurator(command);
    }

    public IBranchConfigurator AddBranch<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TDerivedSettings
    >(string name, Action<IConfigurator<TDerivedSettings>> action)
        where TDerivedSettings : TSettings
    {
        var command = ConfiguredCommand.FromBranch<TDerivedSettings>(name);
        action(new Configurator<TDerivedSettings>(command, _registrar));
        var added = _command.Children.AddAndReturn(command);
        return new BranchConfigurator(added);
    }

    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL2060", Justification = TrimWarnings.SuppressMessage)]
    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL3050", Justification = TrimWarnings.SuppressMessage)]
    ICommandConfigurator IUnsafeConfigurator.AddCommand(string name, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type command)
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

    [UnconditionalSuppressMessage("AssemblyLoadTrimming", "IL3050", Justification = TrimWarnings.SuppressMessage)]
    IBranchConfigurator IUnsafeConfigurator.AddBranch(
        string name,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type settings,
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
        var added = _command.Children.AddAndReturn(command);
        return new BranchConfigurator(added);
    }
}