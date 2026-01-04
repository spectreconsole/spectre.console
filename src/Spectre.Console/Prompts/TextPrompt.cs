namespace Spectre.Console;

/// <summary>
/// Represents a prompt.
/// </summary>
/// <typeparam name="T">The prompt result type.</typeparam>
public sealed class TextPrompt<T> : IPrompt<T>, IHasCulture
{
    private readonly string _prompt;
    private readonly StringComparer? _comparer;

    /// <summary>
    /// Gets or sets the prompt style.
    /// </summary>
    public Style? PromptStyle { get; set; }

    /// <summary>
    /// Gets the list of choices.
    /// </summary>
    public List<T> Choices { get; } = new List<T>();

    /// <summary>
    /// Gets or sets the culture to use when converting input to object.
    /// </summary>
    public CultureInfo? Culture { get; set; }

    /// <summary>
    /// Gets or sets the message for invalid choices.
    /// </summary>
    public string InvalidChoiceMessage { get; set; } = "[red]Please select one of the available options[/]";

    /// <summary>
    /// Gets or sets a value indicating whether input should
    /// be hidden in the console.
    /// </summary>
    public bool IsSecret { get; set; }

    /// <summary>
    /// Gets or sets the character to use while masking
    /// a secret prompt.
    /// </summary>
    public char? Mask { get; set; } = '*';

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationErrorMessage { get; set; } = "[red]Invalid input[/]";

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
    /// Gets or sets a value indicating whether or not an empty result is valid.
    /// </summary>
    public bool AllowEmpty { get; set; }

    /// <summary>
    /// Gets or sets the converter to get the display string for a choice. By default
    /// the corresponding <see cref="TypeConverter"/> is used.
    /// </summary>
    public Func<T, string>? Converter { get; set; } = TypeConverterHelper.ConvertToString;

    /// <summary>
    /// Gets or sets the validator.
    /// </summary>
    public Func<T, ValidationResult>? Validator { get; set; }

    /// <summary>
    /// Gets or sets the style in which the default value is displayed. Defaults to green when <see langword="null"/>.
    /// </summary>
    public Style? DefaultValueStyle { get; set; }

    /// <summary>
    /// Gets or sets the style in which the list of choices is displayed. Defaults to blue when <see langword="null"/>.
    /// </summary>
    public Style? ChoicesStyle { get; set; }

    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    internal DefaultPromptValue<T>? DefaultValue { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextPrompt{T}"/> class.
    /// </summary>
    /// <param name="prompt">The prompt markup text.</param>
    /// <param name="comparer">The comparer used for choices.</param>
    public TextPrompt(string prompt, StringComparer? comparer = null)
    {
        _prompt = prompt ?? throw new System.ArgumentNullException(nameof(prompt));
        _comparer = comparer;
    }

    /// <summary>
    /// Shows the prompt and requests input from the user.
    /// </summary>
    /// <param name="console">The console to show the prompt in.</param>
    /// <returns>The user input converted to the expected type.</returns>
    /// <inheritdoc/>
    public T Show(IAnsiConsole console)
    {
        return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
    }

    /// <inheritdoc/>
    public async Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);

        return await console.RunExclusive(async () =>
        {
            var promptStyle = PromptStyle ?? Style.Plain;
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var choices = Choices.Select(choice => converter(choice)).ToList();
            var choiceMap = Choices.ToDictionary(choice => converter(choice), choice => choice, _comparer);

            WritePrompt(console);

            while (true)
            {
                var input = await console.ReadLine(promptStyle, IsSecret, Mask, choices, cancellationToken).ConfigureAwait(false);

                // Nothing entered?
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (DefaultValue != null)
                    {
                        var defaultValue = converter(DefaultValue.Value);
                        console.Write(IsSecret ? defaultValue.Mask(Mask) : defaultValue, promptStyle);
                        console.WriteLine();
                        return DefaultValue.Value;
                    }

                    if (!AllowEmpty)
                    {
                        continue;
                    }
                }

                console.WriteLine();

                T? result;
                if (Choices.Count > 0)
                {
                    if (choiceMap.TryGetValue(input, out result) && result != null)
                    {
                        return result;
                    }
                    else
                    {
                        console.MarkupLine(InvalidChoiceMessage);
                        WritePrompt(console);
                        continue;
                    }
                }
                else if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result) || result == null)
                {
                    console.MarkupLine(ValidationErrorMessage);
                    WritePrompt(console);
                    continue;
                }

                // Run all validators
                if (!ValidateResult(result, out var validationMessage))
                {
                    console.MarkupLine(validationMessage);
                    WritePrompt(console);
                    continue;
                }

                return result;
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Writes the prompt to the console.
    /// </summary>
    /// <param name="console">The console to write the prompt to.</param>
    private void WritePrompt(IAnsiConsole console)
    {
        ArgumentNullException.ThrowIfNull(console);

        var builder = new StringBuilder();
        builder.Append(_prompt.TrimEnd());

        var appendSuffix = false;
        if (ShowChoices && Choices.Count > 0)
        {
            appendSuffix = true;
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var choices = string.Join("/", Choices.Select(choice => converter(choice)));
            var choicesStyle = ChoicesStyle?.ToMarkup() ?? "blue";
            builder.AppendFormat(CultureInfo.InvariantCulture, " [{0}][[{1}]][/]", choicesStyle, choices);
        }

        if (ShowDefaultValue && DefaultValue != null)
        {
            appendSuffix = true;
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var defaultValueStyle = DefaultValueStyle?.ToMarkup() ?? "green";
            var defaultValue = converter(DefaultValue.Value);

            builder.AppendFormat(
                CultureInfo.InvariantCulture,
                " [{0}]({1})[/]",
                defaultValueStyle,
                IsSecret ? defaultValue.Mask(Mask) : defaultValue);
        }

        var markup = builder.ToString().Trim();
        if (appendSuffix)
        {
            markup += ":";
        }

        console.Markup(markup + " ");
    }

    private bool ValidateResult(T value, [NotNullWhen(false)] out string? message)
    {
        if (Validator != null)
        {
            var result = Validator(value);
            if (!result.Successful)
            {
                message = result.Message ?? ValidationErrorMessage;
                return false;
            }
        }

        message = null;
        return true;
    }
}

/// <summary>
/// Contains extension methods for <see cref="TextPrompt{T}"/>.
/// </summary>
public static class TextPromptExtensions
{
    /// <param name="obj">The prompt.</param>
    /// <typeparam name="T">The prompt result type.</typeparam>
    extension<T>(TextPrompt<T> obj)
    {
        /// <summary>
        /// Allow empty input.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> AllowEmpty()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.AllowEmpty = true;
            return obj;
        }

        /// <summary>
        /// Sets the prompt style.
        /// </summary>
        /// <param name="style">The prompt style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> PromptStyle(Style style)
        {
            ArgumentNullException.ThrowIfNull(obj);

            ArgumentNullException.ThrowIfNull(style);

            obj.PromptStyle = style;
            return obj;
        }

        /// <summary>
        /// Show or hide choices.
        /// </summary>
        /// <param name="show">Whether or not choices should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ShowChoices(bool show)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.ShowChoices = show;
            return obj;
        }

        /// <summary>
        /// Shows choices.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ShowChoices()
        {
            return ShowChoices(obj, true);
        }

        /// <summary>
        /// Hides choices.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> HideChoices()
        {
            return ShowChoices(obj, false);
        }

        /// <summary>
        /// Show or hide the default value.
        /// </summary>
        /// <param name="show">Whether or not the default value should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ShowDefaultValue(bool show)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.ShowDefaultValue = show;
            return obj;
        }

        /// <summary>
        /// Shows the default value.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ShowDefaultValue()
        {
            return ShowDefaultValue(obj, true);
        }

        /// <summary>
        /// Hides the default value.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> HideDefaultValue()
        {
            return ShowDefaultValue(obj, false);
        }

        /// <summary>
        /// Sets the validation error message for the prompt.
        /// </summary>
        /// <param name="message">The validation error message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ValidationErrorMessage(string message)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.ValidationErrorMessage = message;
            return obj;
        }

        /// <summary>
        /// Sets the "invalid choice" message for the prompt.
        /// </summary>
        /// <param name="message">The "invalid choice" message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> InvalidChoiceMessage(string message)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.InvalidChoiceMessage = message;
            return obj;
        }

        /// <summary>
        /// Sets the default value of the prompt.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> DefaultValue(T value)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.DefaultValue = new DefaultPromptValue<T>(value);
            return obj;
        }

        /// <summary>
        /// Sets the validation criteria for the prompt.
        /// </summary>
        /// <param name="validator">The validation criteria.</param>
        /// <param name="message">The validation error message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> Validate(Func<T, bool> validator, string? message = null)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Validator = result =>
            {
                if (validator(result))
                {
                    return ValidationResult.Success();
                }

                return ValidationResult.Error(message);
            };

            return obj;
        }

        /// <summary>
        /// Sets the validation criteria for the prompt.
        /// </summary>
        /// <param name="validator">The validation criteria.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> Validate(Func<T, ValidationResult> validator)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Validator = validator;

            return obj;
        }

        /// <summary>
        /// Adds a choice to the prompt.
        /// </summary>
        /// <param name="choice">The choice to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> AddChoice(T choice)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Choices.Add(choice);
            return obj;
        }

        /// <summary>
        /// Adds multiple choices to the prompt.
        /// </summary>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> AddChoices(IEnumerable<T> choices)
        {
            ArgumentNullException.ThrowIfNull(obj);

            ArgumentNullException.ThrowIfNull(choices);

            foreach (var choice in choices)
            {
                obj.Choices.Add(choice);
            }

            return obj;
        }

        /// <summary>
        /// Replaces prompt user input with asterisks in the console.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> Secret()
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.IsSecret = true;
            return obj;
        }

        /// <summary>
        /// Replaces prompt user input with mask in the console.
        /// </summary>
        /// <param name="mask">The masking character to use for the secret.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> Secret(char? mask)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.IsSecret = true;
            obj.Mask = mask;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice.
        /// </summary>
        /// <param name="displaySelector">The function to get a display string for a given choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> WithConverter(Func<T, string>? displaySelector)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.Converter = displaySelector;
            return obj;
        }

        /// <summary>
        /// Sets the style in which the default value is displayed.
        /// </summary>
        /// <param name="style">The default value style or <see langword="null"/> to use the default style (green).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> DefaultValueStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.DefaultValueStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the style in which the list of choices is displayed.
        /// </summary>
        /// <param name="style">The style to use for displaying the choices or <see langword="null"/> to use the default style (blue).</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TextPrompt<T> ChoicesStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(obj);

            obj.ChoicesStyle = style;
            return obj;
        }
    }
}