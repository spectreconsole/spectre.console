using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Spectre.Console.Cli
{
    internal class CommandTreeParserContext
    {
        private readonly List<string> _args;
        private readonly Dictionary<string, List<string?>> _remaining;

        public IReadOnlyList<string> Arguments => _args;
        public int CurrentArgumentPosition { get; private set; }
        public CommandTreeParser.State State { get; set; }
        public ParsingMode ParsingMode { get; }

        public CommandTreeParserContext(IEnumerable<string> args, ParsingMode parsingMode)
        {
            _args = new List<string>(args);
            _remaining = new Dictionary<string, List<string?>>(StringComparer.Ordinal);

            ParsingMode = parsingMode;
        }

        public void ResetArgumentPosition()
        {
            CurrentArgumentPosition = 0;
        }

        public void IncreaseArgumentPosition()
        {
            CurrentArgumentPosition++;
        }

        public void AddRemainingArgument(string key, string? value)
        {
            if (State == CommandTreeParser.State.Remaining || ParsingMode == ParsingMode.Relaxed)
            {
                if (!_remaining.ContainsKey(key))
                {
                    _remaining.Add(key, new List<string?>());
                }

                _remaining[key].Add(value);
            }
        }

        [SuppressMessage("Style", "IDE0004:Remove Unnecessary Cast", Justification = "Bug in analyzer?")]
        public ILookup<string, string?> GetRemainingArguments()
        {
            return _remaining
                .SelectMany(pair => pair.Value, (pair, value) => new { pair.Key, value })
                .ToLookup(pair => pair.Key, pair => (string?)pair.value);
        }
    }
}