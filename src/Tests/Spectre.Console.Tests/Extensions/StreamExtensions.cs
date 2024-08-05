namespace Spectre.Console.Tests;

public static class StreamExtensions
{
    public static string ReadText(this Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}
