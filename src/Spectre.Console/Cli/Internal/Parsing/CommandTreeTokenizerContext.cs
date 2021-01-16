using System.Collections.Generic;
using System.Text;

namespace Spectre.Console.Cli
{
    internal sealed class CommandTreeTokenizerContext
    {
        private readonly StringBuilder _builder;
        private readonly List<string> _remaining;

        public CommandTreeTokenizer.Mode Mode { get; set; }
        public IReadOnlyList<string> Remaining => _remaining;

        public CommandTreeTokenizerContext()
        {
            _builder = new StringBuilder();
            _remaining = new List<string>();
        }

        public void AddRemaining(char character)
        {
            if (Mode == CommandTreeTokenizer.Mode.Remaining)
            {
                _builder.Append(character);
            }
        }

        public void AddRemaining(string text)
        {
            if (Mode == CommandTreeTokenizer.Mode.Remaining)
            {
                _builder.Append(text);
            }
        }

        public void FlushRemaining()
        {
            if (Mode == CommandTreeTokenizer.Mode.Remaining)
            {
                if (_builder.Length > 0)
                {
                    _remaining.Add(_builder.ToString());
                    _builder.Clear();
                }
            }
        }
    }
}