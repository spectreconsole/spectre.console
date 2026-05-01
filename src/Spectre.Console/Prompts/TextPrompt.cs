using Spectre.Console.Rendering.Prompts;

namespace Spectre.Console;

/// <summary>
/// Represents a prompt.
/// </summary>
/// <typeparam name="T">The prompt result type.</typeparam>
public sealed class TextPrompt<T> : IPrompt<T>, IRenderable, IHasCulture
{
    private readonly string _prompt;
    private readonly StringComparer? _comparer;
    // State: holds the current input being edited
    private string _currentInput = string.Empty;
    private string? _message;

    /// <summary>
    /// Gets or sets the prompt style.
    /// </summary>
    public Style? PromptStyle { get; set; }

    /// <summary>
    /// Gets the list of choices.
    /// </summary>
    public List<T> Choices { get; } = [];

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
    /// Gets or sets the default value input method. Defaults to false.
    /// When enabled, the default value is placed in the input buffer.
    /// </summary>
    public bool DefaultInput { get; set; }

    /// <summary>
    /// Gets or sets the default value editable state that allows the injection of the DefaultValue in the text field.
    /// If true this places the DefaultValue in the input buffer which can then be edited by the user.
    /// </summary>
    public bool EditableDefaultValue { get; set; }

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
    /// Measures the renderable width requirements.
    /// </summary>
    public Measurement Measure(RenderOptions options, int maxWidth)
    {
        // Minimal renderable width: prompt + input field
        // This is a conservative estimate; actual width depends on prompt text
        var promptWidth = _prompt.Length + 1;  // +1 for space after prompt
        return new Measurement(Math.Min(10, maxWidth), maxWidth);
    }

    /// <summary>
    /// Renders the prompt UI: prompt text + current input field.
    /// </summary>
    public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
    {
        var segments = new List<Segment>();

        // Build the prompt prefix (text + choices + default)
        var promptText = BuildPromptText();
        segments.AddRange(((IRenderable)new Markup(promptText)).Render(options, maxWidth));

        // Build and render the input field
        var inputRenderable = BuildInputField();
        segments.AddRange(inputRenderable.Render(options, maxWidth));

        if (!string.IsNullOrWhiteSpace(_message))
        {
            segments.Add(Segment.LineBreak);
            segments.AddRange(((IRenderable)new Markup(_message)).Render(options, maxWidth));
        }

        return segments;
    }

    /// <summary>
    /// Builds the prompt prefix text (includes choices, default value, etc.).
    /// </summary>
    private string BuildPromptText()
    {
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

        return markup + " ";
    }

    /// <summary>
    /// Builds the input field renderable (the editable text area).
    /// </summary>
    private IRenderable BuildInputField()
    {
        var promptStyle = PromptStyle ?? Style.Plain;
        var displayText = IsSecret ? _currentInput.Mask(Mask) : _currentInput;
        return new Text(displayText, promptStyle);
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

            while (true)
            {
                WritePrompt(console);

                var initialText = DefaultInput && DefaultValue != null
                    ? converter(DefaultValue.Value)
                    : string.Empty;

                var input = await console.ReadLine(promptStyle, IsSecret, Mask, choices, initialText, cancellationToken).ConfigureAwait(false);


                // Nothing entered?
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (DefaultValue != null)
                    {
                        var defaultValue = converter(DefaultValue.Value);
                        console.Write(IsSecret ? defaultValue.Mask(Mask) : defaultValue, promptStyle);
                        console.WriteLine();

                        ClearPromptLine(console);
                        return DefaultValue.Value;
                    }

                    if (!AllowEmpty)
                    {
                        continue;
                    }
                }

                //if (string.IsNullOrWhiteSpace(input))
                //{
                //    if (DefaultValue != null)
                //    {
                //        console.WriteLine();
                //        return DefaultValue.Value;
                //    }
//
                //    if (!AllowEmpty)
                //    {
                //        console.WriteLine();
                //        continue;
                //    }
                //}

                console.WriteLine();

                T? result;
                if (Choices.Count > 0)
                {
                    if (choiceMap.TryGetValue(input, out result) && result != null)
                    {
                        return result;
                    }

                    console.MarkupLine(InvalidChoiceMessage);
                    continue;
                }

                if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result) || result == null)
                {
                    console.MarkupLine(ValidationErrorMessage);
                    continue;
                }

                if (!ValidateResult(result, out var validationMessage))
                {
                    console.MarkupLine(validationMessage);
                    continue;
                }

                return result;
            }
        }).ConfigureAwait(false);
    }

    /// <summary>
    /// Shows the prompt as a renderable with live input updates via a render hook.
    /// </summary>
    /// <param name="console">The console to show the prompt in.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user input converted to the expected type.</returns>
    public Task<T> ShowAsRenderableAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        return ShowAsRenderableAsync(console, null, cancellationToken);
    }

    /// <summary>
    /// Shows the prompt as a renderable with live input updates via a render hook.
    /// </summary>
    /// <param name="console">The console to show the prompt in.</param>
    /// <param name="wrapper">An optional wrapper renderable, such as a panel.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user input converted to the expected type.</returns>
    public async Task<T> ShowAsRenderableAsync(IAnsiConsole console, IRenderable? wrapper, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);

        var renderable = wrapper ?? this;

        return await console.RunExclusive(async () =>
        {
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var choices = Choices.Select(choice => converter(choice)).ToList();
            var choiceMap = Choices.ToDictionary(choice => converter(choice), choice => choice, _comparer);

            _currentInput = DefaultInput && DefaultValue != null
                ? converter(DefaultValue.Value)
                : string.Empty;
            _message = null;

            var hook = new TextPromptRenderHook<T>(console, () => renderable);
            using (new RenderHookScope(console, hook))
            {
                console.Cursor.Hide();
                hook.Refresh();

                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
                    if (rawKey == null)
                    {
                        continue;
                    }

                    if (_message != null)
                    {
                        _message = null;
                    }

                    if (HandleInputKey(rawKey.Value))
                    {
                        var input = _currentInput;

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            if (DefaultValue != null)
                            {
                                hook.Clear();
                                console.Cursor.Show();
                                console.WriteLine();
                                return DefaultValue.Value;
                            }

                            if (!AllowEmpty)
                            {
                                _currentInput = string.Empty;
                                hook.Refresh();
                                continue;
                            }
                        }

                        T? result;
                        if (Choices.Count > 0)
                        {
                            if (choiceMap.TryGetValue(input, out result) && result != null)
                            {
                                hook.Clear();
                                console.Cursor.Show();
                                console.WriteLine();
                                return result;
                            }

                            _message = InvalidChoiceMessage;
                            _currentInput = string.Empty;
                            hook.Refresh();
                            continue;
                        }

                        if (!TypeConverterHelper.TryConvertFromStringWithCulture<T>(input, Culture, out result) || result == null)
                        {
                            _message = ValidationErrorMessage;
                            _currentInput = string.Empty;
                            hook.Refresh();
                            continue;
                        }

                        if (!ValidateResult(result, out var validationMessage))
                        {
                            _message = validationMessage;
                            _currentInput = string.Empty;
                            hook.Refresh();
                            continue;
                        }

                        hook.Clear();
                        console.Cursor.Show();
                        console.WriteLine();
                        return result;
                    }

                    hook.Refresh();
                }
            }

            // Should never reach here, but required for compiler.
            throw new InvalidOperationException("Text prompt ended unexpectedly.");
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


    /// <summary>
    /// Clears the prompt line when enabled.
    /// </summary>
    /// <param name="console">The console to clear the prompt from.</param>
    private void ClearPromptLine(IAnsiConsole console)
    {
        //if (!ClearOnFinish)
        //{
        //    return;
        //}
//
        ArgumentNullException.ThrowIfNull(console);

        if (!console.Profile.Capabilities.Ansi)
        {
            return;
        }

        console.Cursor.MoveUp();
        console.Write(ControlCode.Create(console, writer =>
        {
            writer.Write("\r");
            writer.EraseInLine(2);
        }));
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

    /// <summary>
    /// Processes a single key input and updates state.
    /// Returns true if the user pressed Enter (submit), false otherwise.
    /// </summary>
    private bool HandleInputKey(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            return true;  // Submit
        }

        if (key.Key == ConsoleKey.Backspace)
        {
            if (_currentInput.Length > 0)
            {
                _currentInput = _currentInput[..^1];
            }
            return false;
        }

        // Handle Tab for autocomplete (if choices available)
        if (key.Key == ConsoleKey.Tab && Choices.Count > 0)
        {
            var converter = Converter ?? TypeConverterHelper.ConvertToString;
            var choiceStrings = Choices.Select(c => converter(c)).ToList();
            var replacement = AutoComplete(choiceStrings, _currentInput);
            if (!string.IsNullOrEmpty(replacement))
            {
                _currentInput = replacement;
            }
            return false;
        }

        // Add regular characters
        if (!char.IsControl(key.KeyChar))
        {
            _currentInput += key.KeyChar;
            return false;
        }

        return false;
    }

    /// <summary>
    /// Autocomplete logic (from original TextPrompt).
    /// </summary>
    private string AutoComplete(List<string> choices, string text)
    {
        if (choices.Count == 0) return string.Empty;

        // Find exact match
        var found = choices.Find(i => i == text);
        if (found != null)
        {
            // Cycle to next choice
            var index = choices.IndexOf(found);
            index = (index + 1) % choices.Count;
            return choices[index];
        }

        // Find prefix match
        var next = choices.Find(i => i.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));
        if (next != null)
        {
            return next;
        }

        // Use first if empty
        if (string.IsNullOrEmpty(text))
        {
            return choices[0];
        }

        return string.Empty;
    }

    /// <summary>
    /// Builds the renderable for the input field (called by RenderHook).
    /// This is separate from Render() to keep the hook focused.
    /// </summary>
    private IRenderable BuildInputRenderable()
    {
        var promptStyle = PromptStyle ?? Style.Plain;
        var displayText = IsSecret ? _currentInput.Mask(Mask) : _currentInput;

        // Add a cursor indicator if desired
        var text = new Text(displayText + "_", promptStyle);
        return text;
    }
}