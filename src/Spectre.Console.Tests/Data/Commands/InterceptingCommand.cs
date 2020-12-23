using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class InterceptingCommand<TSettings> : Command<TSettings>
    where TSettings : CommandSettings
    {
        private readonly Action<CommandContext, TSettings> _action;

        public InterceptingCommand(Action<CommandContext, TSettings> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public override int Execute(CommandContext context, TSettings settings)
        {
            _action(context, settings);
            return 0;
        }
    }
}
