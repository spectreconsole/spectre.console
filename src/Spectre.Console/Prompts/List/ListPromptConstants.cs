namespace Spectre.Console;

internal sealed class ListPromptConstants
{
    public const string Arrow = ">";
    public const string Checkbox = "[[ ]]";
    public const string SelectedCheckbox = "[[[blue]X[/]]]";
    public const string GroupSelectedCheckbox = "[[[grey]X[/]]]";
    public const string InstructionsMarkup = "[grey](Press <space> to select, <enter> to accept)[/]";
    public const string MoreChoicesMarkup = "[grey](Move up and down to reveal more choices)[/]";
    public const string SearchPlaceholderMarkup = "[grey](Type to search)[/]";

    public static string GetSelectedCheckbox(bool isGroup, SelectionMode mode, Style? style = null)
    {
        if (style != null)
        {
            return "[[" + $"[{style.Value.ToMarkup()}]X[/]" + "]]";
        }

        return isGroup && mode == SelectionMode.Leaf
            ? GroupSelectedCheckbox : SelectedCheckbox;
    }
}