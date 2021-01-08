using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Spectre.Console.Internal;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a list prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    public sealed class MultiSelectionPrompt<T> : IPrompt<List<T>>
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
        /// Gets or sets a value indicating whether or not
        /// at least one selection is required.
        /// </summary>
        public bool Required { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectionPrompt{T}"/> class.
        /// </summary>
        public MultiSelectionPrompt()
        {
            Choices = new List<T>();
        }

        /// <inheritdoc/>
        public List<T> Show(IAnsiConsole console)
        {
            if (!console.Capabilities.SupportsInteraction)
            {
                throw new NotSupportedException(
                    "Cannot show multi selection prompt since the current " +
                    "terminal isn't interactive.");
            }

            if (!console.Capabilities.SupportsAnsi)
            {
                throw new NotSupportedException(
                    "Cannot show multi selection prompt since the current " +
                    "terminal does not support ANSI escape sequences.");
            }

            var converter = Converter ?? TypeConverterHelper.ConvertToString;

            var list = new RenderableMultiSelectionList<T>(console, Title, PageSize, Choices, converter);
            using (new RenderHookScope(console, list))
            {
                console.Cursor.Hide();
                list.Redraw();

                while (true)
                {
                    var key = console.Input.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (Required && list.Selections.Count == 0)
                        {
                            continue;
                        }

                        break;
                    }

                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        list.Select();
                        list.Redraw();
                        continue;
                    }

                    if (list.Update(key.Key))
                    {
                        list.Redraw();
                    }
                }
            }

            list.Clear();
            console.Cursor.Show();

            return list.Selections
                .Select(index => Choices[index])
                .ToList();
        }
    }
}
