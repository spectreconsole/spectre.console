namespace Spectre.Console.Cli.Help;

/// <summary>
/// The default help provider for spectre.console.
/// </summary>
/// <remarks>
/// Other IHelpProvider implementations can be injected into the CommandApp, if desired.
/// </remarks>
public class DefaultHelpProvider : IHelpProvider
{
    // Help provider configuration settings.
    private readonly bool writeOptionsDefaultValues;
    private readonly int maxIndirectExamples;
    private readonly bool trimTrailingPeriod;

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

        public static IReadOnlyList<HelpArgument> Get(ICommandInfo? command)
        {
            var arguments = new List<HelpArgument>();
            arguments.AddRange(command?.Parameters?.OfType<ICommandArgument>()?.Select(
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
        public object? DefaultValue { get; }

        public HelpOption(string? @short, string? @long, string? @value, bool? valueIsOptional, string? description, object? defaultValue)
        {
            Short = @short;
            Long = @long;
            Value = value;
            ValueIsOptional = valueIsOptional;
            Description = description;
            DefaultValue = defaultValue;
        }

        public static IReadOnlyList<HelpOption> Get(ICommandInfo? command)
        {
            var parameters = new List<HelpOption>();
            parameters.Add(new HelpOption("h", "help", null, null, "Prints help information", null));

            // At the root?
            if ((command == null || command?.Parent == null) && !(command?.IsBranch ?? false))
            {
                parameters.Add(new HelpOption("v", "version", null, null, "Prints version information", null));
            }

            parameters.AddRange(command?.Parameters.OfType<ICommandOption>().Where(o => !o.IsHidden).Select(o =>
                new HelpOption(
                    o.ShortNames.FirstOrDefault(), o.LongNames.FirstOrDefault(),
                    o.ValueName, o.ValueIsOptional, o.Description,
                    o.IsFlag && o.DefaultValue?.Value is false ? null : o.DefaultValue?.Value))
                ?? Array.Empty<HelpOption>());
            return parameters;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultHelpProvider"/> class.
    /// </summary>
    /// <param name="writeOptionsDefaultValue">A boolean value indicating whether to write option default values.</param>
    /// <param name="maxIndirectExamples">The maximum number of indirect examples to display.</param>
    /// <param name="trimTrailingPeriod">A boolean value indicating whether to trim trailing periods from command descriptions.</param>
    public DefaultHelpProvider(bool writeOptionsDefaultValue, int maxIndirectExamples, bool trimTrailingPeriod)
    {
        this.writeOptionsDefaultValues = writeOptionsDefaultValue;
        this.maxIndirectExamples = maxIndirectExamples;
        this.trimTrailingPeriod = trimTrailingPeriod;
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IRenderable> Write(ICommandModel model)
    {
        return WriteCommand(model, null);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IRenderable> WriteCommand(ICommandModel model, ICommandInfo? command)
    {
        var result = new List<IRenderable>();

        result.AddRange(GetDescription(command));
        result.AddRange(GetUsage(model, command));
        result.AddRange(GetExamples(model, command, maxIndirectExamples));
        result.AddRange(GetArguments(command));
        result.AddRange(GetOptions(command, writeOptionsDefaultValues));
        result.AddRange(GetCommands(model, command, trimTrailingPeriod));

        return result;
    }

    private static IEnumerable<IRenderable> GetDescription(ICommandInfo? command)
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

    private static IEnumerable<IRenderable> GetUsage(ICommandModel model, ICommandInfo? command)
    {
        var composer = new Composer();
        composer.Style("yellow", "USAGE:").LineBreak();
        composer.Tab().Text(model.ApplicationName);

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

                if (current.Parameters.OfType<ICommandArgument>().Any())
                {
                    if (isCurrent)
                    {
                        foreach (var argument in current.Parameters.OfType<ICommandArgument>()
                            .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                        {
                            parameters.Add($"[aqua]<{argument.Value.EscapeMarkup()}>[/]");
                        }
                    }

                    var optionalArguments = current.Parameters.OfType<ICommandArgument>().Where(x => !x.Required).ToArray();
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

            if (command.IsBranch && command.DefaultCommand == null)
            {
                // The user must specify the command
                parameters.Add("[aqua]<COMMAND>[/]");
            }
            else if (command.IsBranch && command.DefaultCommand != null && command.Commands.Count > 0)
            {
                // We are on a branch with a default commnd
                // The user can optionally specify the command
                parameters.Add("[aqua][[COMMAND]][/]");
            }
            else if (command.IsDefaultCommand)
            {
                var commands = model.Commands.Where(x => !x.IsHidden && !x.IsDefaultCommand).ToList();

                if (commands.Count > 0)
                {
                    // Commands other than the default are present
                    // So make these optional in the usage statement
                    parameters.Add("[aqua][[COMMAND]][/]");
                }
            }
        }

        composer.Join(" ", parameters);
        composer.LineBreak();

        return new[]
        {
            composer,
        };
    }

    /// <summary>
    /// Gets the examples for a command.
    /// </summary>
    /// <remarks>
    /// Examples from the command's direct children are used
    /// if no examples have been set on the specified command or model.
    /// </remarks>
    private static IEnumerable<IRenderable> GetExamples(ICommandModel model, ICommandInfo? command, int maxIndirectExamples)
    {
        var maxExamples = int.MaxValue;

        var examples = command?.Examples ?? model.Examples ?? new List<string[]>();
        if (examples.Count == 0)
        {
            // Since we're not checking direct examples,
            // make sure that we limit the number of examples.
            maxExamples = maxIndirectExamples;

            // Start at the current command (if exists)
            // or alternatively commence at the model.
            var commandContainer = command ?? (ICommandContainer)model;
            var queue = new Queue<ICommandContainer>(new[] { commandContainer });

            // Traverse the command tree and look for examples.
            // As soon as a node contains commands, bail.
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                foreach (var child in current.Commands.Where(x => !x.IsHidden))
                {
                    if (child.Examples.Count > 0)
                    {
                        examples.AddRange(child.Examples);
                    }

                    queue.Enqueue(child);
                }

                if (examples.Count >= maxExamples)
                {
                    break;
                }
            }
        }

        if (Math.Min(maxExamples, examples.Count) > 0)
        {
            var composer = new Composer();
            composer.LineBreak();
            composer.Style("yellow", "EXAMPLES:").LineBreak();

            for (var index = 0; index < Math.Min(maxExamples, examples.Count); index++)
            {
                var args = string.Join(" ", examples[index]);
                composer.Tab().Text(model.ApplicationName).Space().Style("grey", args);
                composer.LineBreak();
            }

            return new[] { composer };
        }

        return Array.Empty<IRenderable>();
    }

    private static IEnumerable<IRenderable> GetArguments(ICommandInfo? command)
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

    private static IEnumerable<IRenderable> GetOptions(ICommandInfo? command, bool writeOptionsDefaultValues)
    {
        // Collect all options into a single structure.
        var parameters = HelpOption.Get(command);
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

        var helpOptions = parameters.ToArray();
        var defaultValueColumn = writeOptionsDefaultValues && helpOptions.Any(e => e.DefaultValue != null);

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        if (defaultValueColumn)
        {
            grid.AddColumn(new GridColumn { Padding = new Padding(0, 0, 4, 0) });
        }

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

        if (defaultValueColumn)
        {
            grid.AddRow(" ", "[lime]DEFAULT[/]", " ");
        }

        foreach (var option in helpOptions)
        {
            var columns = new List<string> { GetOptionParts(option) };
            if (defaultValueColumn)
            {
                static string Bold(object obj) => $"[bold]{obj.ToString().EscapeMarkup()}[/]";

                var defaultValue = option.DefaultValue switch
                {
                    null => " ",
                    "" => " ",
                    Array { Length: 0 } => " ",
                    Array array => string.Join(", ", array.Cast<object>().Select(Bold)),
                    _ => Bold(option.DefaultValue),
                };
                columns.Add(defaultValue);
            }

            columns.Add(option.Description?.TrimEnd('.') ?? " ");

            grid.AddRow(columns.ToArray());
        }

        result.Add(grid);

        return result;
    }

    private static IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command, bool trimTrailingPeriod)
    {
        var commandContainer = command ?? (ICommandContainer)model;
        bool isDefaultCommand = command?.IsDefaultCommand ?? false;

        var commands = isDefaultCommand ? model.Commands : commandContainer.Commands;
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

            if (trimTrailingPeriod)
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
