using System.Diagnostics;
using Spectre.Console.Cli;

namespace AutoCompletion;

// Adding autocomplete to powershell:
//  - .\AutoCompletion.exe completion powershell
//
// Adding autocomplete to powershell (permanent):
// - .\AutoCompletion.exe completion powershell --install
//
// Test completing:
// - .\AutoCompletion.exe cli complete "Li"
internal static class Program
{
    private static void Main(string[] args)
    {
        // If we just want to test the completion with f5 in visual studio
        if (Debugger.IsAttached)
        {
            args = new[] { "cli", "complete", "\"Li\"" };
        }

        var app = new CommandApp();
        app.Configure(config => config.AddCommand<LionCommand>("lion"));

        app.Run(args);
    }
}