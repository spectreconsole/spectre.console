using Spectre.Console.Cli.Completion;

namespace Spectre.Console.Tests.Data;

[Description("The lion command.")]
public class LionCommand : AnimalCommand<LionSettings>, ICommandCompletable, IAsyncCommandCompletable
{
    public override int Execute(CommandContext context, LionSettings settings)
    {
        return 0;
    }

    public CompletionResult GetSuggestions(ICommandParameterInfo parameter, string prefix)
    {
        return new CommandParameterMatcher<LionSettings>()
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
            .Add(x => x.Name, _ => "Angelika")
            .Match(parameter, prefix).WithPreventDefault();
    }

    public async Task<CompletionResult> GetSuggestionsAsync(ICommandParameterInfo parameter, string prefix)
    {
        return await new AsyncCommandParameterMatcher<LionSettings>()
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
            .Add(x => x.Name, _ => "Angelika")
            .MatchAsync(parameter, prefix);
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