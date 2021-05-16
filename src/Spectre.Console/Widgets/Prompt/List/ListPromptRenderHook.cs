using System;
using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class ListPromptRenderHook<T> : IRenderHook
        where T : notnull
    {
        private readonly LiveRenderable _live;
        private readonly object _lock;
        private readonly IAnsiConsole _console;
        private readonly Func<IRenderable> _builder;
        private bool _dirty;

        public ListPromptRenderHook(
            IAnsiConsole console,
            Func<IRenderable> builder)
        {
            _live = new LiveRenderable();
            _lock = new object();
            _console = console;
            _builder = builder;
            _dirty = true;
        }

        public void Clear()
        {
            _console.Write(_live.RestoreCursor());
        }

        public void Refresh()
        {
            _dirty = true;
            _console.Write(new ControlCode(string.Empty));
        }

        public IEnumerable<IRenderable> Process(RenderContext context, IEnumerable<IRenderable> renderables)
        {
            lock (_lock)
            {
                if (!_live.HasRenderable || _dirty)
                {
                    _live.SetRenderable(_builder());
                    _dirty = false;
                }

                yield return _live.PositionCursor();

                foreach (var renderable in renderables)
                {
                    yield return renderable;
                }

                yield return _live;
            }
        }
    }
}
