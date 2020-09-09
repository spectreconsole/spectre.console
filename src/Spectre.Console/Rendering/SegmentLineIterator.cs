using System.Collections;
using System.Collections.Generic;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// An iterator for <see cref="SegmentLine"/> collections.
    /// </summary>
    public sealed class SegmentLineIterator : IEnumerator<Segment>
    {
        private readonly List<SegmentLine> _lines;
        private int _currentLine;
        private int _currentIndex;
        private bool _lineBreakEmitted;

        /// <summary>
        /// Gets the current segment.
        /// </summary>
        public Segment Current { get; private set; }

        /// <inheritdoc/>
        object? IEnumerator.Current => Current;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentLineIterator"/> class.
        /// </summary>
        /// <param name="lines">The lines to iterate.</param>
        public SegmentLineIterator(IEnumerable<SegmentLine> lines)
        {
            if (lines is null)
            {
                throw new System.ArgumentNullException(nameof(lines));
            }

            _currentLine = 0;
            _currentIndex = -1;
            _lines = new List<SegmentLine>(lines);

            Current = Segment.Empty;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (_currentLine > _lines.Count - 1)
            {
                return false;
            }

            _currentIndex++;

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
                _currentLine++;
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
                    _currentLine++;
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

        /// <inheritdoc/>
        public void Reset()
        {
            _currentLine = 0;
            _currentIndex = -1;

            Current = Segment.Empty;
        }
    }
}
