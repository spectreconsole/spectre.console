using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="MultiSelectionPrompt{T}"/>.
    /// </summary>
    public static class MultiSelectionPromptExtensions
    {
        /// <summary>
        /// Adds a choice.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choice">The choice to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> AddChoice<T>(this MultiSelectionPrompt<T> obj, T choice)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Choices.Add(choice);
            return obj;
        }

        /// <summary>
        /// Adds multiple choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> AddChoices<T>(this MultiSelectionPrompt<T> obj, params T[] choices)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Choices.AddRange(choices);
            return obj;
        }

        /// <summary>
        /// Adds multiple choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> AddChoices<T>(this MultiSelectionPrompt<T> obj, IEnumerable<T> choices)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Choices.AddRange(choices);
            return obj;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="title">The title markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> Title<T>(this MultiSelectionPrompt<T> obj, string? title)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Title = title;
            return obj;
        }

        /// <summary>
        /// Sets how many choices that are displayed to the user.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="pageSize">The number of choices that are displayed to the user.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> PageSize<T>(this MultiSelectionPrompt<T> obj, int pageSize)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (pageSize <= 2)
            {
                throw new ArgumentException("Page size must be greater or equal to 3.", nameof(pageSize));
            }

            obj.PageSize = pageSize;
            return obj;
        }

        /// <summary>
        /// Requires no choice to be selected.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> NotRequired<T>(this MultiSelectionPrompt<T> obj)
        {
            return Required(obj, false);
        }

        /// <summary>
        /// Requires a choice to be selected.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> Required<T>(this MultiSelectionPrompt<T> obj)
        {
            return Required(obj, true);
        }

        /// <summary>
        /// Sets a value indicating whether or not at least one choice must be selected.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="required">Whether or not at least one choice must be selected.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> Required<T>(this MultiSelectionPrompt<T> obj, bool required)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Required = required;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice.
        /// </summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="displaySelector">The function to get a display string for a given choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static MultiSelectionPrompt<T> UseConverter<T>(this MultiSelectionPrompt<T> obj, Func<T, string>? displaySelector)
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
