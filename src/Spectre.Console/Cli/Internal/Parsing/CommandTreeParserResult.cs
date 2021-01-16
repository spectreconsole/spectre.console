namespace Spectre.Console.Cli
{
    // Consider removing this in favor for value tuples at some point.
    internal sealed class CommandTreeParserResult
    {
        public CommandTree? Tree { get; }
        public IRemainingArguments Remaining { get; }

        public CommandTreeParserResult(CommandTree? tree, IRemainingArguments remaining)
        {
            Tree = tree;
            Remaining = remaining;
        }
    }
}
