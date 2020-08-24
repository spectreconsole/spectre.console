using System.Collections;
using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    internal sealed class SegmentLineEnumerator : IEnumerable<Segment>
    {
        private readonly List<SegmentLine> _lines;

        public SegmentLineEnumerator(List<SegmentLine> lines)
        {
            _lines = lines;
        }

        public IEnumerator<Segment> GetEnumerator()
        {
            return new SegmentLineIterator(_lines);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
