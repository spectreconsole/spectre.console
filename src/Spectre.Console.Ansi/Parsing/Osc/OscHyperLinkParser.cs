namespace Spectre.Console.Ansi;

internal static class OscHyperLinkParser
{
    public static OscCommand? Parse(ReadOnlySpan<char> buffer)
    {
        string? id = null;

        // The URI is everything after the first ';'. Per the OSC 8 spec the params
        // section precedes it, and the URI itself may legally contain ';'
        var separator = buffer.IndexOf(';');
        if (separator < 0)
        {
            // OSC 8 requires the params section, even an empty one, so there's no URI here
            return null;
        }

        var parameters = buffer[..separator];
        var uri = buffer[(separator + 1)..];

        // An empty URI closes the hyperlink, whatever the params say
        if (uri.IsEmpty)
        {
            return new OscCommand.HyperLinkEnd();
        }

        // Params are a ':'-separated list of key=value pairs.
        while (!parameters.IsEmpty)
        {
            var colonIndex = parameters.IndexOf(':');
            var pair = colonIndex < 0 ? parameters : parameters[..colonIndex];

            var equalsIndex = pair.IndexOf('=');
            if (equalsIndex >= 0 && pair[..equalsIndex].Trim() is "id")
            {
                var value = pair[(equalsIndex + 1)..].Trim();
                id = value.IsEmpty ? null : value.ToString();
            }

            if (colonIndex < 0)
            {
                break;
            }

            parameters = parameters[(colonIndex + 1)..];
        }

        return new OscCommand.HyperLinkStart(Id: id, Uri: uri.ToString());
    }
}