namespace Spectre.Console;

internal static class AnsiWriterExtensions
{
    public static void Write(this AnsiWriter writer, IAnsiConsole console, IRenderable? renderable)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(console);

        if (renderable == null)
        {
            return;
        }

        foreach (var segment in renderable.GetSegments(console))
        {
            if (segment.IsControlCode)
            {
                writer.Write(segment.Text);
                continue;
            }

            var parts = segment.Text.NormalizeNewLines().Split(['\n']);
            foreach (var (_, _, last, part) in parts.Enumerate())
            {
                if (!string.IsNullOrEmpty(part))
                {
                    writer.Write(part, segment.Style);
                }

                if (!last)
                {
                    writer.WriteLine();
                }
            }
        }
    }
}