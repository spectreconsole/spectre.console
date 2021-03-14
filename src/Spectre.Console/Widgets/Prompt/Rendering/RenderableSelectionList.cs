using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class RenderableSelectionList<T> : RenderableList<T>
    {
        private const string Prompt = ">";
        private const string MoreChoicesText = "[grey](Move up and down to reveal more choices)[/]";

        private readonly IAnsiConsole _console;
        private readonly string? _title;
        private readonly Markup _moreChoices;
        private readonly Style _highlightStyle;

        public RenderableSelectionList(
            IAnsiConsole console, string? title, int requestedPageSize,
            List<T> choices, Func<T, string>? converter, Style? highlightStyle,
            string? moreChoices)
            : base(console, requestedPageSize, choices, converter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _title = title;
            _highlightStyle = highlightStyle ?? new Style(foreground: Color.Blue);
            _moreChoices = new Markup(moreChoices ?? MoreChoicesText);
        }

        protected override int CalculatePageSize(int requestedPageSize)
        {
            var pageSize = requestedPageSize;
            if (pageSize > _console.Profile.Height - 4)
            {
                pageSize = _console.Profile.Height - 4;
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

                var prompt = choice.Index == pointerIndex ? Prompt : string.Empty;
                var style = current ? _highlightStyle : Style.Plain;

                var item = current
                    ? new Text(choice.Item.RemoveMarkup(), style)
                    : (IRenderable)new Markup(choice.Item, style);

                grid.AddRow(new Markup(prompt, style), item);
            }

            list.Add(grid);

            if (scrollable)
            {
                // (Move up and down to reveal more choices)
                list.Add(Text.Empty);
                list.Add(_moreChoices);
            }

            return new Rows(list);
        }
    }
}
