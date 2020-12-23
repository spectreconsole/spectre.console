using System;
using System.Linq;
using Spectre.Console.Cli;
using SystemConsole = System.Console;

namespace Spectre.Console.Tests.Data
{
    public abstract class AnimalCommand<TSettings> : Command<TSettings>
        where TSettings : CommandSettings
    {
        protected void DumpSettings(CommandContext context, TSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var properties = settings.GetType().GetProperties();
            foreach (var group in properties.GroupBy(x => x.DeclaringType).Reverse())
            {
                SystemConsole.WriteLine();
                SystemConsole.ForegroundColor = ConsoleColor.Yellow;
                SystemConsole.WriteLine(group.Key.FullName);
                SystemConsole.ResetColor();

                foreach (var property in group)
                {
                    SystemConsole.WriteLine($"  {property.Name} = {property.GetValue(settings)}");
                }
            }

            if (context.Remaining.Raw.Count > 0)
            {
                SystemConsole.WriteLine();
                SystemConsole.ForegroundColor = ConsoleColor.Yellow;
                SystemConsole.WriteLine("Remaining:");
                SystemConsole.ResetColor();
                SystemConsole.WriteLine(string.Join(", ", context.Remaining));
            }

            SystemConsole.WriteLine();
        }
    }
}
