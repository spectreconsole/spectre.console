using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    internal sealed class LiveRenderable : Renderable
    {
        private readonly object _lock = new object();
        private IRenderable? _renderable;
        private SegmentShape? _shape;

        public bool HasRenderable => _renderable != null;

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
                if (_shape == null)
                {
                    return new ControlSequence(string.Empty);
                }

                return new ControlSequence("\r" + "\u001b[1A".Repeat(_shape.Value.Height - 1));
            }
        }

        public IRenderable RestoreCursor()
        {
            lock (_lock)
            {
                if (_shape == null)
                {
                    return new ControlSequence(string.Empty);
                }

                return new ControlSequence("\r\u001b[2K" + "\u001b[1A\u001b[2K".Repeat(_shape.Value.Height - 1));
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

                    var shape = SegmentShape.Calculate(context, lines);
                    _shape = _shape == null ? shape : _shape.Value.Inflate(shape);
                    _shape.Value.Apply(context, ref lines);

                    foreach (var (_, _, last, line) in lines.Enumerate())
                    {
                        foreach (var item in line)
                        {
                            yield return item;
                        }

                        if (!last)
                        {
                            yield return Segment.LineBreak;
                        }
                    }

                    yield break;
                }

                _shape = null;
            }
        }
    }
}
