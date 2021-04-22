using System.Collections.Generic;
using static Spectre.Console.AnsiSequences;

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
                    return new ControlCode(string.Empty);
                }

                var linesToMoveUp = _shape.Value.Height - 1;
                return new ControlCode("\r" + CUU(linesToMoveUp));
            }
        }

        public IRenderable RestoreCursor()
        {
            lock (_lock)
            {
                if (_shape == null)
                {
                    return new ControlCode(string.Empty);
                }

                var linesToClear = _shape.Value.Height - 1;
                return new ControlCode("\r" + EL(2) + (CUU(1) + EL(2)).Repeat(linesToClear));
            }
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            lock (_lock)
            {
                if (_renderable != null)
                {
                    var segments = _renderable.Render(context, maxWidth);
                    var lines = Segment.SplitLines(segments);

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
