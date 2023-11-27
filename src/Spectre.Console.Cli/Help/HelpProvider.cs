using Spectre.Console.Cli.Resources;

namespace Spectre.Console.Cli.Help;

/// <summary>
/// The help provider for Spectre.Console.
/// </summary>
/// <remarks>
/// Other IHelpProvider implementations can be injected into the CommandApp, if desired.
/// </remarks>
public class HelpProvider : IHelpProvider
{
    private HelpProviderResources resources;

    /// <summary>
    /// Gets a value indicating how many examples from direct children to show in the help text.
    /// </summary>
    protected virtual int MaximumIndirectExamples { get; }

    /// <summary>
    /// Gets a value indicating whether any default values for command options are shown in the help text.
    /// </summary>
    protected virtual bool ShowOptionDefaultValues { get; }

    /// <summary>
    /// Gets a value indicating whether a trailing period of a command description is trimmed in the help text.
    /// </summary>
    protected virtual bool TrimTrailingPeriod { get; }

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

        public static IReadOnlyList<HelpOption> Get(ICommandInfo? command, HelpProviderResources resources)
        {
            var parameters = new List<HelpOption>();
            parameters.Add(new HelpOption("h", "help", null, null, resources.PrintHelpDescription, null));

            // Version information applies to the entire application
            // Include the "-v" option in the help when at the root of the command line application
            // Don't allow the "-v" option if users have specified one or more sub-commands
            if ((command == null || command?.Parent == null) && !(command?.IsBranch ?? false))
            {
                parameters.Add(new HelpOption("v", "version", null, null, resources.PrintVersionDescription, null));
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
    /// Initializes a new instance of the <see cref="HelpProvider"/> class.
    /// </summary>
    /// <param name="settings">The command line application settings used for configuration.</param>
    public HelpProvider(ICommandAppSettings settings)
    {
        this.ShowOptionDefaultValues = settings.ShowOptionDefaultValues;
        this.MaximumIndirectExamples = settings.MaximumIndirectExamples;
        this.TrimTrailingPeriod = settings.TrimTrailingPeriod;

        resources = new HelpProviderResources(settings.Culture);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IRenderable> Write(ICommandModel model, ICommandInfo? command)
    {
        var result = new List<IRenderable>();

        result.AddRange(GetHeader(model, command));
        result.AddRange(GetDescription(model, command));
        result.AddRange(GetUsage(model, command));
        result.AddRange(GetExamples(model, command));
        result.AddRange(GetArguments(model, command));
        result.AddRange(GetOptions(model, command));
        result.AddRange(GetCommands(model, command));
        result.AddRange(GetFooter(model, command));

        return result;
    }

    /// <summary>
    /// Gets the header for the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetHeader(ICommandModel model, ICommandInfo? command)
    {
        yield break;
    }

    /// <summary>
    /// Gets the description section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetDescription(ICommandModel model, ICommandInfo? command)
    {
        if (command?.Description == null)
        {
            yield break;
        }

        var composer = new Composer();
        composer.Style("yellow", $"{resources.Description}:").LineBreak();
        composer.Text(command.Description).LineBreak();
        yield return composer.LineBreak();
    }

    /// <summary>
    /// Gets the usage section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetUsage(ICommandModel model, ICommandInfo? command)
    {
        var composer = new Composer();
        composer.Style("yellow", $"{resources.Usage}:").LineBreak();
        composer.Tab().Text(model.ApplicationName);

        var parameters = new List<string>();

        if (command == null)
        {
            parameters.Add($"[grey][[{resources.Options}]][/]");
            parameters.Add($"[aqua]<{resources.Command}>[/]");
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
                    parameters.Add($"[grey][[{resources.Options}]][/]");
                }
            }

            if (command.IsBranch && command.DefaultCommand == null)
            {
                // The user must specify the command
                parameters.Add($"[aqua]<{resources.Command}>[/]");
            }
            else if (command.IsBranch && command.DefaultCommand != null && command.Commands.Count > 0)
            {
                // We are on a branch with a default command
                // The user can optionally specify the command
                parameters.Add($"[aqua][[{resources.Command}]][/]");
            }
            else if (command.IsDefaultCommand)
            {
                var commands = model.Commands.Where(x => !x.IsHidden && !x.IsDefaultCommand).ToList();

                if (commands.Count > 0)
                {
                    // Commands other than the default are present
                    // So make these optional in the usage statement
                    parameters.Add($"[aqua][[{resources.Command}]][/]");
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
    /// Gets the examples section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    /// <remarks>
    /// Examples from the command's direct children are used
    /// if no examples have been set on the specified command or model.
    /// </remarks>
    public virtual IEnumerable<IRenderable> GetExamples(ICommandModel model, ICommandInfo? command)
    {
        var maxExamples = int.MaxValue;

        var examples = command?.Examples?.ToList() ?? model.Examples?.ToList() ?? new List<string[]>();
        if (examples.Count == 0)
        {
            // Since we're not checking direct examples,
            // make sure that we limit the number of examples.
            maxExamples = MaximumIndirectExamples;

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
            composer.Style("yellow", $"{resources.Examples}:").LineBreak();

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

    /// <summary>
    /// Gets the arguments section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetArguments(ICommandModel model, ICommandInfo? command)
    {
        var arguments = HelpArgument.Get(command);
        if (arguments.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
            {
                new Markup(Environment.NewLine),
                new Markup($"[yellow]{resources.Arguments}:[/]"),
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

    /// <summary>
    /// Gets the options section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetOptions(ICommandModel model, ICommandInfo? command)
    {
        // Collect all options into a single structure.
        var parameters = HelpOption.Get(command, resources);
        if (parameters.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
            {
                new Markup(Environment.NewLine),
                new Markup($"[yellow]{resources.Options}:[/]"),
                new Markup(Environment.NewLine),
            };

        var helpOptions = parameters.ToArray();
        var defaultValueColumn = ShowOptionDefaultValues && helpOptions.Any(e => e.DefaultValue != null);

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
            grid.AddRow(" ", $"[lime]{resources.Default}[/]", " ");
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

    /// <summary>
    /// Gets the commands section of the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetCommands(ICommandModel model, ICommandInfo? command)
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
                new Markup($"[yellow]{resources.Commands}:[/]"),
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

            if (TrimTrailingPeriod)
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

    /// <summary>
    /// Gets the footer for the help information.
    /// </summary>
    /// <param name="model">The command model to write help for.</param>
    /// <param name="command">The command for which to write help information (optional).</param>
    /// <returns>An enumerable collection of <see cref="IRenderable"/> objects.</returns>
    public virtual IEnumerable<IRenderable> GetFooter(ICommandModel model, ICommandInfo? command)
    {
        yield break;
    }
}