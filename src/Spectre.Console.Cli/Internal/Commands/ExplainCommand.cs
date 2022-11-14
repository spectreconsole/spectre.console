namespace Spectre.Console.Cli;

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

        [Description("Include hidden commands and options.")]
        [CommandOption("--hidden")]
        public bool IncludeHidden { get; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var tree = new Tree("CLI Configuration");
        tree.AddNode(ExplainCommand.ValueMarkup("Application Name", _commandModel.ApplicationName, "no application name"));
        tree.AddNode(ExplainCommand.ValueMarkup("Parsing Mode", _commandModel.ParsingMode.ToString()));

        if (settings.Commands == null || settings.Commands.Length == 0)
        {
            // If there is a default command we'll want to include it in the list too.
            var commands = _commandModel.DefaultCommand != null
                ? new[] { _commandModel.DefaultCommand }.Concat(_commandModel.Commands)
                : _commandModel.Commands;

            AddCommands(
                tree.AddNode(ExplainCommand.ParentMarkup("Commands")),
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
                tree.AddNode(ExplainCommand.ParentMarkup("Commands")),
                new[] { currentCommand },
                settings.Detailed ?? true,
                settings.IncludeHidden);
        }

        _writer.Write(tree);

        return 0;
    }

    private static IRenderable ValueMarkup(string key, string value)
    {
        return new Markup($"{key}: [blue]{value.EscapeMarkup()}[/]");
    }

    private static IRenderable ValueMarkup(string key, string? value, string noValueText)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new Markup($"{key}: [grey]({noValueText.EscapeMarkup()})[/]");
        }

        var table = new Table().NoBorder().HideHeaders().AddColumns("key", "value");
        table.AddRow($"{key}", $"[blue]{value.EscapeMarkup()}[/]");
        return table;
    }

    private static string ParentMarkup(string description)
    {
        return $"[yellow]{description.EscapeMarkup()}[/]";
    }

    private static void AddStringList(TreeNode node, string description, IList<string>? strings)
    {
        if (strings == null || strings.Count == 0)
        {
            return;
        }

        var parentNode = node.AddNode(ExplainCommand.ParentMarkup(description));
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
            commandNode.AddNode(ExplainCommand.ValueMarkup("Description", command.Description, "no description"));
            if (command.IsHidden)
            {
                commandNode.AddNode(ExplainCommand.ValueMarkup("IsHidden", command.IsHidden.ToString()));
            }

            if (!command.IsBranch)
            {
                commandNode.AddNode(ExplainCommand.ValueMarkup("Type", command.CommandType?.ToString(), "no command type"));
                commandNode.AddNode(ExplainCommand.ValueMarkup("Settings Type", command.SettingsType.ToString()));
            }

            if (command.Parameters.Count > 0)
            {
                var parametersNode = commandNode.AddNode(ExplainCommand.ParentMarkup("Parameters"));
                foreach (var parameter in command.Parameters)
                {
                    ExplainCommand.AddParameter(parametersNode, parameter, detailed, includeHidden);
                }
            }

            ExplainCommand.AddStringList(commandNode, "Aliases", command.Aliases.ToList());
            ExplainCommand.AddStringList(commandNode, "Examples", command.Examples.Select(i => string.Join(" ", i)).ToList());

            if (command.Children.Count > 0)
            {
                var childNode = commandNode.AddNode(ExplainCommand.ParentMarkup("Child Commands"));
                AddCommands(childNode, command.Children, detailed, includeHidden);
            }
        }
    }

    private static void AddParameter(TreeNode parametersNode, CommandParameter parameter, bool detailed, bool includeHidden)
    {
        if (!includeHidden && parameter.IsHidden)
        {
            return;
        }

        if (!detailed)
        {
            parametersNode.AddNode(
                $"{parameter.PropertyName} [purple]{GetShortOptions(parameter)}[/] [grey]{parameter.Property.PropertyType.ToString().EscapeMarkup()}[/]");

            return;
        }

        var parameterNode = parametersNode.AddNode(
            $"{parameter.PropertyName} [grey]{parameter.Property.PropertyType.ToString().EscapeMarkup()}[/]");

        parameterNode.AddNode(ExplainCommand.ValueMarkup("Description", parameter.Description, "no description"));
        parameterNode.AddNode(ExplainCommand.ValueMarkup("Parameter Kind", parameter.ParameterKind.ToString()));

        if (parameter is CommandOption commandOptionParameter)
        {
            if (commandOptionParameter.IsShadowed)
            {
                parameterNode.AddNode(ExplainCommand.ValueMarkup("IsShadowed", commandOptionParameter.IsShadowed.ToString()));
            }

            if (commandOptionParameter.LongNames.Count > 0)
            {
                parameterNode.AddNode(ExplainCommand.ValueMarkup(
                    "Long Names",
                    string.Join("|", commandOptionParameter.LongNames.Select(i => $"--{i}"))));

                parameterNode.AddNode(ExplainCommand.ValueMarkup(
                    "Short Names",
                    string.Join("|", commandOptionParameter.ShortNames.Select(i => $"-{i}"))));
            }
        }
        else if (parameter is CommandArgument commandArgumentParameter)
        {
            parameterNode.AddNode(ExplainCommand.ValueMarkup("Position", commandArgumentParameter.Position.ToString()));
            parameterNode.AddNode(ExplainCommand.ValueMarkup("Value", commandArgumentParameter.Value));
        }

        parameterNode.AddNode(ExplainCommand.ValueMarkup("Required", parameter.Required.ToString()));

        if (parameter.Converter != null)
        {
            parameterNode.AddNode(ExplainCommand.ValueMarkup(
                "Converter", $"\"{parameter.Converter.ConverterTypeName}\""));
        }

        if (parameter.DefaultValue != null)
        {
            parameterNode.AddNode(ExplainCommand.ValueMarkup(
                "Default Value", $"\"{parameter.DefaultValue.Value}\""));
        }

        if (parameter.PairDeconstructor != null)
        {
            parameterNode.AddNode(ExplainCommand.ValueMarkup("Pair Deconstructor", parameter.PairDeconstructor.Type.ToString()));
        }

        ExplainCommand.AddStringList(
            parameterNode,
            "Validators",
            parameter.Validators.ConvertAll(i => i.GetType().ToString()));
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