Title: Using AutoCompletion
Order: 14
Description: "How to use the AutoCompletion feature"
---
# Spectre.Console AutoCompletion

Spectre.Console.Cli includes auto completion for the shell.
It comes with suggestions for Options and Branches out of the box, but you can also add your own suggestions for option and argument values.

- [Shell integrations](#shell-integrations)
  - [PowerShell](#powershell)
- [How integrations get the suggestions](#how-integrations-get-the-suggestions)
- [Customizations](#customizations)
  - [Static Autocomplete](#static-autocomplete)
  - [Dynamic Autocomplete](#dynamic-autocomplete)
- [Disabling the Module](#disabling-the-module)



## Shell integrations
1. [PowerShell](#powershell)
3. More to come...


### PowerShell

You can add autocomplete to PowerShell by running your application with the `completion powershell` command, as shown below:


```powershell
.\AutoCompletion.exe completion powershell | Out-String | Invoke-Expression
```

To add autocomplete to PowerShell permanently, use the `--install` flag:

```powershell
.\AutoCompletion.exe completion powershell --install | Out-String | Invoke-Expression
```

## How integrations get the suggestions

The shell integration uses the `cli complete` command to get the suggestions for the current command line like this:

```powershell
.\AutoCompletion.exe cli complete "Li"
```

## Customizations
1. [Static Autocomplete](#static-autocomplete) 
2. [Dynamic Autocomplete](#dynamic-autocomplete)

### Static Autocomplete

Spectre.Console auto completion allows you to specify static autocomplete suggestions for your command arguments and options. This can be done using the `CompletionSuggestions` attribute in your command settings class.

Here's an example of how to add static autocomplete suggestions:

```csharp
public class LionSettings : CommandSettings
{
    [CommandArgument(0, "<TEETH>")]
    [Description("The number of teeth the lion has.")]
    [CompletionSuggestions("10", "15", "20", "30")]
    public int Teeth { get; set; }

    [CommandOption("-a|--age <AGE>")]
    public int Age { get; set; }

    [CommandOption("-n|--name <NAME>")]
    public string Name { get; set; }
}
```

### Dynamic Autocomplete

In addition to static autocomplete suggestions, you can also provide dynamic autocomplete suggestions based on the user's input. This can be done by implementing the `IAsyncCommandCompletable` interface in your command class and overriding the `GetSuggestionsAsync` method.

Here's an example of how to add dynamic autocomplete suggestions:

```csharp
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
            .Add(x => x.Age, (prefix) =>
            {
                if (prefix.Length != 0)
                {
                    return FindNextEvenNumber(prefix);
                }

                return "16";
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
}
```

## Disabling the Module

If you need to disable the autocomplete feature for any reason, you can do so by setting the `AutoCompletionModule` to `None` when configuring your application:

```csharp
var app = new CommandApp();
app.Configure(config => config.UseAutoComplete(AutoCompletionModule.None));

app.Run(args);
```

The `AutoCompletionModule` enum provides several options for enabling and disabling different aspects of the autocomplete feature:

```csharp
[Flags]
public enum AutoCompletionModule
{
    None = 0, // No auto completion module is enabled.
    Base = 1 << 0, // Basic auto completion functionality.
    Powershell = 1 << 1, // Auto completion features specific to Powershell.
    All = Base | Powershell, // All auto completion modules are enabled.
}
```

There is a working [example of the AutoCompletion feature](https://github.com/JKamsker/spectre.console/tree/AutoCompletion/examples/Cli/AutoCompletion) demonstrating this.