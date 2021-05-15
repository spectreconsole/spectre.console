using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class RenderableMultiSelectionList<T> : RenderableList<T>
        where T : notnull
    {
        private const string Checkbox = "[[ ]]";
        private const string SelectedCheckbox = "[[X]]";
        private const string MoreChoicesText = "[grey](Move up and down to reveal more choices)[/]";
        private const string InstructionsText = "[grey](Press <space> to select, <enter> to accept)[/]";

        private readonly IAnsiConsole _console;
        private readonly string? _title;
        private readonly Func<T, string> _converter;
        private readonly Markup _moreChoices;
        private readonly Markup _instructions;
        private readonly Style _highlightStyle;

        public HashSet<RenderableListItem<T>> Selections { get; set; }

        public RenderableMultiSelectionList(
            IAnsiConsole console, string? title, int pageSize,
            IEnumerable<RenderableListItem<T>> choices, HashSet<int> selections,
            Func<T, string> converter,
            Style? highlightStyle, string? moreChoicesText, string? instructionsText)
            : base(console, pageSize, choices)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _title = title;
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            _highlightStyle = highlightStyle ?? new Style(foreground: Color.Blue);
            _moreChoices = new Markup(moreChoicesText ?? MoreChoicesText);
            _instructions = new Markup(instructionsText ?? InstructionsText);

            Selections = new HashSet<RenderableListItem<T>>(
                GetSelectedChoices(selections),
                new RenderableListItemComparer<T>());
        }

        public void Select()
        {
            if (Selections.Contains(Current))
            {
                Selections.Remove(Current);
            }
            else
            {
                Selections.Add(Current);
            }

            Build();
        }

        protected override int CalculatePageSize(int requestedPageSize)
        {
            var pageSize = requestedPageSize;
            if (pageSize > _console.Profile.Height - 5)
            {
                pageSize = _console.Profile.Height - 5;
            }

            return pageSize;
        }

        protected override IRenderable Build(int pointerIndex, bool scrollable,
            IEnumerable<(int Original, int Index, RenderableListItem<T> Item)> choices)
        {
            var list = new List<IRenderable>();

            if (_title != null)
            {
                list.Add(new Markup(_title));
            }

            var grid = new Grid();
            grid.AddColumn(new GridColumn().Padding(0, 0, 1, 0).NoWrap());
            grid.AddColumn(new GridColumn().Padding(0, 0, 0, 0));

            if (_title != null)
            {
                grid.AddEmptyRow();
            }

            foreach (var choice in choices)
            {
                var current = choice.Index == pointerIndex;
                var selected = Selections.Contains(choice.Item);

                var prompt = choice.Index == pointerIndex ? "> " : "  ";
                var checkbox = selected ? SelectedCheckbox : Checkbox;

                var text = _converter?.Invoke(choice.Item.Data) ?? choice.Item.Data.ToString() ?? "?";
                var style = current ? _highlightStyle : Style.Plain;

                var item = current
                    ? new Text(text.RemoveMarkup(), style)
                    : (IRenderable)new Markup(text, style);

                grid.AddRow(new Markup(prompt + checkbox, style), item);
            }

            list.Add(grid);
            list.Add(Text.Empty);

            if (scrollable)
            {
                // (Move up and down to reveal more choices)
                list.Add(_moreChoices);
            }

            // (Press <space> to select)
            list.Add(_instructions);

            return new Rows(list);
        }

        private IEnumerable<RenderableListItem<T>> GetSelectedChoices(HashSet<int> selections)
        {
            foreach (var selection in selections)
            {
                var item = Choices.FirstOrDefault(x => x.Index == selection);
                if (item != null)
                {
                    yield return item;
                }
            }
        }
    }
}