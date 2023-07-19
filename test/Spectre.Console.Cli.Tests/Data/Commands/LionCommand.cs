using Spectre.Console.Cli.Completion;

namespace Spectre.Console.Tests.Data;

[Description("The lion command.")]
public class LionCommand : AnimalCommand<LionSettings>, ICommandParameterCompleter
{
    public override int Execute(CommandContext context, LionSettings settings)
    {
        return 0;
    }

    public ICompletionResult GetSuggestions(ICommandParameterInfo parameter, string prefix)
    {
        return new CommandParameterMatcher<LionSettings>()
            .Add(x => x.Legs, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return CompletionResult.Result(FindNextEvenNumber(prefix)).WithPreventDefault();
                }

                return CompletionResult.Result("16").WithPreventDefault();
            })
            .Add(x => x.Teeth, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return CompletionResult.Result(FindNextEvenNumber(prefix)).WithPreventDefault();
                }

                return CompletionResult.Result("32").WithPreventDefault();
            })
            .Add(x => x.Name, x => CompletionResult.Result("Angelika").WithPreventDefault())
            .Match(parameter, prefix);
    }

    private static string FindNextEvenNumber(string input)
    {
        int number = int.Parse(input); // Parse the input string to an integer

        // Find the next even number greater than the input number
        int nextEvenNumber = number + (2 - (number % 2));

        // Convert the number to string to check the prefix
        string nextEvenNumberString = nextEvenNumber.ToString();

        // Check if the prefix of the even number matches the input string
        while (!nextEvenNumberString.StartsWith(input))
        {
            nextEvenNumber += 2; // Increment by 2 to find the next even number
            nextEvenNumberString = nextEvenNumber.ToString(); // Update the string representation
        }

        return nextEvenNumber.ToString();
    }
}