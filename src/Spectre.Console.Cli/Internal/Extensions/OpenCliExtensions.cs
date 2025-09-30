#if NETSTANDARD2_0
namespace System.IO;

// Polyfills needed for OpenCli.
// This can be removed once me migrate over to the Polyfill library.
internal static class OpenCliExtensions
{
    public static Task<string> ReadToEndAsync(this StreamReader reader, CancellationToken cancellationToken)
    {
        return reader.ReadToEndAsync();
    }
}
#endif