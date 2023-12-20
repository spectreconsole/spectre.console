namespace Spectre.Console.Cli.Help;

/// <summary>
/// The help provider for Spectre.Console.
/// </summary>
/// <remarks>
/// Other IHelpProvider implementations can be injected into the CommandApp, if desired.
/// </remarks>
public class HelpProvider : IHelpProvider
{
    private readonly HelpProviderResources resources;

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

        var parameters = new List<Composer>();

        if (command == null)
        {
            parameters.Add(new Composer().Style("grey", $"[{resources.Options}]"));
            parameters.Add(new Composer().Style("aqua", $"<{resources.Command}>"));
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
                        parameters.Add(new Composer().Style("underline", $"{current.Name}"));
                    }
                    else
                    {
                        parameters.Add(new Composer().Text(current.Name));
                    }
                }

                if (current.Parameters.OfType<ICommandArgument>().Any())
                {
                    if (isCurrent)
                    {
                        foreach (var argument in current.Parameters.OfType<ICommandArgument>()
                            .Where(a => a.Required).OrderBy(a => a.Position).ToArray())
                        {
                            parameters.Add(new Composer().Style("aqua", $"<{argument.Value}>"));
                        }
                    }

                    var optionalArguments = current.Parameters.OfType<ICommandArgument>().Where(x => !x.Required).ToArray();
                    if (optionalArguments.Length > 0 || !isCurrent)
                    {
                        foreach (var optionalArgument in optionalArguments)
                        {
                            parameters.Add(new Composer().Style("silver", $"[{optionalArgument.Value}]"));
                        }
                    }
                }

                if (isCurrent)
                {
                    parameters.Add(new Composer().Style("grey", $"[{resources.Options}]"));
                }
            }

            if (command.IsBranch && command.DefaultCommand == null)
            {
                // The user must specify the command
                parameters.Add(new Composer().Style("aqua", $"<{resources.Command}>"));
            }
            else if (command.IsBranch && command.DefaultCommand != null && command.Commands.Count > 0)
            {
                // We are on a branch with a default command
                // The user can optionally specify the command
                parameters.Add(new Composer().Style("aqua", $"[{resources.Command}]"));
            }
            else if (command.IsDefaultCommand)
            {
                var commands = model.Commands.Where(x => !x.IsHidden && !x.IsDefaultCommand).ToList();

                if (commands.Count > 0)
                {
                    // Commands other than the default are present
                    // So make these optional in the usage statement
                    parameters.Add(new Composer().Style("aqua", $"[{resources.Command}]"));
                }
            }
        }

        composer.Join(" ", parameters);
        composer.LineBreak();

        return new[] { composer };
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
            new Composer().LineBreak().Style("yellow", $"{resources.Arguments}:").LineBreak(),
        };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var argument in arguments.Where(x => x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                new Composer().Style("silver", $"<{argument.Name}>"),
                new Composer().Text(argument.Description?.TrimEnd('.') ?? " "));
        }

        foreach (var argument in arguments.Where(x => !x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                new Composer().Style("silver", $"[{argument.Name}]"),
                new Composer().Text(argument.Description?.TrimEnd('.') ?? " "));
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
            new Composer().LineBreak().Style("yellow", $"{resources.Options}:").LineBreak(),
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

        if (defaultValueColumn)
        {
            grid.AddRow(
                new Composer().Space(),
                new Composer().Style("lime", resources.Default),
                new Composer().Space());
        }

        foreach (var option in helpOptions)
        {
            var columns = new List<IRenderable>() { GetOptionParts(option) };

            if (defaultValueColumn)
            {
                columns.Add(GetOptionDefaultValue(option.DefaultValue));
            }

            columns.Add(new Composer().Text(option.Description?.TrimEnd('.') ?? " "));

            grid.AddRow(columns.ToArray());
        }

        result.Add(grid);

        return result;
    }

    private IRenderable GetOptionParts(HelpOption option)
    {
        var composer = new Composer();

        if (option.Short != null)
        {
            composer.Text("-").Text(option.Short);
            if (option.Long != null)
            {
                composer.Text(", ");
            }
        }
        else
        {
            composer.Text("  ");
            if (option.Long != null)
            {
                composer.Text("  ");
            }
        }

        if (option.Long != null)
        {
            composer.Text("--").Text(option.Long);
        }

        if (option.Value != null)
        {
            composer.Text(" ");
            if (option.ValueIsOptional ?? false)
            {
                composer.Style("grey", $"[{option.Value}]");
            }
            else
            {
                composer.Style("silver", $"<{option.Value}>");
            }
        }

        return composer;
    }

    private IRenderable GetOptionDefaultValue(object? defaultValue)
    {
        return defaultValue switch
        {
            null => new Composer().Text(" "),
            "" => new Composer().Text(" "),
            Array { Length: 0 } => new Composer().Text(" "),
            Array array => new Composer().Join(", ", array.Cast<object>().Select(o => new Composer().Style("bold", o.ToString() ?? string.Empty))),
            _ => new Composer().Style("bold", defaultValue?.ToString() ?? string.Empty),
        };
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
            new Composer().LineBreak().Style("yellow", $"{resources.Commands}:").LineBreak(),
        };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var child in commands)
        {
            var arguments = new Composer();
            arguments.Style("silver", child.Name);
            arguments.Space();

            foreach (var argument in HelpArgument.Get(child).Where(a => a.Required))
            {
                arguments.Style("silver", $"<{argument.Name}>");
                arguments.Space();
            }

            if (TrimTrailingPeriod)
            {
                grid.AddRow(
                    new Composer().Text(arguments.ToString().TrimEnd()),
                    new Composer().Text(child.Description?.TrimEnd('.') ?? " "));
            }
            else
            {
                grid.AddRow(
                    new Composer().Text(arguments.ToString().TrimEnd()),
                    new Composer().Text(child.Description ?? " "));
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