using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Spectre.Console.Cli.Internal
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

        public string GetApplicationName()
        {
            return ApplicationName ?? Path.GetFileName(Assembly.GetEntryAssembly()?.Location) ?? "?";
        }
    }
}
