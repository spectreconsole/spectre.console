namespace Spectre.Console;

internal sealed class EncoderCapabilities : IReadOnlyCapabilities
{
    public ColorSystem ColorSystem { get; }

    public bool Ansi => false;
    public bool Links => false;
    public bool Legacy => false;
    public bool IsTerminal => false;
    public bool Interactive => false;
    public bool Unicode => true;
    public bool AlternateBuffer => false;

    public EncoderCapabilities(ColorSystem colors)
    {
        ColorSystem = colors;
    }
}