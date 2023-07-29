using System.Text.RegularExpressions;

namespace Spectre.Console.Cli.Internal.Commands.Completion;

internal class CommandCompletionContext
{
    public bool ShouldSuggestMatchingInRoot { get; set; }
    public bool ShouldReturnEarly { get; set; }

    public string[] CommandElements { get; set; }
    public string PartialElement { get; set; }
    public CommandInfo? Parent { get; set; }
    public List<MappedCommandParameter> MappedParameters { get; set; }

    public CommandCompletionContext()
    {
        CommandElements = Array.Empty<string>();
        PartialElement = string.Empty;
        MappedParameters = new List<MappedCommandParameter>();
    }
}

internal class CommandCompletionContextParser
{
    private readonly CommandModel _model;
    private readonly IConfiguration _configuration;

    public CommandCompletionContextParser(CommandModel model, IConfiguration configuration)
    {
        _model = model;
        _configuration = configuration;
    }

    public CommandCompletionContext? Parse(string? commandToComplete, int? position)
    {
        var normalizedCommand = NormalizeCommand(commandToComplete, position);
        var commandElements = SplitBySpace(normalizedCommand).Skip(1).ToArray();

        if (commandElements?.Length is 0 or null)
        {
            return new CommandCompletionContext
            {
                ShouldSuggestMatchingInRoot = true,
            };
        }

        var parser = new CommandTreeParser(_model, _configuration.Settings.CaseSensitivity);
        CommandTreeParserResult? parsedResult = null;
        var context = string.Empty;
        var partialElement = string.Empty;
        try
        {
            parsedResult = parser.Parse(commandElements);
            context = commandElements.LastOrDefault() ?? string.Empty;

            if (string.IsNullOrEmpty(context))
            {
                context = commandElements[commandElements.Length - 2];
            }

            // Early return when "myapp feline"
            // but show completions for feline if "myapp feline "
            // spacing matters
            if (parsedResult.Tree?.Command.Name == context)
            {
                return new CommandCompletionContext
                {
                    CommandElements = commandElements,
                    PartialElement = partialElement,
                    ShouldSuggestMatchingInRoot = false,
                    ShouldReturnEarly = true,
                };
            }
        }
        catch (CommandParseException)
        {
            var strippedCommandElements = commandElements.Take(commandElements.Length - 1);
            if (strippedCommandElements.Any())
            {
                parsedResult = parser.Parse(strippedCommandElements);
                context = strippedCommandElements.Last();
            }

            partialElement = commandElements.LastOrDefault()?.ToLowerInvariant() ?? string.Empty;
        }

        CommandInfo parent;
        List<MappedCommandParameter> mappedParameters;
        if (parsedResult?.Tree == null)
        {
            // Should suggest anything matching in the root
            return new CommandCompletionContext
            {
                CommandElements = commandElements,
                PartialElement = partialElement,
                ShouldSuggestMatchingInRoot = true,
            };
        }

        var lastContext = FindContextInTree(parsedResult);

        if (lastContext?.Command == null)
        {
            parent = parsedResult.Tree.Command;
            mappedParameters = parsedResult.Tree.Mapped;
        }
        else
        {
            parent = lastContext.Command;
            mappedParameters = lastContext.Mapped;
        }

        return new CommandCompletionContext
        {
            CommandElements = commandElements,
            PartialElement = partialElement,
            Parent = parent,
            MappedParameters = mappedParameters,
        };
    }

    private static CommandTree? FindContextInTree(CommandTreeParserResult? parsedResult)
    {
        var tree = parsedResult?.Tree;
        return FindContextInTree(tree);
    }

    private static CommandTree? FindContextInTree(CommandTree? tree)
    {
        if (tree is null)
        {
            return null;
        }

        var next = tree.Next;

        if (next is null)
        {
            return tree;
        }

        return FindContextInTree(next) ?? tree;
    }

    /// <summary>
    /// Trims the first character and the last character.
    /// </summary>
    private static string TrimOnce(string input, char character)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        if (input[0] == character && input[input.Length - 1] == character)
        {
            return input.Substring(1, input.Length - 2);
        }

        return input;
    }

    private static string[] SplitBySpace(string input)
    {
        // Regular expression pattern to match spaces except those within double quotes
        string pattern = @"\s+(?=(?:[^""]*""[^""]*"")*[^""]*$)";

        // Split the input string using the regular expression pattern
        string[] result = Regex.Split(input, pattern);

        // Remove any leading or trailing " characters on each element
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = TrimOnce(result[i], '"');
        }

        return result;
    }

    private static string NormalizeCommand(string? commandToComplete, int? position)
    {
        var normalizedCommand = TrimOnce(commandToComplete, '"');

        if (!string.IsNullOrEmpty(normalizedCommand)
            && position != null
            && position > normalizedCommand.Length)
        {
            // extend the command to the position with whitespace
            var requiredWhitespace = (position - normalizedCommand.Length) ?? 1;
            normalizedCommand += new string(' ', requiredWhitespace);
        }

        return normalizedCommand;
    }
}