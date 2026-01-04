namespace Spectre.Console.Tests;

public static class StreamExtensions
{
    public static string ReadText(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}