namespace Spectre.Console.Tests;

public static class StreamExtensions
{
    extension(Stream stream)
    {
        public string ReadText()
        {
            ArgumentNullException.ThrowIfNull(stream);

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}