namespace Spectre.Console;

internal static class InteractionDetector
{
    public static bool IsInteractive(InteractionSupport interaction)
    {
        var interactive = interaction == InteractionSupport.Yes;
        if (interaction == InteractionSupport.Detect)
        {
            // TODO: We need a better way of checking this
            interactive = !System.Console.IsInputRedirected;
        }

        return interactive;
    }
}