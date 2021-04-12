using System;

namespace Spectre.Console
{
    internal sealed class DefaultInput : IAnsiConsoleInput
    {
        private readonly Profile _profile;

        public DefaultInput(Profile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (!_profile.Capabilities.Interactive)
            {
                throw new InvalidOperationException("Failed to read input in non-interactive mode.");
            }

            return System.Console.ReadKey(intercept);
        }
    }
}
