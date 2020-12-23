using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Testing
{
    public sealed class FakeCommandInterceptor : ICommandInterceptor
    {
        private readonly Action<CommandContext, CommandSettings> _action;

        public FakeCommandInterceptor(Action<CommandContext, CommandSettings> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public void Intercept(CommandContext context, CommandSettings settings)
        {
            _action(context, settings);
        }
    }
}
