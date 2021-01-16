using System;
using System.Threading.Tasks;

namespace Spectre.Console.Cli
{
    internal sealed class DelegateCommand : ICommand
    {
        private readonly Func<CommandContext, CommandSettings, int> _func;

        public DelegateCommand(Func<CommandContext, CommandSettings, int> func)
        {
            _func = func;
        }

        public Task<int> Execute(CommandContext context, CommandSettings settings)
        {
            return Task.FromResult(_func(context, settings));
        }

        public ValidationResult Validate(CommandContext context, CommandSettings settings)
        {
            return ValidationResult.Success();
        }
    }
}
