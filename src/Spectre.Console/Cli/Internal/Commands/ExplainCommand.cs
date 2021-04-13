using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console.Cli
{
    [Description("Displays diagnostics about CLI configurations")]
    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
    internal sealed class ExplainCommand : Command<ExplainCommand.Settings>
    {
        private readonly CommandModel _commandModel;
        private readonly IAnsiConsole _writer;

        public ExplainCommand(IConfiguration configuration, CommandModel commandModel)
        {
            _commandModel = commandModel ?? throw new ArgumentNullException(nameof(commandModel));
            _writer = configuration.Settings.Console.GetConsole();
        }

        public sealed class Settings : CommandSettings
        {
            public Settings(string[]? commands, bool? detailed, bool includeHidden)
            {
                Commands = commands;
                Detailed = detailed;
                IncludeHidden = includeHidden;
            }

            [CommandArgument(0, "[command]")]
            public string[]? Commands { get; }

            [Description("Include detailed information about the commands.")]
            [CommandOption("-d|--detailed")]
            public bool? Detailed { get; }

            [Description("Include hidden commands.")]
            [CommandOption("--hidden")]
            public bool IncludeHidden { get; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var tree = new Tree("CLI Configuration");
            tree.AddNode(ValueMarkup("Application Name", _commandModel.ApplicationName, "no application name"));
            tree.AddNode(ValueMarkup("Parsing Mode", _commandModel.ParsingMode.ToString()));

            if (settings.Commands == null || settings.Commands.Length == 0)
            {
                // If there is a default command we'll want to include it in the list too.
                var commands = _commandModel.DefaultCommand != null
                    ? new[] { _commandModel.DefaultCommand }.Concat(_commandModel.Commands)
                    : _commandModel.Commands;

                AddCommands(
                    tree.AddNode(ParentMarkup("Commands")),
                    commands,
                    settings.Detailed ?? false,
                    settings.IncludeHidden);
            }
            else
            {
                var currentCommandTier = _commandModel.Commands;
                CommandInfo? currentCommand = null;
                foreach (var command in settings.Commands)
                {
                    currentCommand = currentCommandTier
                        .SingleOrDefault(i =>
                            i.Name.Equals(command, StringComparison.CurrentCultureIgnoreCase) ||
                            i.Aliases
                            .Any(alias => alias.Equals(command, StringComparison.CurrentCultureIgnoreCase)));

                    if (currentCommand == null)
                    {
                        break;
                    }

                    currentCommandTier = currentCommand.Children;
                }

                if (currentCommand == null)
                {
                    throw new Exception($"Command {string.Join(" ", settings.Commands)} not found");
                }

                AddCommands(
                    tree.AddNode(ParentMarkup("Commands")),
                    new[] { currentCommand },
                    settings.Detailed ?? true,
                    settings.IncludeHidden);
            }

            _writer.Write(tree);

            return 0;
        }

        private IRenderable ValueMarkup(string key, string value)
        {
            return new Markup($"{key}: [blue]{value.EscapeMarkup()}[/]");
        }

        private IRenderable ValueMarkup(string key, string? value, string noValueText)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new Markup($"{key}: [grey]({noValueText.EscapeMarkup()})[/]");
            }

            var table = new Table().NoBorder().HideHeaders().AddColumns("key", "value");
            table.AddRow($"{key}", $"[blue]{value.EscapeMarkup()}[/]");
            return table;
        }

        private string ParentMarkup(string description)
        {
            return $"[yellow]{description.EscapeMarkup()}[/]";
        }

        private void AddStringList(TreeNode node, string description, IList<string>? strings)
        {
            if (strings == null || strings.Count == 0)
            {
                return;
            }

            var parentNode = node.AddNode(ParentMarkup(description));
            foreach (var s in strings)
            {
                parentNode.AddNode(s);
            }
        }

        private void AddCommands(TreeNode node, IEnumerable<CommandInfo> commands, bool detailed, bool includeHidden)
        {
            foreach (var command in commands)
            {
                if (!includeHidden && command.IsHidden)
                {
                    continue;
                }

                var commandName = $"[green]{command.Name}[/]";
                if (command.IsDefaultCommand)
                {
                    commandName += " (default)";
                }

                var commandNode = node.AddNode(commandName);
                commandNode.AddNode(ValueMarkup("Description", command.Description, "no description"));
                if (command.IsHidden)
                {
                    commandNode.AddNode(ValueMarkup("IsHidden", command.IsHidden.ToString()));
                }

                if (!command.IsBranch)
                {
                    commandNode.AddNode(ValueMarkup("Type", command.CommandType?.ToString(), "no command type"));
                    commandNode.AddNode(ValueMarkup("Settings Type", command.SettingsType.ToString()));
                }

                if (command.Parameters.Count > 0)
                {
                    var parametersNode = commandNode.AddNode(ParentMarkup("Parameters"));
                    foreach (var parameter in command.Parameters)
                    {
                        AddParameter(parametersNode, parameter, detailed);
                    }
                }

                AddStringList(commandNode, "Aliases", command.Aliases.ToList());
                AddStringList(commandNode, "Examples", command.Examples.Select(i => string.Join(" ", i)).ToList());

                if (command.Children.Count > 0)
                {
                    var childNode = commandNode.AddNode(ParentMarkup("Child Commands"));
                    AddCommands(childNode, command.Children, detailed, includeHidden);
                }
            }
        }

        private void AddParameter(TreeNode parametersNode, CommandParameter parameter, bool detailed)
        {
            if (!detailed)
            {
                parametersNode.AddNode(
                    $"{parameter.PropertyName} [purple]{GetShortOptions(parameter)}[/] [grey]{parameter.Property.PropertyType.ToString().EscapeMarkup()}[/]");

                return;
            }

            var parameterNode = parametersNode.AddNode(
                $"{parameter.PropertyName} [grey]{parameter.Property.PropertyType.ToString().EscapeMarkup()}[/]");

            parameterNode.AddNode(ValueMarkup("Description", parameter.Description, "no description"));
            parameterNode.AddNode(ValueMarkup("Parameter Kind", parameter.ParameterKind.ToString()));

            if (parameter is CommandOption commandOptionParameter)
            {
                if (commandOptionParameter.IsShadowed)
                {
                    parameterNode.AddNode(ValueMarkup("IsShadowed", commandOptionParameter.IsShadowed.ToString()));
                }

                if (commandOptionParameter.LongNames.Count > 0)
                {
                    parameterNode.AddNode(ValueMarkup(
                        "Long Names",
                        string.Join("|", commandOptionParameter.LongNames.Select(i => $"--{i}"))));

                    parameterNode.AddNode(ValueMarkup(
                        "Short Names",
                        string.Join("|", commandOptionParameter.ShortNames.Select(i => $"-{i}"))));
                }
            }
            else if (parameter is CommandArgument commandArgumentParameter)
            {
                parameterNode.AddNode(ValueMarkup("Position", commandArgumentParameter.Position.ToString()));
                parameterNode.AddNode(ValueMarkup("Value", commandArgumentParameter.Value));
            }

            parameterNode.AddNode(ValueMarkup("Required", parameter.Required.ToString()));

            if (parameter.Converter != null)
            {
                parameterNode.AddNode(ValueMarkup(
                    "Converter", $"\"{parameter.Converter.ConverterTypeName}\""));
            }

            if (parameter.DefaultValue != null)
            {
                parameterNode.AddNode(ValueMarkup(
                    "Default Value", $"\"{parameter.DefaultValue.Value}\""));
            }

            if (parameter.PairDeconstructor != null)
            {
                parameterNode.AddNode(ValueMarkup("Pair Deconstructor", parameter.PairDeconstructor.Type.ToString()));
            }

            AddStringList(
                parameterNode,
                "Validators",
                parameter.Validators.Select(i => i.GetType().ToString()).ToList());
        }

        private static string GetShortOptions(CommandParameter parameter)
        {
            if (parameter is CommandOption commandOptionParameter)
            {
                return string.Join(
                    " | ",
                    commandOptionParameter.LongNames.Select(i => $"--{i}")
                        .Concat(commandOptionParameter.ShortNames.Select(i => $"-{i}")));
            }

            if (parameter is CommandArgument commandArgumentParameter)
            {
                return $"{commandArgumentParameter.Value} position {commandArgumentParameter.Position}";
            }

            return string.Empty;
        }
    }
}