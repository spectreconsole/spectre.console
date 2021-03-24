namespace Spectre.Console.Testing
{
    public sealed class FakeCapabilities : IReadOnlyCapabilities
    {
        public bool Ansi { get; set; }

        public bool Links { get; set; }

        public bool Legacy { get; set; }

        public bool Tty { get; set; }

        public bool Interactive { get; set; }

        public bool Unicode { get; set; }
    }
}
