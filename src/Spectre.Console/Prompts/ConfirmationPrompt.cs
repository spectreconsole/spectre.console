namespace Spectre.Console;

/// <summary>
/// A prompt that is answered with a yes or no.
/// </summary>
public sealed class ConfirmationPrompt : IPrompt<bool>
{
    private readonly string _prompt;

    /// <summary>
    /// Gets or sets the character that represents "yes".
    /// </summary>
    public char Yes { get; set; } = 'y';

    /// <summary>
    /// Gets or sets the character that represents "no".
    /// </summary>
    public char No { get; set; } = 'n';

    /// <summary>
    /// Gets or sets a value indicating whether "yes" is the default answer.
    /// </summary>
    public bool DefaultValue { get; set; } = true;

    /// <summary>
    /// Gets or sets the message for invalid choices.
    /// </summary>
    public string InvalidChoiceMessage { get; set; } = "[red]Please select one of the available options[/]";

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// choices should be shown.
    /// </summary>
    public bool ShowChoices { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not
    /// default values should be shown.
    /// </summary>
    public bool ShowDefaultValue { get; set; } = true;

    /// <summary>
    /// Gets or sets the style in which the default value is displayed. Defaults to green when <see langword="null"/>.
    /// </summary>
    public Style? DefaultValueStyle { get; set; }

    /// <summary>
    /// Gets or sets the style in which the list of choices is displayed. Defaults to blue when <see langword="null"/>.
    /// </summary>
    public Style? ChoicesStyle { get; set; }

    /// <summary>
    /// Gets or sets the string comparer to use when comparing user input
    /// against Yes/No choices.
    /// </summary>
    /// <remarks>
    /// Defaults to <see cref="StringComparer.CurrentCultureIgnoreCase"/>.
    /// </remarks>
    public StringComparer Comparer { get; set; } = StringComparer.CurrentCultureIgnoreCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmationPrompt"/> class.
    /// </summary>
    /// <param name="prompt">The prompt markup text.</param>
    public ConfirmationPrompt(string prompt)
    {
        _prompt = prompt ?? throw new System.ArgumentNullException(nameof(prompt));
    }

    /// <inheritdoc/>
    public bool Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<bool> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        var comparer = Comparer ?? StringComparer.CurrentCultureIgnoreCase;

        var prompt = new TextPrompt<char>(_prompt, comparer)
            .InvalidChoiceMessage(InvalidChoiceMessage)
            .ValidationErrorMessage(InvalidChoiceMessage)
            .ShowChoices(ShowChoices)
            .ChoicesStyle(ChoicesStyle)
            .ShowDefaultValue(ShowDefaultValue)
            .DefaultValue(DefaultValue ? Yes : No)
            .DefaultValueStyle(DefaultValueStyle)
            .AddChoice(Yes)
            .AddChoice(No);

        var result = await prompt.ShowAsync(console, cancellationToken).ConfigureAwait(false);

        return comparer.Compare(Yes.ToString(), result.ToString()) == 0;
    }
}

/// <summary>
/// Contains extension methods for <see cref="ConfirmationPrompt"/>.
/// </summary>
public static class ConfirmationPromptExtensions
{
    /// <summary>
    /// Show or hide choices.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not the choices should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt ShowChoices(this ConfirmationPrompt obj, bool show)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.ShowChoices = show;
        return obj;
    }

    /// <summary>
    /// Shows choices.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt ShowChoices(this ConfirmationPrompt obj)
    {
        return ShowChoices(obj, true);
    }

    /// <summary>
    /// Hides choices.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt HideChoices(this ConfirmationPrompt obj)
    {
        return ShowChoices(obj, false);
    }

    /// <summary>
    /// Sets the style in which the list of choices is displayed.
    /// </summary>
    /// <param name="obj">The confirmation prompt.</param>
    /// <param name="style">The style to use for displaying the choices or <see langword="null"/> to use the default style (blue).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt ChoicesStyle(this ConfirmationPrompt obj, Style? style)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.ChoicesStyle = style;
        return obj;
    }

    /// <summary>
    /// Show or hide the default value.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <param name="show">Whether or not the default value should be visible.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt ShowDefaultValue(this ConfirmationPrompt obj, bool show)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.ShowDefaultValue = show;
        return obj;
    }

    /// <summary>
    /// Shows the default value.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt ShowDefaultValue(this ConfirmationPrompt obj)
    {
        return ShowDefaultValue(obj, true);
    }

    /// <summary>
    /// Hides the default value.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt HideDefaultValue(this ConfirmationPrompt obj)
    {
        return ShowDefaultValue(obj, false);
    }

    /// <summary>
    /// Sets the style in which the default value is displayed.
    /// </summary>
    /// <param name="obj">The confirmation prompt.</param>
    /// <param name="style">The default value style or <see langword="null"/> to use the default style (green).</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt DefaultValueStyle(this ConfirmationPrompt obj, Style? style)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.DefaultValueStyle = style;
        return obj;
    }

    /// <summary>
    /// Sets the "invalid choice" message for the prompt.
    /// </summary>
    /// <param name="obj">The prompt.</param>
    /// <param name="message">The "invalid choice" message.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt InvalidChoiceMessage(this ConfirmationPrompt obj, string message)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.InvalidChoiceMessage = message;
        return obj;
    }

    /// <summary>
    /// Sets the character to interpret as "yes".
    /// </summary>
    /// <param name="obj">The confirmation prompt.</param>
    /// <param name="character">The character to interpret as "yes".</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt Yes(this ConfirmationPrompt obj, char character)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.Yes = character;
        return obj;
    }

    /// <summary>
    /// Sets the character to interpret as "no".
    /// </summary>
    /// <param name="obj">The confirmation prompt.</param>
    /// <param name="character">The character to interpret as "no".</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public static ConfirmationPrompt No(this ConfirmationPrompt obj, char character)
    {
        ArgumentNullException.ThrowIfNull(obj);

        obj.No = character;
        return obj;
    }
}