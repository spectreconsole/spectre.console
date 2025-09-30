using OpenCli;

namespace Spectre.Console.Cli;

internal sealed class OpenCliGeneratorCommand : Command, IBuiltInCommand
{
    private readonly IConfiguration _configuration;
    private readonly CommandModel _model;

    public OpenCliGeneratorCommand(IConfiguration configuration, CommandModel model)
    {
        _configuration = configuration;
        _model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public override int Execute(CommandContext context)
    {
        var document = new OpenCliDocument
        {
            OpenCli = "0.1-draft",
            Info = new OpenCliInfo
            {
                Title = ((ICommandModel)_model).ApplicationName, Version = _model.ApplicationVersion ?? "1.0",
            },
            Commands = CreateCommands(_model.Commands),
            Arguments = CreateArguments(_model.DefaultCommand?.GetArguments()),
            Options = CreateOptions(_model.DefaultCommand?.GetOptions()),
        };

        var writer = _configuration.Settings.Console.GetConsole();
        writer.WriteLine(document.Write());

        return 0;
    }

    private List<OpenCliCommand> CreateCommands(IList<CommandInfo> commands)
    {
        var result = new List<OpenCliCommand>();

        foreach (var command in commands.OrderBy(o => o.Name, StringComparer.OrdinalIgnoreCase))
        {
            if (typeof(IBuiltInCommand).IsAssignableFrom(command.CommandType))
            {
                continue;
            }

            var openCliCommand = new OpenCliCommand
            {
                Name = command.Name,
                Aliases =
                [
                    ..command.Aliases.OrderBy(str => str)
                ],
                Commands = CreateCommands(command.Children),
                Arguments = CreateArguments(command.GetArguments()),
                Options = CreateOptions(command.GetOptions()),
                Description = command.Description,
                Hidden = command.IsHidden,
                Examples = [..command.Examples.Select(example => string.Join(" ", example))],
            };

            // Skip branches without commands
            if (command.IsBranch && openCliCommand.Commands.Count == 0)
            {
                continue;
            }

            result.Add(openCliCommand);
        }

        return result;
    }

    private List<OpenCliArgument> CreateArguments(IEnumerable<CommandArgument>? arguments)
    {
        var result = new List<OpenCliArgument>();

        if (arguments == null)
        {
            return result;
        }

        foreach (var argument in arguments.OrderBy(x => x.Position))
        {
            var metadata = default(List<OpenCliMetadata>);
            if (argument.ParameterType != typeof(void) &&
                argument.ParameterType != typeof(bool))
            {
                metadata =
                [
                    new OpenCliMetadata { Name = "ClrType", Value = argument.ParameterType.ToCliTypeString(), },
                ];
            }

            result.Add(new OpenCliArgument
            {
                Name = argument.Value,
                Required = argument.IsRequired,
                Arity = new OpenCliArity
                {
                    // TODO: Look into this
                    Minimum = 1,
                    Maximum = 1,
                },
                Description = argument.Description,
                Hidden = argument.IsHidden,
                Metadata = metadata,
                AcceptedValues = null,
                Group = null,
            });
        }

        return result;
    }

    private List<OpenCliOption> CreateOptions(IEnumerable<CommandOption>? options)
    {
        var result = new List<OpenCliOption>();

        if (options == null)
        {
            return result;
        }

        foreach (var option in options.OrderBy(o => o.GetOptionName(), StringComparer.OrdinalIgnoreCase))
        {
            var arguments = new List<OpenCliArgument>();
            if (option.ParameterType != typeof(void) &&
                option.ParameterType != typeof(bool))
            {
                arguments.Add(new OpenCliArgument
                {
                    Name = option.ValueName ?? "VALUE",
                    Required = !option.ValueIsOptional,
                    Arity = new OpenCliArity
                    {
                        // TODO: Look into this
                        Minimum = option.ValueIsOptional
                            ? 0
                            : 1,
                        Maximum = 1,
                    },
                    AcceptedValues = null,
                    Group = null,
                    Hidden = null,
                    Metadata =
                    [
                        new OpenCliMetadata
                        {
                            Name = "ClrType",
                            Value = option.ParameterType.ToCliTypeString(),
                        },
                    ],
                });
            }

            var optionMetadata = default(List<OpenCliMetadata>);
            if (arguments.Count == 0 && option.ParameterType != typeof(void) &&
                option.ParameterType != typeof(bool))
            {
                optionMetadata =
                [
                    new OpenCliMetadata { Name = "ClrType", Value = option.ParameterType.ToCliTypeString(), },
                ];
            }

            var (optionName, optionAliases) = GetOptionNames(option);
            result.Add(new OpenCliOption
            {
                Name = optionName,
                Required = option.IsRequired,
                Aliases = [..optionAliases.OrderBy(str => str)],
                Arguments = arguments,
                Description = option.Description,
                Group = null,
                Hidden = option.IsHidden,
                Recursive = option.IsShadowed, // TODO: Is this correct?
                Metadata = optionMetadata,
            });
        }

        return result;
    }

    private static (string Name, HashSet<string> Aliases) GetOptionNames(CommandOption option)
    {
        var name = GetOptionName(option);
        var aliases = new HashSet<string>();

        if (option.LongNames.Count > 0)
        {
            foreach (var alias in option.LongNames.Skip(1))
            {
                aliases.Add("--" + alias);
            }

            foreach (var alias in option.ShortNames)
            {
                aliases.Add("-" + alias);
            }
        }
        else
        {
            foreach (var alias in option.LongNames)
            {
                aliases.Add("--" + alias);
            }

            foreach (var alias in option.ShortNames.Skip(1))
            {
                aliases.Add("-" + alias);
            }
        }

        return (name, aliases);
    }

    private static string GetOptionName(CommandOption option)
    {
        return option.LongNames.Count > 0
            ? "--" + option.LongNames[0]
            : "-" + option.ShortNames[0];
    }
}