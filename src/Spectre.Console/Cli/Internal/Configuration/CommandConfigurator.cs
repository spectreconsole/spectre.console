namespace Spectre.Console.Cli
{
    internal sealed class CommandConfigurator : ICommandConfigurator
    {
        public ConfiguredCommand Command { get; }

        public CommandConfigurator(ConfiguredCommand command)
        {
            Command = command;
        }

        public ICommandConfigurator WithExample(string[] args)
        {
            Command.Examples.Add(args);
            return this;
        }

        public ICommandConfigurator WithAlias(string alias)
        {
            Command.Aliases.Add(alias);
            return this;
        }

        public ICommandConfigurator WithDescription(string description)
        {
            Command.Description = description;
            return this;
        }

        public ICommandConfigurator WithData(object data)
        {
            Command.Data = data;
            return this;
        }

        public ICommandConfigurator IsHidden()
        {
            Command.IsHidden = true;
            return this;
        }
    }
}
