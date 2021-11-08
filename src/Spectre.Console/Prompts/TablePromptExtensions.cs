using System;
using System.Collections.Generic;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="TablePrompt{T}"/>.
    /// </summary>
    public static class TablePromptExtensions
    {
        /// <summary>
        /// Adds multiple columns to the table.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="columns">The columns to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> AddColumns<T>(this TablePrompt<T> obj, params string[] columns)
            where T : notnull
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            foreach (var column in columns)
            {
                obj.AddColumn(column);
            }

            return obj;
        }

        /// <summary>
        /// Adds multiple choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> AddChoices<T>(this TablePrompt<T> obj, params T[] choices)
            where T : notnull
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            foreach (var choice in choices)
            {
                obj.AddChoice(choice);
            }

            return obj;
        }

        /// <summary>
        /// Adds multiple choices.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="choices">The choices to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> AddChoices<T>(this TablePrompt<T> obj, IEnumerable<T> choices)
            where T : notnull
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            foreach (var choice in choices)
            {
                obj.AddChoice(choice);
            }

            return obj;
        }

        /// <summary>
        /// Sets the title.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="title">The title markup text.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> Title<T>(this TablePrompt<T> obj, string? title)
            where T : notnull
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Title = title;
            return obj;
        }

        /// <summary>
        /// Sets the highlight style of the selected choice.
        /// </summary>
        /// <typeparam name="T">The prompt result type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="highlightStyle">The highlight style of the selected choice.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> HighlightStyle<T>(this TablePrompt<T> obj, Style highlightStyle)
            where T : notnull
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.HighlightStyle = highlightStyle;
            return obj;
        }

        /// <summary>
        /// Sets the function to create a display string for a given choice and column.
        /// </summary>
        /// <typeparam name="T">The prompt type.</typeparam>
        /// <param name="obj">The table prompt.</param>
        /// <param name="displaySelector">The function to get a display string for a given choice and column.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TablePrompt<T> UseConverter<T>(this TablePrompt<T> obj, Func<T, int, string>? displaySelector)
            where T : notnull
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
