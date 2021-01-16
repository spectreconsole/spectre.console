using System;

namespace Spectre.Console
{
    internal sealed class MarkupToken
    {
        public MarkupTokenKind Kind { get; }
        public string Value { get; }
        public int Position { get; set; }

        public MarkupToken(MarkupTokenKind kind, string value, int position)
        {
            Kind = kind;
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Position = position;
        }
    }
}
