using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal abstract class RenderableList<T> : IRenderHook
        where T : notnull
    {
        private readonly LiveRenderable _live;
        private readonly object _lock;
        private readonly IAnsiConsole _console;
        private readonly int _requestedPageSize;
        private readonly List<RenderableListItem<T>> _choices;
        private int _index;

        public RenderableListItem<T> Current => _choices[_index];
        public IReadOnlyList<RenderableListItem<T>> Choices => _choices;

        public RenderableList(IAnsiConsole console, int requestedPageSize, IEnumerable<RenderableListItem<T>> choices)
        {
            _console = console;
            _requestedPageSize = requestedPageSize;
            _choices = new List<RenderableListItem<T>>(choices ?? throw new ArgumentNullException(nameof(choices)));
            _live = new LiveRenderable();
            _lock = new object();
            _index = 0;
        }

        protected abstract int CalculatePageSize(int requestedPageSize);
        protected abstract IRenderable Build(int pointerIndex, bool scrollable, IEnumerable<(int Original, int Index, RenderableListItem<T> Item)> choices);

        public void Clear()
        {
            _console.Write(_live.RestoreCursor());
        }

        public void Redraw()
        {
            _console.Write(new ControlCode(string.Empty));
        }

        public bool Update(ConsoleKey key)
        {
            var index = key switch
            {
                ConsoleKey.UpArrow => _index - 1,
                ConsoleKey.DownArrow => _index + 1,
                ConsoleKey.Home => 0,
                ConsoleKey.End => _choices.Count - 1,
                ConsoleKey.PageUp => _index - CalculatePageSize(_requestedPageSize),
                ConsoleKey.PageDown => _index + CalculatePageSize(_requestedPageSize),
                _ => _index,
            };

            index = index.Clamp(0, _choices.Count - 1);
            if (index != _index)
            {
                _index = index;
                Build();
                return true;
            }

            return false;
        }

        public IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables)
        {
            lock (_lock)
            {
                if (!_live.HasRenderable)
                {
                    Build();
                }

                yield return _live.PositionCursor();

                foreach (var renderable in renderables)
                {
                    yield return renderable;
                }

                yield return _live;
            }
        }

        protected void Build()
        {
            var pageSize = CalculatePageSize(_requestedPageSize);
            var middleOfList = pageSize / 2;

            var skip = 0;
            var take = _choices.Count;
            var pointer = _index;

            var scrollable = _choices.Count > pageSize;
            if (scrollable)
            {
                skip = Math.Max(0, _index - middleOfList);
                take = Math.Min(pageSize, _choices.Count - skip);

                if (_choices.Count - _index < middleOfList)
                {
                    // Pointer should be below the end of the list
                    var diff = middleOfList - (_choices.Count - _index);
                    skip -= diff;
                    take += diff;
                    pointer = middleOfList + diff;
                }
                else
                {
                    // Take skip into account
                    pointer -= skip;
                }
            }

            // Build the list
            _live.SetRenderable(Build(
                pointer,
                scrollable,
                _choices.Skip(skip).Take(take)
                .Enumerate()
                .Select(x => (skip + x.Index, x.Index, x.Item))));
        }
    }
}
