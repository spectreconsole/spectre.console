using Spectre.Console;

namespace Generator.Commands;

public static class AsciiCastExtensions
{
    extension(IAnsiConsole ansiConsole)
    {
        public AsciiCastOut WrapWithAsciiCastRecorder()
        {
            AsciiCastOut castRecorder = new(ansiConsole.Profile.Out);
            ansiConsole.Profile.Out = castRecorder;

            return castRecorder;
        }
    }
}