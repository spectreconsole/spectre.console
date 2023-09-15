using System.ComponentModel;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Completion;

namespace AutoCompletion;

[Description("The lion command.")]
public class LionCommand : Command<LionSettings>, IAsyncCommandCompletable
{
    public override int Execute(CommandContext context, LionSettings settings)
    {
        return 0;
    }

    public async Task<CompletionResult> GetSuggestionsAsync(ICommandParameterInfo parameter, string? prefix)
    {
        if(string.IsNullOrEmpty(prefix))
        {
            return CompletionResult.None();
        }

        return await AsyncSuggestionMatcher
            .Add(x => x.Legs, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return FindNextEvenNumber(prefix);
                }

                return "16";
            })
            .Add(x => x.Teeth, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return FindNextEvenNumber(prefix);
                }

                return "32";
            })
            .Add(x => x.Name, prefix =>
            {
                var names = new List<string>
                {
                    "angel", "angelika", "robert",
                    "jennifer", "michael", "lucy",
                    "david", "sarah", "john", "katherine",
                    "mark"
                };

                var bestMatches = names
                    .Where(name => name.StartsWith(prefix))
                    .ToList();

                return new CompletionResult(bestMatches, bestMatches.Any());
            })
            .MatchAsync(parameter, prefix)
            .WithPreventDefault();
    }

    private static string FindNextEvenNumber(string input)
    {
        var number = int.Parse(input); // Parse the input string to an integer

        // Find the next even number greater than the input number
        var nextEvenNumber = number + (2 - (number % 2));

        // Convert the number to string to check the prefix
        var nextEvenNumberString = nextEvenNumber.ToString();

        // Check if the prefix of the even number matches the input string
        while (!nextEvenNumberString.StartsWith(input))
        {
            nextEvenNumber += 2; // Increment by 2 to find the next even number
            nextEvenNumberString = nextEvenNumber.ToString(); // Update the string representation
        }

        return nextEvenNumber.ToString();
    }
}