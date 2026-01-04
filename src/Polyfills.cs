// Borrowed from https://github.com/SimonCropp/Polyfill/blob/main/src/Polyfill/ArgumentExceptions/ArgumentNullExceptionPolyfill.cs#L13
// which is licensed under MIT. For some reason the compiler isn't picking that one up...

namespace Spectre.Console;

internal static class Polyfills
{
    extension(ArgumentNullException)
    {
#if NETSTANDARD
        //Link: https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception.throwifnull?view=net-10.0#system-argumentnullexception-throwifnull(system-object-system-string)
        [DoesNotReturn]
        public static void ThrowIfNull([NotNull] object? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
#endif
    }
}