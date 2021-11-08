using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a prompt in table format.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    public sealed class TablePrompt<T> : IPrompt<T>, IListPromptStrategy<T>
        where T : notnull
    {
        private readonly ListPromptTree<T> _tree;
        private readonly List<string> _columns;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the highlight style of the selected choice.
        /// </summary>
        public Style? HighlightStyle { get; set; }

        /// <summary>
        /// Gets or sets the converter to get the display string for a choice and column.
        /// By default the corresponding <see cref="TypeConverter"/> is used.
        /// </summary>
        public Func<T, int, string>? Converter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TablePrompt{T}"/> class.
        /// </summary>
        public TablePrompt()
        {
            _tree = new ListPromptTree<T>(EqualityComparer<T>.Default);
            _columns = new List<string>();
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="column">The column to add.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TablePrompt<T> AddColumn(string column)
        {
            _columns.Add(column);
            return this;
        }

        /// <summary>
        /// Adds a choice.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>A <see cref="ISelectionItem{T}"/> so that multiple calls can be chained.</returns>
        public ISelectionItem<T> AddChoice(T item)
        {
            var node = new ListPromptItem<T>(item);
            _tree.Add(node);
            return node;
        }

        /// <inheritdoc/>
        public T Show(IAnsiConsole console)
        {
            return ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public async Task<T> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
        {
            // Create the list prompt
            var prompt = new ListPrompt<T>(console, this);
            var result = await prompt.Show(_tree, cancellationToken).ConfigureAwait(false);

            // Return the selected item
            return result.Items[result.Index].Data;
        }

        /// <inheritdoc/>
        ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
        {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                return ListPromptInputResult.Submit;
            }

            return ListPromptInputResult.None;
        }

        /// <inheritdoc/>
        int IListPromptStrategy<T>.CalculatePageSize(IAnsiConsole console, int totalItemCount, int requestedPageSize)
        {
            // Display the entire table
            return totalItemCount;
        }

        /// <inheritdoc/>
        IRenderable IListPromptStrategy<T>.Render(IAnsiConsole console, bool scrollable, int cursorIndex, IEnumerable<(int Index, ListPromptItem<T> Node)> items)
        {
            var highlightStyle = HighlightStyle ?? new Style(foreground: Color.Blue);
            var table = new Table();

            if (Title != null)
            {
                table.Title = new TableTitle(Title);
            }

            foreach (var column in _columns)
            {
                table.AddColumn(column);
            }

            foreach (var item in items)
            {
                var current = item.Index == cursorIndex;
                var style = current ? highlightStyle : Style.Plain;

                var columns = new List<IRenderable>();

                for (var columnIndex = 0; columnIndex < _columns.Count; columnIndex++)
                {
                    var text = Converter?.Invoke(item.Node.Data, columnIndex) ?? TypeConverterHelper.ConvertToString(item.Node.Data) ?? item.Node.Data.ToString() ?? "?";
                    if (current)
                    {
                        text = text.RemoveMarkup().EscapeMarkup();
                    }

                    columns.Add(new Markup(text, style));
                }

                table.AddRow(columns);
            }

            return table;
        }
    }
}
