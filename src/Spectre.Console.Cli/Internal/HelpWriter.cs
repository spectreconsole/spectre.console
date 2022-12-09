namespace Spectre.Console.Cli;

internal static class HelpWriter
{
    private sealed class HelpArgument
    {
        public string Name { get; }
        public int Position { get; set; }
        public bool Required { get; }
        public string? Description { get; }

        public HelpArgument(string name, int position, bool required, string? description)
        {
            Name = name;
            Position = position;
            Required = required;
            Description = description;
        }

        public static IReadOnlyList<HelpArgument> Get(CommandInfo? command)
        {
            var arguments = new List<HelpArgument>();
            arguments.AddRange(command?.Parameters?.OfType<CommandArgument>()?.Select(
                x => new HelpArgument(x.Value, x.Position, x.Required, x.Description))
                ?? Array.Empty<HelpArgument>());
            return arguments;
        }
    }

    private sealed class HelpOption
    {
        public string? Short { get; }
        public string? Long { get; }
        public string? Value { get; }
        public bool? ValueIsOptional { get; }
        public string? Description { get; }

        public HelpOption(string? @short, string? @long, string? @value, bool? valueIsOptional, string? description)
        {
            Short = @short;
            Long = @long;
            Value = value;
            ValueIsOptional = valueIsOptional;
            Description = description;
        }

        public static IReadOnlyList<HelpOption> Get(CommandModel model, CommandInfo? command)
        {
            var parameters = new List<HelpOption>();
            parameters.Add(new HelpOption("h", "help", null, null, "Prints help information"));

            // At the root and no default command?
            if (command == null && model?.DefaultCommand == null)
            {
                parameters.Add(new HelpOption("v", "version", null, null, "Prints version information"));
            }

            parameters.AddRange(command?.Parameters.OfType<CommandOption>().Where(o => !o.IsHidden).Select(o =>
                new HelpOption(
                    o.ShortNames.FirstOrDefault(), o.LongNames.FirstOrDefault(),
                    o.ValueName, o.ValueIsOptional, o.Description))
                ?? Array.Empty<HelpOption>());
            return parameters;
        }
    }

    public static IEnumerable<IRenderable> Write(CommandModel model)
    {
        return WriteCommand(model, null);
    }

    public static IEnumerable<IRenderable> WriteCommand(CommandModel model, CommandInfo? command)
    {
        var container = command as ICommandContainer ?? model;
        var isDefaultCommand = command?.IsDefaultCommand ?? false;

        var result = new List<IRenderable>();
        result.AddRange(GetDescription(command));
        result.AddRange(GetUsage(model, command));
        result.AddRange(GetExamples(model, command));
        result.AddRange(GetArguments(command));
        result.AddRange(GetOptions(model, command));
        result.AddRange(GetCommands(model, container, isDefaultCommand));

        return result;
    }

    private static IEnumerable<IRenderable> GetDescription(CommandInfo? command)
    {
        if (command?.Description == null)
        {
            yield break;
        }

        var composer = new Composer();
        composer.Style("yellow", "DESCRIPTION:").LineBreak();
        composer.Text(command.Description).LineBreak();
        yield return composer.LineBreak();
    }

    private static IEnumerable<IRenderable> GetUsage(CommandModel model, CommandInfo? command)
    {
        var composer = new Composer();
        composer.Style("yellow", "USAGE:").LineBreak();
        composer.Tab().Text(model.GetApplicationName());

        var parameters = new List<string>();

        if (command == null)
        {
            parameters.Add("[grey][[OPTIONS]][/]");
            parameters.Add("[aqua]<COMMAND>[/]");
        }
        else
        {
            foreach (var current in command.Flatten())
            {
                var isCurrent = current == command;

                if (!current.IsDefaultCommand)
                {
                    if (isCurrent)
                    {
                        parameters.Add($"[underline]{current.Name.EscapeMarkup()}[/]");
                    }
                    else
                    {
                        parameters.Add($"{current.Name.EscapeMarkup()}");
                    }
                }

                if (current.Parameters.OfType<CommandArgument>().Any())
                {
                    if (isCurrent)
                    {
                        foreach (var argument in current.Parameters.OfType<CommandArgument>()
                            .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                        {
                            parameters.Add($"[aqua]<{argument.Value.EscapeMarkup()}>[/]");
                        }
                    }

                    var optionalArguments = current.Parameters.OfType<CommandArgument>().Where(x => !x.Required).ToArray();
                    if (optionalArguments.Length > 0 || !isCurrent)
                    {
                        foreach (var optionalArgument in optionalArguments)
                        {
                            parameters.Add($"[silver][[{optionalArgument.Value.EscapeMarkup()}]][/]");
                        }
                    }
                }

                if (isCurrent)
                {
                    parameters.Add("[grey][[OPTIONS]][/]");
                }
            }

            if (command.IsBranch)
            {
                parameters.Add("[aqua]<COMMAND>[/]");
            }
        }

        composer.Join(" ", parameters);
        composer.LineBreak();

        return new[]
        {
            composer,
        };
    }

    private static IEnumerable<IRenderable> GetExamples(CommandModel model, CommandInfo? command)
    {
        var maxExamples = int.MaxValue;

        var examples = command?.Examples ?? model.Examples ?? new List<string[]>();
        if (examples.Count == 0)
        {
            // Since we're not checking direct examples,
            // make sure that we limit the number of examples.
            maxExamples = 5;

            // Get the current root command.
            var root = command ?? (ICommandContainer)model;
            var queue = new Queue<ICommandContainer>(new[] { root });

            // Traverse the command tree and look for examples.
            // As soon as a node contains commands, bail.
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var cmd in current.Commands.Where(x => !x.IsHidden))
                {
                    if (cmd.Examples.Count > 0)
                    {
                        examples.AddRange(cmd.Examples);
                    }

                    queue.Enqueue(cmd);
                }

                if (examples.Count >= maxExamples)
                {
                    break;
                }
            }
        }

        if (examples.Count > 0)
        {
            var composer = new Composer();
            composer.LineBreak();
            composer.Style("yellow", "EXAMPLES:").LineBreak();

            for (var index = 0; index < Math.Min(maxExamples, examples.Count); index++)
            {
                var args = string.Join(" ", examples[index]);
                composer.Tab().Text(model.GetApplicationName()).Space().Style("grey", args);
                composer.LineBreak();
            }

            return new[] { composer };
        }

        return Array.Empty<IRenderable>();
    }

    private static IEnumerable<IRenderable> GetArguments(CommandInfo? command)
    {
        var arguments = HelpArgument.Get(command);
        if (arguments.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
            {
                new Markup(Environment.NewLine),
                new Markup("[yellow]ARGUMENTS:[/]"),
                new Markup(Environment.NewLine),
            };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var argument in arguments.Where(x => x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                $"[silver]<{argument.Name.EscapeMarkup()}>[/]",
                argument.Description?.TrimEnd('.') ?? " ");
        }

        foreach (var argument in arguments.Where(x => !x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                $"[grey][[{argument.Name.EscapeMarkup()}]][/]",
                argument.Description?.TrimEnd('.') ?? " ");
        }

        result.Add(grid);

        return result;
    }

    private static IEnumerable<IRenderable> GetOptions(CommandModel model, CommandInfo? command)
    {
        // Collect all options into a single structure.
        var parameters = HelpOption.Get(model, command);
        if (parameters.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
            {
                new Markup(Environment.NewLine),
                new Markup("[yellow]OPTIONS:[/]"),
                new Markup(Environment.NewLine),
            };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        static string GetOptionParts(HelpOption option)
        {
            var builder = new StringBuilder();
            if (option.Short != null)
            {
                builder.Append('-').Append(option.Short.EscapeMarkup());
                if (option.Long != null)
                {
                    builder.Append(", ");
                }
            }
            else
            {
                builder.Append("  ");
                if (option.Long != null)
                {
                    builder.Append("  ");
                }
            }

            if (option.Long != null)
            {
                builder.Append("--").Append(option.Long.EscapeMarkup());
            }

            if (option.Value != null)
            {
                builder.Append(' ');
                if (option.ValueIsOptional ?? false)
                {
                    builder.Append("[grey][[").Append(option.Value.EscapeMarkup()).Append("]][/]");
                }
                else
                {
                    builder.Append("[silver]<").Append(option.Value.EscapeMarkup()).Append(">[/]");
                }
            }

            return builder.ToString();
        }

        foreach (var option in parameters.ToArray())
        {
            grid.AddRow(
                GetOptionParts(option),
                option.Description?.TrimEnd('.') ?? " ");
        }

        result.Add(grid);

        return result;
    }

    private static IEnumerable<IRenderable> GetCommands(
        CommandModel model,
        ICommandContainer command,
        bool isDefaultCommand)
    {
        var commands = isDefaultCommand ? model.Commands : command.Commands;
        commands = commands.Where(x => !x.IsHidden).ToList();

        if (commands.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
            {
                new Markup(Environment.NewLine),
                new Markup("[yellow]COMMANDS:[/]"),
                new Markup(Environment.NewLine),
            };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var child in commands)
        {
            var arguments = new Composer();
            arguments.Style("silver", child.Name.EscapeMarkup());
            arguments.Space();

            foreach (var argument in HelpArgument.Get(child).Where(a => a.Required))
            {
                arguments.Style("silver", $"<{argument.Name.EscapeMarkup()}>");
                arguments.Space();
            }

            if (model.TrimTrailingPeriod)
            {
                grid.AddRow(
                    arguments.ToString().TrimEnd(),
                    child.Description?.TrimEnd('.') ?? " ");
            }
            else
            {
                grid.AddRow(
                    arguments.ToString().TrimEnd(),
                    child.Description ?? " ");
            }
        }

        result.Add(grid);

        return result;
    }
}