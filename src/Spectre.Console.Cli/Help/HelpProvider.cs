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
    private readonly HelpProviderStyle? helpStyles;

    /// <summary>
    /// Gets a value indicating how many examples from direct children to show in the help text.
    /// </summary>
    protected virtual int MaximumIndirectExamples { get; }

    /// <summary>
    /// Gets a value indicating whether any default values for command options are shown in the help text.
    /// </summary>
    protected virtual bool ShowOptionDefaultValues { get; }

    /// <summary>
    /// Gets a value indicating whether a trailing period of a description is trimmed in the help text.
    /// </summary>
    protected virtual bool TrimTrailingPeriod { get; }

    /// <summary>
    /// Gets a value indicating whether to emit the markup styles, inline, when rendering the help text.
    /// </summary>
    /// <remarks>
    /// Useful for unit testing different styling of the same help text.
    /// </remarks>
    protected virtual bool RenderMarkupInline { get; } = false;

    private sealed class HelpArgument
    {
        public string Name { get; }
        public int Position { get; set; }
        public bool Required { get; }
        public string? Description { get; }

        private HelpArgument(string name, int position, bool required, string? description)
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
                                   x => new HelpArgument(x.Value, x.Position, x.IsRequired, x.Description))
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
        public bool IsRequired { get; }
        public string? Description { get; }
        public object? DefaultValue { get; }

        private HelpOption(
            string? @short, string? @long, string? @value,
            bool? valueIsOptional, bool isRequired,
            string? description, object? defaultValue)
        {
            Short = @short;
            Long = @long;
            Value = value;
            ValueIsOptional = valueIsOptional;
            IsRequired = isRequired;
            Description = description;
            DefaultValue = defaultValue;
        }

        public static IReadOnlyList<HelpOption> Get(
            ICommandModel model,
            ICommandInfo? command,
            HelpProviderResources resources)
        {
            var parameters = new List<HelpOption>
            {
                new HelpOption("h", "help", null, null, false,
                    resources.PrintHelpDescription, null),
            };

            // Version information applies to the entire CLI application.
            // Whether to show the "-v|--version" option in the help is determined as per:
            // - If an application version has been set, and
            // -- When at the root of the application, or
            // -- When at the root of the application with a default command, unless
            // --- The default command has a version option in its settings
            if ((command?.Parent == null) && !(command?.IsBranch ?? false) && (command?.IsDefaultCommand ?? true))
            {
                // Check whether the default command has a version option in its settings.
                var versionCommandOption = command?.Parameters?.OfType<CommandOption>()?.FirstOrDefault(o =>
                    (o.ShortNames.FirstOrDefault(v => v.Equals("v", StringComparison.OrdinalIgnoreCase)) != null) ||
                    (o.LongNames.FirstOrDefault(v => v.Equals("version", StringComparison.OrdinalIgnoreCase)) != null));

                // Only show the version option if the default command doesn't have a version option in its settings.
                if (versionCommandOption == null)
                {
                    // Only show the version option if there is an application version set.
                    if (model.ApplicationVersion != null)
                    {
                        parameters.Add(new HelpOption("v", "version", null, null, false,
                            resources.PrintVersionDescription, null));
                    }
                }
            }

            parameters.AddRange(command?.Parameters.OfType<ICommandOption>().Where(o => !o.IsHidden).Select(o =>
                                    new HelpOption(
                                        o.ShortNames.FirstOrDefault(), o.LongNames.FirstOrDefault(),
                                        o.ValueName, o.ValueIsOptional, o.IsRequired, o.Description,
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

        // Don't provide a default style if HelpProviderStyles is null,
        // as the user will have explicitly done this to output unstyled help text
        this.helpStyles = settings.HelpProviderStyles;

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

        var composer = NewComposer();
        composer.Style(helpStyles?.Description?.Header ?? Style.Plain, $"{resources.Description}:").LineBreak();
        composer.Text(NormalizeDescription(command.Description)).LineBreak();
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
        var composer = NewComposer();
        composer.Style(helpStyles?.Usage?.Header ?? Style.Plain, $"{resources.Usage}:").LineBreak();
        composer.Tab().Text(model.ApplicationName);

        var parameters = new List<Composer>();

        if (command == null)
        {
            parameters.Add(NewComposer().Style(helpStyles?.Usage?.Options ?? Style.Plain, $"[{resources.Options}]"));
            parameters.Add(NewComposer().Style(helpStyles?.Usage?.Command ?? Style.Plain, $"<{resources.Command}>"));
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
                        parameters.Add(NewComposer().Style(
                            helpStyles?.Usage?.CurrentCommand ?? Style.Plain,
                            $"{current.Name}"));
                    }
                    else
                    {
                        parameters.Add(NewComposer().Text(current.Name));
                    }
                }

                if (current.Parameters.OfType<ICommandArgument>().Any())
                {
                    if (isCurrent)
                    {
                        foreach (var argument in current.Parameters.OfType<ICommandArgument>()
                                     .Where(a => a.IsRequired).OrderBy(a => a.Position).ToArray())
                        {
                            parameters.Add(NewComposer().Style(
                                helpStyles?.Usage?.RequiredArgument ?? Style.Plain,
                                $"<{argument.Value}>"));
                        }
                    }

                    var optionalArguments = current.Parameters.OfType<ICommandArgument>().Where(x => !x.IsRequired)
                        .ToArray();
                    if (optionalArguments.Length > 0 || !isCurrent)
                    {
                        foreach (var optionalArgument in optionalArguments)
                        {
                            parameters.Add(NewComposer().Style(
                                helpStyles?.Usage?.OptionalArgument ?? Style.Plain,
                                $"[{optionalArgument.Value}]"));
                        }
                    }
                }

                if (isCurrent)
                {
                    parameters.Add(NewComposer()
                        .Style(helpStyles?.Usage?.Options ?? Style.Plain, $"[{resources.Options}]"));
                }
            }

            if (command.IsBranch && command.DefaultCommand == null)
            {
                // The user must specify the command
                parameters.Add(NewComposer()
                    .Style(helpStyles?.Usage?.Command ?? Style.Plain, $"<{resources.Command}>"));
            }
            else if (command.IsBranch && command.DefaultCommand != null && command.Commands.Count > 0)
            {
                // We are on a branch with a default command
                // The user can optionally specify the command
                parameters.Add(NewComposer()
                    .Style(helpStyles?.Usage?.Command ?? Style.Plain, $"[{resources.Command}]"));
            }
            else if (command.IsDefaultCommand)
            {
                var commands = model.Commands.Where(x => !x.IsHidden && !x.IsDefaultCommand).ToList();

                if (commands.Count > 0)
                {
                    // Commands other than the default are present
                    // So make these optional in the usage statement
                    parameters.Add(NewComposer()
                        .Style(helpStyles?.Usage?.Command ?? Style.Plain, $"[{resources.Command}]"));
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
            var composer = NewComposer();
            composer.LineBreak();
            composer.Style(helpStyles?.Examples?.Header ?? Style.Plain, $"{resources.Examples}:").LineBreak();

            for (var index = 0; index < Math.Min(maxExamples, examples.Count); index++)
            {
                var args = string.Join(" ", examples[index]);
                composer.Tab().Text(model.ApplicationName).Space()
                    .Style(helpStyles?.Examples?.Arguments ?? Style.Plain, args);
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
            NewComposer().LineBreak().Style(helpStyles?.Arguments?.Header ?? Style.Plain, $"{resources.Arguments}:")
                .LineBreak(),
        };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var argument in arguments.Where(x => x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                NewComposer().Style(helpStyles?.Arguments?.RequiredArgument ?? Style.Plain, $"<{argument.Name}>"),
                NewComposer().Text(NormalizeDescription(argument.Description)));
        }

        foreach (var argument in arguments.Where(x => !x.Required).OrderBy(x => x.Position))
        {
            grid.AddRow(
                NewComposer().Style(helpStyles?.Arguments?.OptionalArgument ?? Style.Plain, $"[{argument.Name}]"),
                NewComposer().Text(NormalizeDescription(argument.Description)));
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
        var parameters = HelpOption.Get(model, command, resources);
        if (parameters.Count == 0)
        {
            return Array.Empty<IRenderable>();
        }

        var result = new List<IRenderable>
        {
            NewComposer().LineBreak().Style(helpStyles?.Options?.Header ?? Style.Plain, $"{resources.Options}:")
                .LineBreak(),
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
                NewComposer().Space(),
                NewComposer().Style(helpStyles?.Options?.DefaultValueHeader ?? Style.Plain, resources.Default),
                NewComposer().Space());
        }

        foreach (var option in helpOptions)
        {
            var columns = new List<IRenderable>() { GetOptionParts(option) };

            if (defaultValueColumn)
            {
                columns.Add(GetDefaultValueForOption(option.DefaultValue));
            }

            var description = option.Description;
            if (option.IsRequired)
            {
                description = string.IsNullOrWhiteSpace(description)
                    ? "[i]Required[/]"
                    : description.TrimEnd('.') + ". [i]Required[/]";
            }

            columns.Add(NewComposer().Text(NormalizeDescription(description)));

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
            NewComposer().LineBreak().Style(helpStyles?.Commands?.Header ?? Style.Plain, $"{resources.Commands}:")
                .LineBreak(),
        };

        var grid = new Grid();
        grid.AddColumn(new GridColumn { Padding = new Padding(4, 4), NoWrap = true });
        grid.AddColumn(new GridColumn { Padding = new Padding(0, 0) });

        foreach (var child in commands)
        {
            var arguments = NewComposer();
            arguments.Style(helpStyles?.Commands?.ChildCommand ?? Style.Plain, child.Name);
            arguments.Space();

            foreach (var argument in HelpArgument.Get(child).Where(a => a.Required))
            {
                arguments.Style(helpStyles?.Commands?.RequiredArgument ?? Style.Plain, $"<{argument.Name}>");
                arguments.Space();
            }

            grid.AddRow(
                NewComposer().Text(arguments.ToString().TrimEnd()),
                NewComposer().Text(NormalizeDescription(child.Description)));
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

    private Composer NewComposer()
    {
        return new Composer(RenderMarkupInline);
    }

    private IRenderable GetOptionParts(HelpOption option)
    {
        var composer = NewComposer();

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
                composer.Style(helpStyles?.Options?.OptionalOptionValue ?? Style.Plain, $"[{option.Value}]");
            }
            else
            {
                composer.Style(helpStyles?.Options?.RequiredOptionValue ?? Style.Plain, $"<{option.Value}>");
            }
        }

        return composer;
    }

    private Composer GetDefaultValueForOption(object? defaultValue)
    {
        return defaultValue switch
        {
            null => NewComposer().Text(" "),
            "" => NewComposer().Text(" "),
            Array { Length: 0 } => NewComposer().Text(" "),
            Array array => NewComposer().Join(
                ", ",
                array.Cast<object>().Select(o =>
                    NewComposer().Style(
                        helpStyles?.Options?.DefaultValue ?? Style.Plain,
                        o.ToString() ?? string.Empty))),
            _ => NewComposer().Style(
                helpStyles?.Options?.DefaultValue ?? Style.Plain,
                defaultValue?.ToString() ?? string.Empty),
        };
    }

    private string NormalizeDescription(string? description)
    {
        if (description == null)
        {
            return " ";
        }

        return TrimTrailingPeriod
            ? description.TrimEnd('.')
            : description;
    }
}