using System;
using System.Collections.Generic;
using Spectre.Console.Cli.Unsafe;

namespace Spectre.Console.Cli
{
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

        public void AddExample(string[] args)
        {
            Examples.Add(args);
        }

        public void SetDefaultCommand<TDefaultCommand>()
            where TDefaultCommand : class, ICommand
        {
            DefaultCommand = ConfiguredCommand.FromType<TDefaultCommand>(
                CliConstants.DefaultCommandName, isDefaultCommand: true);
        }

        public ICommandConfigurator AddCommand<TCommand>(string name)
            where TCommand : class, ICommand
        {
            var command = Commands.AddAndReturn(ConfiguredCommand.FromType<TCommand>(name, false));
            return new CommandConfigurator(command);
        }

        public ICommandConfigurator AddDelegate<TSettings>(string name, Func<CommandContext, TSettings, int> func)
            where TSettings : CommandSettings
        {
            var command = Commands.AddAndReturn(ConfiguredCommand.FromDelegate<TSettings>(
                name, (context, settings) => func(context, (TSettings)settings)));
            return new CommandConfigurator(command);
        }

        public void AddBranch<TSettings>(string name, Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings
        {
            var command = ConfiguredCommand.FromBranch<TSettings>(name);
            action(new Configurator<TSettings>(command, _registrar));
            Commands.Add(command);
        }

        ICommandConfigurator IUnsafeConfigurator.AddCommand(string name, Type command)
        {
            var method = GetType().GetMethod("AddCommand");
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

        void IUnsafeConfigurator.AddBranch(string name, Type settings, Action<IUnsafeBranchConfigurator> action)
        {
            var command = ConfiguredCommand.FromBranch(settings, name);

            // Create the configurator.
            var configuratorType = typeof(Configurator<>).MakeGenericType(settings);
            if (!(Activator.CreateInstance(configuratorType, new object?[] { command, _registrar }) is IUnsafeBranchConfigurator configurator))
            {
                throw new CommandConfigurationException("Could not create configurator by reflection.");
            }

            action(configurator);
            Commands.Add(command);
        }
    }
}
