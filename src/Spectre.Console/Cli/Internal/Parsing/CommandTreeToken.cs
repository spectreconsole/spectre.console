namespace Spectre.Console.Cli
{
    internal sealed class CommandTreeToken
    {
        public Kind TokenKind { get; }
        public int Position { get; }
        public string Value { get; }
        public string Representation { get; }
        public bool IsGrouped { get; set; }

        public enum Kind
        {
            String,
            LongOption,
            ShortOption,
            Remaining,
        }

        public CommandTreeToken(Kind kind, int position, string value, string representation)
        {
            TokenKind = kind;
            Position = position;
            Value = value;
            Representation = representation;
        }
    }
}
