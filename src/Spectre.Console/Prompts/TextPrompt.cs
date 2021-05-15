using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    public sealed class TextPrompt<T> : IPrompt<T>
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
        /// Gets or sets the message for invalid choices.
        /// </summary>
        public string InvalidChoiceMessage { get; set; } = "[red]Please select one of the available options[/]";

        /// <summary>
        /// Gets or sets a value indicating whether input should
        /// be hidden in the console.
        /// </summary>
        public bool IsSecret { get; set; }

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
            _prompt = prompt;
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
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            return console.RunExclusive(() =>
            {
                var promptStyle = PromptStyle ?? Style.Plain;
                var converter = Converter ?? TypeConverterHelper.ConvertToString;
                var choices = Choices.Select(choice => converter(choice)).ToList();
                var choiceMap = Choices.ToDictionary(choice => converter(choice), choice => choice, _comparer);

                WritePrompt(console);

                while (true)
                {
                    var input = console.ReadLine(promptStyle, IsSecret, choices);

                    // Nothing entered?
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        if (DefaultValue != null)
                        {
                            console.Write(IsSecret ? "******" : converter(DefaultValue.Value), promptStyle);
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
                    else if (!TypeConverterHelper.TryConvertFromString<T>(input, out result) || result == null)
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
            });
        }

        /// <summary>
        /// Writes the prompt to the console.
        /// </summary>
        /// <param name="console">The console to write the prompt to.</param>
        private void WritePrompt(IAnsiConsole console)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            var builder = new StringBuilder();
            builder.Append(_prompt.TrimEnd());

            if (ShowChoices && Choices.Count > 0)
            {
                var converter = Converter ?? TypeConverterHelper.ConvertToString;
                var choices = string.Join("/", Choices.Select(choice => converter(choice)));
                builder.AppendFormat(CultureInfo.InvariantCulture, " [blue][[{0}]][/]", choices);
            }

            if (ShowDefaultValue && DefaultValue != null)
            {
                var converter = Converter ?? TypeConverterHelper.ConvertToString;
                builder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " [green]({0})[/]",
                    IsSecret ? "******" : converter(DefaultValue.Value));
            }

            var markup = builder.ToString().Trim();
            if (!markup.EndsWith("?", StringComparison.OrdinalIgnoreCase) &&
                !markup.EndsWith(":", StringComparison.OrdinalIgnoreCase))
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
}
