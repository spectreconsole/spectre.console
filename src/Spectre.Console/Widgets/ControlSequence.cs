using System.Collections.Generic;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class ControlSequence : Renderable
    {
        private readonly Segment _segment;

        public ControlSequence(string control)
        {
            _segment = Segment.Control(control);
        }

        protected override Measurement Measure(RenderContext context, int maxWidth)
        {
            return new Measurement(0, 0);
        }

        protected override IEnumerable<Segment> Render(RenderContext context, int maxWidth)
        {
            yield return _segment;
        }
    }
}
