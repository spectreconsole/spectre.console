using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Represents a multi selection list prompt.
    /// </summary>
    /// <typeparam name="T">The prompt result type.</typeparam>
    public sealed class MultiSelectionPrompt<T> : IPrompt<List<T>>, IListPromptStrategy<T>
        where T : notnull
    {
        private readonly ListPromptTree<T> _tree;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

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
        /// Gets or sets the converter to get the display string for a choice. By default
        /// the corresponding <see cref="TypeConverter"/> is used.
        /// </summary>
        public Func<T, string>? Converter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not
        /// at least one selection is required.
        /// </summary>
        public bool Required { get; set; } = true;

        /// <summary>
        /// Gets or sets the text that will be displayed if there are more choices to show.
        /// </summary>
        public string? MoreChoicesText { get; set; }

        /// <summary>
        /// Gets or sets the text that instructs the user of how to select items.
        /// </summary>
        public string? InstructionsText { get; set; }

        /// <summary>
        /// Gets or sets the selection mode.
        /// Defaults to <see cref="SelectionMode.Leaf"/>.
        /// </summary>
        public SelectionMode Mode { get; set; } = SelectionMode.Leaf;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiSelectionPrompt{T}"/> class.
        /// </summary>
        public MultiSelectionPrompt()
        {
            _tree = new ListPromptTree<T>();
        }

        /// <summary>
        /// Adds a choice.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>A <see cref="IMultiSelectionItem{T}"/> so that multiple calls can be chained.</returns>
        public IMultiSelectionItem<T> AddChoice(T item)
        {
            var node = new ListPromptItem<T>(item);
            _tree.Add(node);
            return node;
        }

        /// <inheritdoc/>
        public List<T> Show(IAnsiConsole console)
        {
            // Create the list prompt
            var prompt = new ListPrompt<T>(console, this);
            var result = prompt.Show(_tree, PageSize);

            if (Mode == SelectionMode.Leaf)
            {
                return result.Items
                    .Where(x => x.Selected && x.Children.Count == 0)
                    .Select(x => x.Data)
                    .ToList();
            }

            return result.Items
                .Where(x => x.Selected)
                .Select(x => x.Data)
                .ToList();
        }

        /// <inheritdoc/>
        ListPromptInputResult IListPromptStrategy<T>.HandleInput(ConsoleKeyInfo key, ListPromptState<T> state)
        {
            if (key.Key == ConsoleKey.Enter)
            {
                if (Required && state.Items.None(x => x.Selected))
                {
                    // Selection not permitted
                    return ListPromptInputResult.None;
                }

                // Submit
                return ListPromptInputResult.Submit;
            }

            if (key.Key == ConsoleKey.Spacebar)
            {
                var current = state.Items[state.Index];
                var select = !current.Selected;

                if (Mode == SelectionMode.Leaf)
                {
                    // Select the node and all it's children
                    foreach (var item in current.Traverse(includeSelf: true))
                    {
                        item.Selected = select;
                    }

                    // Visit every parent and evaluate if it's selection
                    // status need to be updated
                    var parent = current.Parent;
                    while (parent != null)
                    {
                        parent.Selected = parent.Traverse(includeSelf: false).All(x => x.Selected);
                        parent = parent.Parent;
                    }
                }
                else
                {
                    current.Selected = !current.Selected;
                }

                // Refresh the list
                return ListPromptInputResult.Refresh;
            }

            return ListPromptInputResult.None;
        }

        /// <inheritdoc/>
        int IListPromptStrategy<T>.CalculatePageSize(IAnsiConsole console, int totalItemCount, int requestedPageSize)
        {
            // The instructions take up two rows including a blank line
            var extra = 2;
            if (Title != null)
            {
                // Title takes up two rows including a blank line
                extra += 2;
            }

            // Scrolling?
            if (totalItemCount > requestedPageSize)
            {
                // The scrolling instructions takes up one row
                extra++;
            }

            var pageSize = requestedPageSize;
            if (pageSize > console.Profile.Height - extra)
            {
                pageSize = console.Profile.Height - extra;
            }

            return pageSize;
        }

        /// <inheritdoc/>
        IRenderable IListPromptStrategy<T>.Render(IAnsiConsole console, bool scrollable, int cursorIndex, IEnumerable<(int Index, ListPromptItem<T> Node)> items)
        {
            var list = new List<IRenderable>();
            var highlightStyle = HighlightStyle ?? new Style(foreground: Color.Blue);

            if (Title != null)
            {
                list.Add(new Markup(Title));
            }

            var grid = new Grid();
            grid.AddColumn(new GridColumn().Padding(0, 0, 1, 0).NoWrap());

            if (Title != null)
            {
                grid.AddEmptyRow();
            }

            foreach (var item in items)
            {
                var current = item.Index == cursorIndex;
                var style = current ? highlightStyle : Style.Plain;

                var indent = new string(' ', item.Node.Depth * 2);
                var prompt = item.Index == cursorIndex ? ListPromptConstants.Arrow : new string(' ', ListPromptConstants.Arrow.Length);

                var text = (Converter ?? TypeConverterHelper.ConvertToString)?.Invoke(item.Node.Data) ?? item.Node.Data.ToString() ?? "?";
                if (current)
                {
                    text = text.RemoveMarkup();
                }

                var checkbox = item.Node.Selected
                    ? (item.Node.IsGroup && Mode == SelectionMode.Leaf
                        ? ListPromptConstants.GroupSelectedCheckbox : ListPromptConstants.SelectedCheckbox)
                    : ListPromptConstants.Checkbox;

                grid.AddRow(new Markup(indent + prompt + " " + checkbox + " " + text, style));
            }

            list.Add(grid);
            list.Add(Text.Empty);

            if (scrollable)
            {
                // There are more choices
                list.Add(new Markup(MoreChoicesText ?? ListPromptConstants.MoreChoicesMarkup));
            }

            // Instructions
            list.Add(new Markup(InstructionsText ?? ListPromptConstants.InstructionsMarkup));

            // Combine all items
            return new Rows(list);
        }
    }
}
