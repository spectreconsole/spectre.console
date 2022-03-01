namespace Spectre.Console;

internal sealed class DefaultInput : IAnsiConsoleInput
{
    private readonly Profile _profile;

    public DefaultInput(Profile profile)
    {
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
    }

    public bool IsKeyAvailable()
    {
        if (!_profile.Capabilities.Interactive)
        {
            throw new InvalidOperationException("Failed to read input in non-interactive mode.");
        }

        return System.Console.KeyAvailable;
    }

    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        if (!_profile.Capabilities.Interactive)
        {
            throw new InvalidOperationException("Failed to read input in non-interactive mode.");
        }

        return System.Console.ReadKey(intercept);
    }

    public async Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
    {
        if (!_profile.Capabilities.Interactive)
        {
            throw new InvalidOperationException("Failed to read input in non-interactive mode.");
        }

        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            if (System.Console.KeyAvailable)
            {
                break;
            }

            await Task.Delay(5, cancellationToken).ConfigureAwait(false);
        }

        return ReadKey(intercept);
    }
}