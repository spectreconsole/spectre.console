using Spectre.Console;

namespace Emojis;

public static class Program
{
    public static void Main(string[] args)
    {
        // Show a known emoji
        RenderEmoji();

        // Show a remapped emoji
        Emoji.Remap("globe_showing_europe_africa", Emoji.Known.GrinningFaceWithSmilingEyes);
        RenderEmoji();
    }

    private static void RenderEmoji()
    {
        AnsiConsole.Write(
            new Panel("[yellow]Hello :globe_showing_europe_africa:![/]")
                .RoundedBorder());
    }
}
