namespace Spectre.Console;

internal static class InteractionDetector
{
    public static bool IsInteractive(InteractionSupport interaction)
    {
        return IsInteractive(
            interaction,
            System.Console.IsInputRedirected,
            System.Console.IsOutputRedirected,
            System.Console.IsErrorRedirected);
    }

    internal static bool IsInteractive(
        InteractionSupport interaction,
        bool isInputRedirected,
        bool isOutputRedirected,
        bool isErrorRedirected)
    {
        var interactive = interaction == InteractionSupport.Yes;

        if (interaction == InteractionSupport.Detect)
        {
            // Consider the console interactive only when input, output and error are not redirected.
            interactive =
                !isInputRedirected &&
                !isOutputRedirected &&
                !isErrorRedirected;
        }

        return interactive;
    }
}