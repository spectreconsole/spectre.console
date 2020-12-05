using System.Collections.Generic;
using System.Linq;
using Spectre.Console.Internal;

namespace Spectre.Console.Rendering
{
    internal sealed class LiveRenderable : Renderable
    {
        private readonly object _lock = new object();
        private IRenderable? _renderable;
        private int? _height;

        public void SetRenderable(IRenderable renderable)
        {
            lock (_lock)
            {
                _renderable = renderable;
            }
        }

        public IRenderable PositionCursor()
        {
            lock (_lock)
            {
                if (_height == null)
                {
                    return new ControlSequence(string.Empty);
                }

                return new ControlSequence("\r" + "\u001b[1A".Repeat(_height.Value - 1));
            }
        }

        public IRenderable RestoreCursor()
        {
            lock (_lock)
            {
                if (_height == null)
                {
                    return new ControlSequence(string.Empty);
                }

                return new ControlSequence("\r\u001b[2K" + "\u001b[1A\u001b[2K".Repeat(_height.Value - 1));
            }
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            lock (_lock)
            {
                if (_renderable != null)
                {
                    var segments = _renderable.Render(context, maxWidth);
                    var lines = Segment.SplitLines(context, segments);

                    _height = lines.Count;

                    var result = new List<Segment>();
                    foreach (var (_, _, last, line) in lines.Enumerate())
                    {
                        foreach (var item in line)
                        {
                            result.Add(item);
                        }

                        if (!last)
                        {
                            result.Add(Segment.LineBreak);
                        }
                    }

                    return result;
                }

                _height = 0;
                return Enumerable.Empty<Segment>();
            }
        }
    }
}
