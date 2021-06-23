using System;
using Spectre.Console.Cli;

namespace Spectre.Console.Tests.Data
{
    public sealed class ThrowingCommand : Command<ThrowingCommandSettings>
    {
        public override int Execute(CommandContext context, ThrowingCommandSettings settings)
        {
            throw new InvalidOperationException("W00t?");
        }
    }

    public sealed class ThrowingCommandSettings : CommandSettings
    {
    }
}
