using System;
using System.Collections.Generic;
using System.ComponentModel;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a list prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    public sealed class SelectionPrompt<T> : IPrompt<T>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets the choices.
        /// </summary>
        public List<T> Choices { get; }

        /// <summary>
        /// Gets or sets the converter to get the display string for a choice. By default
        /// the corresponding <see cref="TypeConverter"/> is used.
        /// </summary>
        public Func<T, string>? Converter { get; set; } = TypeConverterHelper.ConvertToString;

        /// <summary>
        /// Gets or sets the page size.
        /// Defaults to <c>10</c>.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the highlight style of the selected choice.
        /// </summary>
        public Style? HighlightStyle { get; set; }

        /// <summary>
        /// Gets or sets the text that will be displayed if there are more choices to show.
        /// </summary>
        public string? MoreChoicesText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionPrompt{T}"/> class.
        /// </summary>
        public SelectionPrompt()
        {
            Choices = new List<T>();
        }

        /// <inheritdoc/>
        T IPrompt<T>.Show(IAnsiConsole console)
        {
            if (!console.Profile.Capabilities.Interactive)
            {
                throw new NotSupportedException(
                    "Cannot show selection prompt since the current " +
                    "terminal isn't interactive.");
            }

            if (!console.Profile.Capabilities.Ansi)
            {
                throw new NotSupportedException(
                    "Cannot show selection prompt since the current " +
                    "terminal does not support ANSI escape sequences.");
            }

            return console.RunExclusive(() =>
            {
                var converter = Converter ?? TypeConverterHelper.ConvertToString;
                var list = new RenderableSelectionList<T>(
                    console, Title, PageSize, Choices,
                    converter, HighlightStyle, MoreChoicesText);

                using (new RenderHookScope(console, list))
                {
                    console.Cursor.Hide();
                    list.Redraw();

                    while (true)
                    {
                        var key = console.Input.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
                        {
                            break;
                        }

                        if (list.Update(key.Key))
                        {
                            list.Redraw();
                        }
                    }
                }

                list.Clear();
                console.Cursor.Show();

                return Choices[list.Index];
            });
        }
    }
}
