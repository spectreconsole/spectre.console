namespace Spectre.Console.Ansi;

internal static class OscHyperLinkParser
{
    public static OscCommand? Parse(ReadOnlySpan<char> buffer)
    {
        string? id = null;

        // Find the final ';' which separates params from the URI.
        var lastSemicolon = buffer.LastIndexOf(';');
        if (lastSemicolon < 0)
        {
            return new OscCommand.HyperLinkStart(null, buffer.ToString());
        }

        var parameters = buffer[..lastSemicolon];
        var uri = buffer[(lastSemicolon + 1)..];

        // Parse key=value pairs from the params section.
        while (!parameters.IsEmpty)
        {
            var equalsIndex = parameters.IndexOf('=');
            if (equalsIndex < 0)
            {
                break;
            }

            var key = parameters[..equalsIndex].Trim();
            parameters = parameters[(equalsIndex + 1)..];

            var semicolonIndex = parameters.IndexOf(';');
            var value = semicolonIndex < 0
                ? parameters.Trim()
                : parameters[..semicolonIndex].Trim();

            if (key.SequenceEqual("id"))
            {
                id = value.IsEmpty ? null : value.ToString();
            }

            if (semicolonIndex < 0)
            {
                break;
            }

            parameters = parameters[(semicolonIndex + 1)..];
        }

        if (id == null && uri.Length == 0)
        {
            return new OscCommand.HyperLinkEnd();
        }

        return uri.Length == 0
            ? null
            : new OscCommand.HyperLinkStart(id, uri.ToString());
    }
}