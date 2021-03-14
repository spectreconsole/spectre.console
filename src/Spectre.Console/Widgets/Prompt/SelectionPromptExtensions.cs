using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="SelectionPrompt{T}"/>.
    /// </summary>
    public static class SelectionPromptExtensions
    {
        /// <summary>
        /// Adds a choice.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="choice">The choice to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SelectionPrompt<T> AddChoice<T>(this SelectionPrompt<T> obj, T choice)
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
        public static SelectionPrompt<T> AddChoices<T>(this SelectionPrompt<T> obj, params T[] choices)
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
        public static SelectionPrompt<T> AddChoices<T>(this SelectionPrompt<T> obj, IEnumerable<T> choices)
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
        public static SelectionPrompt<T> Title<T>(this SelectionPrompt<T> obj, string? title)
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
        public static SelectionPrompt<T> PageSize<T>(this SelectionPrompt<T> obj, int pageSize)
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
        /// Sets the highlight style of the selected choice.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="highlightStyle">The highlight style of the selected choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SelectionPrompt<T> HighlightStyle<T>(this SelectionPrompt<T> obj, Style highlightStyle)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.HighlightStyle = highlightStyle;
            return obj;
        }

        /// <summary>
        /// Sets the text that will be displayed if there are more choices to show.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="text">The text to display.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SelectionPrompt<T> MoreChoicesText<T>(this SelectionPrompt<T> obj, string? text)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.MoreChoicesText = text;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice.
        /// </summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="obj">The prompt.</param>
        /// <param name="displaySelector">The function to get a display string for a given choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static SelectionPrompt<T> UseConverter<T>(this SelectionPrompt<T> obj, Func<T, string>? displaySelector)
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
