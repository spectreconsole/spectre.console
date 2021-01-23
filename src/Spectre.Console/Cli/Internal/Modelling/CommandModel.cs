using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Spectre.Console.Cli
{
    internal sealed class CommandModel : ICommandContainer
    {
        public string? ApplicationName { get; }
        public ParsingMode ParsingMode { get; }
        public CommandInfo? DefaultCommand { get; }
        public IList<CommandInfo> Commands { get; }
        public IList<string[]> Examples { get; }

        public CommandModel(
            CommandAppSettings settings,
            CommandInfo? defaultCommand,
            IEnumerable<CommandInfo> commands,
            IEnumerable<string[]> examples)
        {
            ApplicationName = settings.ApplicationName;
            ParsingMode = settings.ParsingMode;
            DefaultCommand = defaultCommand;
            Commands = new List<CommandInfo>(commands ?? Array.Empty<CommandInfo>());
            Examples = new List<string[]>(examples ?? Array.Empty<string[]>());
        }

        public string GetApplicationName() =>
            ApplicationName ??
            Path.GetFileName(GetApplicationFile()) ?? // it is actually null safe
            "?";

        private static string? GetApplicationFile() =>
            Assembly.GetEntryAssembly()?.Location switch // location can be empty string or null
            {
                var location when !string.IsNullOrWhiteSpace(location) => location,
                _ => Process.GetCurrentProcess().MainModule?.FileName,
            };
    }
}
