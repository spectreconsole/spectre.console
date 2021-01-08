using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class RenderableMultiSelectionList<T> : RenderableList<T>
    {
        private const string Checkbox = "[[ ]]";
        private const string SelectedCheckbox = "[[X]]";

        private readonly IAnsiConsole _console;
        private readonly string? _title;
        private readonly Style _highlightStyle;

        public HashSet<int> Selections { get; set; }

        public RenderableMultiSelectionList(
            IAnsiConsole console, string? title, int pageSize,
            List<T> choices, Func<T, string>? converter)
            : base(console, pageSize, choices, converter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _title = title;
            _highlightStyle = new Style(foreground: Color.Blue);

            Selections = new HashSet<int>();
        }

        public void Select()
        {
            if (Selections.Contains(Index))
            {
                Selections.Remove(Index);
            }
            else
            {
                Selections.Add(Index);
            }

            Build();
        }

        protected override int CalculatePageSize(int requestedPageSize)
        {
            var pageSize = requestedPageSize;
            if (pageSize > _console.Height - 5)
            {
                pageSize = _console.Height - 5;
            }

            return pageSize;
        }

        protected override IRenderable Build(int pointerIndex, bool scrollable, IEnumerable<(int Original, int Index, string Item)> choices)
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
                var selected = Selections.Contains(choice.Original);

                var prompt = choice.Index == pointerIndex ? "> " : "  ";
                var checkbox = selected ? SelectedCheckbox : Checkbox;

                var style = current ? _highlightStyle : Style.Plain;

                grid.AddRow(
                    new Markup($"{prompt}{checkbox}", style),
                    new Markup(choice.Item.EscapeMarkup(), style));
            }

            list.Add(grid);
            list.Add(Text.Empty);

            if (scrollable)
            {
                list.Add(new Markup("[grey](Move up and down to reveal more choices)[/]"));
            }

            list.Add(new Markup("[grey](Press <space> to select)[/]"));

            return new Rows(list);
        }
    }
}
