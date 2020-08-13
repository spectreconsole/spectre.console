using System.Collections;
using System.Collections.Generic;

namespace Spectre.Console.Composition
{
    internal sealed class SegmentLineIterator : IEnumerator<Segment>
    {
        private readonly List<SegmentLine> _lines;
        private int _currentLine;
        private int _currentIndex;
        private bool _lineBreakEmitted;

        public Segment Current { get; private set; }
        object? IEnumerator.Current => Current;

        public SegmentLineIterator(List<SegmentLine> lines)
        {
            _currentLine = 0;
            _currentIndex = -1;
            _lines = lines;

            Current = Segment.Empty;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_currentLine > _lines.Count - 1)
            {
                return false;
            }

            _currentIndex += 1;

            // Did we go past the end of the line?
            if (_currentIndex > _lines[_currentLine].Count - 1)
            {
                // We haven't just emitted a line break?
                if (!_lineBreakEmitted)
                {
                    // Got any more lines?
                    if (_currentIndex + 1 > _lines[_currentLine].Count - 1)
                    {
                        // Only emit a line break if the next one isn't a line break.
                        if ((_currentLine + 1 <= _lines.Count - 1)
                            && _lines[_currentLine + 1].Count > 0
                            && !_lines[_currentLine + 1][0].IsLineBreak)
                        {
                            _lineBreakEmitted = true;
                            Current = Segment.LineBreak;
                            return true;
                        }
                    }
                }

                // Increase the line and reset the index.
                _currentLine += 1;
                _currentIndex = 0;

                _lineBreakEmitted = false;

                // No more lines?
                if (_currentLine > _lines.Count - 1)
                {
                    return false;
                }

                // Nothing on the line?
                while (_currentIndex > _lines[_currentLine].Count - 1)
                {
                    _currentLine += 1;
                    _currentIndex = 0;

                    if (_currentLine > _lines.Count - 1)
                    {
                        return false;
                    }
                }
            }

            // Reset the flag
            _lineBreakEmitted = false;

            Current = _lines[_currentLine][_currentIndex];
            return true;
        }

        public void Reset()
        {
            _currentLine = 0;
            _currentIndex = -1;

            Current = Segment.Empty;
        }
    }
}
