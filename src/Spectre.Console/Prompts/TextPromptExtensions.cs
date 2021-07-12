using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="TextPrompt{T}"/>.
    /// </summary>
    public static class TextPromptExtensions
    {
        /// <summary>
        /// Allow empty input.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> AllowEmpty<T>(this TextPrompt<T> obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.AllowEmpty = true;
            return obj;
        }

        /// <summary>
        /// Sets the prompt style.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="style">The prompt style.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> PromptStyle<T>(this TextPrompt<T> obj, Style style)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (style is null)
            {
                throw new ArgumentNullException(nameof(style));
            }

            obj.PromptStyle = style;
            return obj;
        }

        /// <summary>
        /// Show or hide choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="show">Whether or not choices should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> ShowChoices<T>(this TextPrompt<T> obj, bool show)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ShowChoices = show;
            return obj;
        }

        /// <summary>
        /// Shows choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> ShowChoices<T>(this TextPrompt<T> obj)
        {
            return ShowChoices(obj, true);
        }

        /// <summary>
        /// Hides choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> HideChoices<T>(this TextPrompt<T> obj)
        {
            return ShowChoices(obj, false);
        }

        /// <summary>
        /// Show or hide the default value.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="show">Whether or not the default value should be visible.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> ShowDefaultValue<T>(this TextPrompt<T> obj, bool show)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ShowDefaultValue = show;
            return obj;
        }

        /// <summary>
        /// Shows the default value.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> ShowDefaultValue<T>(this TextPrompt<T> obj)
        {
            return ShowDefaultValue(obj, true);
        }

        /// <summary>
        /// Hides the default value.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> HideDefaultValue<T>(this TextPrompt<T> obj)
        {
            return ShowDefaultValue(obj, false);
        }

        /// <summary>
        /// Sets the validation error message for the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="message">The validation error message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> ValidationErrorMessage<T>(this TextPrompt<T> obj, string message)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.ValidationErrorMessage = message;
            return obj;
        }

        /// <summary>
        /// Sets the "invalid choice" message for the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="message">The "invalid choice" message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> InvalidChoiceMessage<T>(this TextPrompt<T> obj, string message)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.InvalidChoiceMessage = message;
            return obj;
        }

        /// <summary>
        /// Sets the default value of the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="value">The default value.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> DefaultValue<T>(this TextPrompt<T> obj, T value)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.DefaultValue = new DefaultPromptValue<T>(value);
            return obj;
        }

        /// <summary>
        /// Sets the validation criteria for the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="validator">The validation criteria.</param>
        /// <param name="message">The validation error message.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> Validate<T>(this TextPrompt<T> obj, Func<T, bool> validator, string? message = null)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

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
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="validator">The validation criteria.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> Validate<T>(this TextPrompt<T> obj, Func<T, ValidationResult> validator)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Validator = validator;

            return obj;
        }

        /// <summary>
        /// Adds a choice to the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choice">The choice to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> AddChoice<T>(this TextPrompt<T> obj, T choice)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Choices.Add(choice);
            return obj;
        }

        /// <summary>
        /// Adds multiple choices to the prompt.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> AddChoices<T>(this TextPrompt<T> obj, IEnumerable<T> choices)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (choices is null)
            {
                throw new ArgumentNullException(nameof(choices));
            }

            foreach (var choice in choices)
            {
                obj.Choices.Add(choice);
            }

            return obj;
        }

        /// <summary>
        /// Replaces prompt user input with asterixes in the console.
        /// </summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> Secret<T>(this TextPrompt<T> obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.IsSecret = true;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice.
        /// </summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="displaySelector">The function to get a display string for a given choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TextPrompt<T> WithConverter<T>(this TextPrompt<T> obj, Func<T, string>? displaySelector)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Converter = displaySelector;
            return obj;
        }
    }
}
