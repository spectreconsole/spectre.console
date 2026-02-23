using System.IO;

namespace Spectre.Console.Ansi.Tests;

public sealed class AnsiFixture
{
    private StringWriter _output { get; }

    public AnsiCapabilities Capabilities { get; }
    public AnsiWriter Writer { get; }
    public AnsiMarkup Markup { get; }

    public string Output => _output.ToString();

    public AnsiFixture(AnsiCapabilities? settings = null)
    {
        _output = new StringWriter();

        Capabilities = settings ?? new AnsiCapabilities
        {
            Ansi = true,
            Links = true,
            ColorSystem = ColorSystem.TrueColor,
            AlternateBuffer = true,
        };

        Writer = new AnsiWriter(_output, Capabilities);
        Markup = new AnsiMarkup(Writer);
    }
}