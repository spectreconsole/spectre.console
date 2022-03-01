using System;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Dynamic;

public sealed class MyCommand : Command
{
    public override int Execute(CommandContext context)
    {
        if (!(context.Data is int data))
        {
            throw new InvalidOperationException("Command has no associated data.");

        }

        AnsiConsole.WriteLine("Value = {0}", data);
        return 0;
    }
}
